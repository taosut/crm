using System;
using System.Threading.Tasks;
using CRM.DataContract.IntegrationEvents.Lead;
using CRM.Lead.Model;
using CRM.Shared.EventBus;
using CRM.Shared.ValidationModel;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LeadApi;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;
using static LeadApi.Lead;

namespace CRM.Lead.Api.Services
{
    public class LeadService : LeadBase
    {
        private readonly ILogger<LeadService> _logger;
        private readonly IValidator<CreateLeadRequest> _validator;
        private readonly ILeadRepository _leadRepo;
        private readonly IEventBus _bus;

        public LeadService(ILoggerFactory loggerFactory,
            IValidator<CreateLeadRequest> validator,
            ILeadRepository leadRepository,
            Shared.EventBus.Nats.INatsConnection connection,
            IEventBus bus)
        {
            _logger = loggerFactory.CreateLogger<LeadService>();
            _validator = validator;
            _leadRepo = leadRepository;
            _bus = bus;

            connection.TryConnect();
        }

        public override Task<PongReply> Ping(Empty request, ServerCallContext context)
        {
            var a =GlobalTracer.Instance;
            _logger.LogInformation("123456");
            
            _bus.Publish(new LeadCreatedEvent
            {
                Id = Guid.NewGuid()
            });
            return Task.FromResult(new PongReply
            {
                Message = "Pong"
            });
        }

        // [Authorize]
        public override async Task<LeadsResponse> getLeads(Empty request, ServerCallContext context)
        {
            var leads = await _leadRepo.GetLeadsAsync();
            var response = new LeadsResponse();
            response.Leads.AddRange(leads);

             _bus.Publish(new LeadCreatedEvent
            {
                Id = Guid.NewGuid()
            });

            return response;
        }

        public override async Task<LeadInformation> createLead(CreateLeadRequest request, ServerCallContext context)
        {
            await _validator.HandleValidation(request);
            return new LeadInformation();
        }
    }
}
