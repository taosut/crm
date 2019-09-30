using CRM.Graph.Gateway.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace CRM.Graph.Gateway.Types
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            // descriptor.Authorize();
            descriptor.Field<ContactResolver>(t => t.ListContacts(default))
                .UsePaging<ContactType>();
                //.Type<ListType<ContactType>>();
        }
    }
}
