using HotChocolate.Types;
using CRM.Protobuf.Contacts.V1;

namespace CRM.Graph.Gateway.Types.Contacts
{
    public class ContactInformationType : ObjectType<ContactInformation>
    {
        protected override void Configure(IObjectTypeDescriptor<ContactInformation> descriptor)
        {
            descriptor.Field(t => t.CalculateSize()).Ignore();
            descriptor.Field(t => t.Clone()).Ignore();
            descriptor.Field(t => t.Equals(null)).Ignore();
        }
    }
}
