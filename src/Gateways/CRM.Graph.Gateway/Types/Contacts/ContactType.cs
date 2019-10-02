using HotChocolate.Types;
using CRM.Protobuf.Contacts.V1;

namespace CRM.Graph.Gateway.Types.Contacts
{
    public class ContactType : ObjectType<Contact>
    {
        protected override void Configure(IObjectTypeDescriptor<Contact> descriptor)
        {
            descriptor.Field(t => t.CalculateSize()).Ignore();
            descriptor.Field(t => t.Clone()).Ignore();
            descriptor.Field(t => t.Equals(null)).Ignore();

            descriptor.Field(t => t.ContactInfo).Type<ContactInformationType>();
            descriptor.Field(t => t.Address).Type<AddressType>();
        }
    }
}
