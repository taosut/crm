using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.Shared.CorrelationId;
using CRM.Shared.ValidationModel;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Context;

namespace CRM.Shared.Interceptors
{
    public class ExceptionInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;
        private readonly CorrelationIdOptions _options;
        private readonly ICorrelationContextFactory _correlationContextFactory;

        public ExceptionInterceptor(IOptions<CorrelationIdOptions> options,
            ICorrelationContextFactory correlationContextFactory,
            ILogger<ExceptionInterceptor> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _correlationContextFactory = correlationContextFactory ?? throw new ArgumentNullException(nameof(correlationContextFactory));
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var correlationId = SetCorrelationId(context);
            _correlationContextFactory.Create(correlationId, _options.Header);

            using (LogContext.PushProperty(_options.Header, correlationId))
            {
                try
                {
                    return await continuation(request, context);
                }
                catch (ValidationException ex)
                {
                    _logger.LogError(ex, ex.ValidationResultModel.ToString());
                    throw new RpcException(new Status(StatusCode.Internal, ex.ValidationResultModel.ToString()));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw new RpcException(new Status(StatusCode.Internal, ex.Message));
                }
                finally
                {
                    _correlationContextFactory.Dispose();
                }
            }
        }

        private Guid SetCorrelationId(ServerCallContext context)
        {
            if (context.RequestHeaders.Any(h => h.Key == _options.Header.ToLower()) &&
                Guid.TryParse(context.RequestHeaders.First(h => h.Key == _options.Header.ToLower()).Value, out var correlationId))
            {
                return correlationId;
            }
            return Guid.NewGuid();
        }
    }
}
