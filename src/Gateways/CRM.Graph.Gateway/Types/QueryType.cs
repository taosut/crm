using CRM.Graph.Gateway.Resolvers;
using HotChocolate.Types;

namespace CRM.Graph.Gateway.Types
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<LeadResolver>(t => t.GetLeads()).Type<ListType<LeadType>>();
        }
    }
}
