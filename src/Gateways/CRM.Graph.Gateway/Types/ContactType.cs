using HotChocolate.Types;

namespace CRM.Graph.Gateway.Types
{
    public class ContactType : ObjectType<CRM.Protobuf.Contact.V1.Contact>
    {
        protected override void Configure(IObjectTypeDescriptor<Protobuf.Contact.V1.Contact> descriptor)
        {
            descriptor.Field(t => t.CalculateSize()).Ignore();
            descriptor.Field(t => t.Clone()).Ignore();
            descriptor.Field(t => t.Equals(null)).Ignore();
        }
    }
}
