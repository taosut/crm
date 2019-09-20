using System;
using CRM.Shared.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using LeadApi;
using Microsoft.Extensions.Logging;

namespace CRM.Graph.Gateway.Services
{
    public class LeadService : ServiceBase, ILeadService
    {
        private readonly LeadApi.Lead.LeadClient _leadClient;
        private readonly ILogger<LeadService> _logger;

        public LeadService(GrpcClientFactory clientFactory, ILoggerFactory loggerFactory) : base()
        {
            _leadClient = clientFactory.CreateClient<LeadApi.Lead.LeadClient>(nameof(Lead.LeadClient));
            _logger = loggerFactory.CreateLogger<LeadService>();
        }
        public LeadsResponse GetLeads()
        {
            var metaData = new Metadata();
            var result = _leadClient.getLeads(new Empty(), metaData);
            _logger.LogInformation(result.Leads.ToString());
            return result;
        }
    }
}
