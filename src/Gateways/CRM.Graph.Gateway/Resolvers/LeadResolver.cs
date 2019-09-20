using System.Collections.Generic;
using System.Linq;
using CRM.Graph.Gateway.Services;

namespace CRM.Graph.Gateway.Resolvers
{
    public class LeadResolver
    {
        private readonly ILeadService _leadService;

        public LeadResolver(ILeadService leadService)
        {
            _leadService = leadService;
        }

        public IList<LeadApi.LeadInformation> GetLeads()
        {
            var leads = _leadService.GetLeads().Leads;
            return leads;
        }
    }
}
