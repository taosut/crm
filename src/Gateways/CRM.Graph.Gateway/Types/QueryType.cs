using System;
using CRM.Graph.Gateway.Types.Contacts;
using CRM.Graph.Gateway.Types.Profiles;
using CRM.Protobuf.Profile.V1;
using HotChocolate.Types;

namespace CRM.Graph.Gateway.Types
{
    public sealed class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            RegisterContactResource(descriptor);

            // RegisterProfileResource(descriptor);
        }

        private static void RegisterContactResource(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<ContactResolver>(t => t.ListContacts())
                .Name("contacts")
                .Type<ListType<ContactType>>();

            descriptor.Field<ContactResolver>(t => t.GetContactById(default))
                .Name("contactById")
                .Type<ContactType>()
                .Argument("contactId", a => a.Type<StringType>());
        }

        // private static void RegisterProfileResource(IObjectTypeDescriptor descriptor)
        // {
        //     descriptor.Field("company")
        //         .Type<CompanyType>()
        //         .Resolver(() => new Company
        //         {
        //             Id = Guid.NewGuid().ToString(),
        //             CompanyName = "My Co"
        //         });
        // }
    }
}
