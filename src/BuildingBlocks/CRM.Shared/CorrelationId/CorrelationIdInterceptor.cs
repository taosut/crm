using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Microsoft.Extensions.Options;
using System;
using Serilog.Context;
using Microsoft.Extensions.Logging;

namespace CRM.Shared.CorrelationId
{
    public class CorrelationIdInterceptor : Interceptor
    {
        private readonly CorrelationIdOptions _options;
        private readonly ICorrelationContextFactory _correlationContextFactory;
        private readonly ILogger<CorrelationIdInterceptor> _logger;

        public CorrelationIdInterceptor(IOptions<CorrelationIdOptions> options,
            ICorrelationContextFactory correlationContextFactory,
            ILogger<CorrelationIdInterceptor> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _correlationContextFactory = correlationContextFactory ?? throw new ArgumentNullException(nameof(correlationContextFactory));
            _logger = logger;
        }
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var correlationId = SetCorrelationId(context);

            _correlationContextFactory.Create(correlationId, _options.Header);
            try
            {
                using (LogContext.PushProperty(_options.Header, correlationId))
                {
                    return await continuation(request, context);
                }
            }
            finally
            {
                _correlationContextFactory.Dispose();
            }
        }

        private StringValues SetCorrelationId(ServerCallContext context)
        {
            return context.RequestHeaders.Any(h => h.Key == _options.Header.ToLower())
                ? context.RequestHeaders.First(h => h.Key == _options.Header.ToLower()).Value
                : Guid.NewGuid().ToString(); //todo: check how can to get traceIdentifier ....
        }
    }
}
