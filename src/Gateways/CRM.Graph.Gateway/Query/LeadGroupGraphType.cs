using System.Collections.Generic;
using CRM.Graph.Gateway.Services;
using CRM.Graph.Gateway.Types;
using GraphQL.Types;
using LeadApi;

namespace CRM.Graph.Gateway.Query
{
    public class LeadGroupGraphType : ObjectGraphType
    {
        public LeadGroupGraphType(ILeadService leadService)
        {
            Name = "Lead";

            Field<ListGraphType<LeadType>>("leads", resolve: (context) =>
            {
                var a = leadService.GetLeads();
                return a.Leads;
            });
        }
    }
}
