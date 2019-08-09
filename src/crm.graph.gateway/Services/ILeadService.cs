using System.Collections.Generic;

namespace CRM.Graph.Gateway.Services
{
    public interface ILeadService
    {
        LeadApi.LeadsResponse GetLeads();
    }
}