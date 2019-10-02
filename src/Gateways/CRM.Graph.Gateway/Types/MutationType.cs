using CRM.Graph.Gateway.Types.Contacts;
using HotChocolate.Types;

namespace CRM.Graph.Gateway.Types
{
    public sealed class MutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            RegisterContactResource(descriptor);
        }

        private static void RegisterContactResource(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<ContactResolver>(t => t.CreateNewContact(default))
                .Type<NonNullType<ContactType>>()
                .Argument("contact", a => a.Type<NonNullType<ContactInputType>>());
        }
    }
}
