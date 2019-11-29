using System;
using System.Collections.Generic;
using Serilog.Sinks.Loki.Labels;

namespace CRM.Shared.Logging
{
    public class LokiLogLabelProvider : ILogLabelProvider
    {
        private readonly string _applicationName;
        private readonly string _env;
        public LokiLogLabelProvider(string env, string applicationName)
        {
            _env = env ?? throw new ArgumentNullException("Env label cannot be null");
            _applicationName = applicationName ?? throw new ArgumentNullException("Application label cannot be null");

        }
        public IList<LokiLabel> GetLabels()
        {
            return new List<LokiLabel>
            {
                new LokiLabel("Application", _applicationName),
                new LokiLabel("Env", _env)
            };
        }
    }
}