using System;
using System.Text;
using Microsoft.Extensions.Logging;
using NATS.Client;
using System.Text.Json;
using System.Threading.Tasks;
using Polly.Retry;
using Polly;
using OpenTracing;
using OpenTracing.Propagation;
using System.Collections.Generic;
using OpenTracing.Tag;

namespace CRM.Shared.EventBus.Nats
{
    public class EventBusNats : IEventBus, IDisposable
    {
        private readonly ILogger<EventBusNats> _logger;
        private readonly INatsConnection _connection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITracer _tracer;

        public EventBusNats(INatsConnection connection, ILoggerFactory loggerFactory,
            IEventBusSubscriptionsManager subsManager,
            IServiceProvider serviceProvider,
            ITracer tracer)
        {
            _logger = loggerFactory.CreateLogger<EventBusNats>();
            _connection = connection;
            _subsManager = subsManager;
            _serviceProvider = serviceProvider;
            _tracer = tracer;
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var policy = RetryPolicy.Handle<NATSException>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                    });
            var eventName = @event.GetType().Name;

            using var tracingScope = _tracer.BuildSpan($"EventBus-Publish-{eventName}")
                .WithTag(Tags.SpanKind.Key, Tags.SpanKindServer)
                .WithTag(Tags.PeerHostname, Environment.MachineName)
                .StartActive(finishSpanOnDispose: true);
            var dictionary = new Dictionary<string, string>();
            _tracer.Inject(tracingScope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));
            
            @event.TracingKeys = dictionary;
            var msg = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(msg);

            policy.Execute(() =>
            {
                _connection.NatsConnection.Publish(eventName, body);
                _connection.NatsConnection.Flush();
            });
        }

        public void Subscribe<T, TH>(string queueName)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            DoInternalSubscription(eventName, queueName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddSubscription<T, TH>();
        }

        public void Dispose()
        {
            _subsManager.Clear();
        }

        private void DoInternalSubscription(string eventName, string queueName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_connection.IsConnected)
                {
                    _connection.TryConnect();
                }
                var sub = _connection.NatsConnection.SubscribeAsync(eventName, queueName);
                sub.MessageHandler += Consumer_Received();
                sub.Start();
            }
        }

        private EventHandler<MsgHandlerEventArgs> Consumer_Received()
        {
            return async (sender, args) =>
            {
                var eventName = args.Message.Subject;
                var message = Encoding.UTF8.GetString(args.Message.Data);
                var eventType = _subsManager.GetEventTypeByName(eventName);
                var integrationEvent = JsonSerializer.Deserialize(message, eventType) as IntegrationEvent;
                
                ISpanContext spanCtx = _tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(integrationEvent.TracingKeys));
                using var tracingScope = _tracer.BuildSpan($"EventBus-Consume-{eventName}")
                        .AsChildOf(spanCtx)
                        .WithTag(Tags.SpanKind, Tags.SpanKindConsumer)
                        .StartActive(finishSpanOnDispose: true);
                try
                {
                    await ProcessEvent(eventName, message);
                }
                catch (Exception ex)
                {
                    tracingScope.Span.SetTag(Tags.Error, true);
                    tracingScope.Span.Log(ex.Message);
                    _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
                }
            };
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing NATS event: {EventName}", eventName);
            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                if (!subscription.IsDynamic)
                {
                    var handler = _serviceProvider.GetService(subscription.HandlerType);
                    if (handler == null)
                    {
                        continue;
                    }
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType) as IntegrationEvent;
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
        }
    }
}