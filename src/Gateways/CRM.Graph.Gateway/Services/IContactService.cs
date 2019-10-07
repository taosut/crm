using CRM.Protobuf.Contacts.V1;

namespace CRM.Graph.Gateway.Services
{
    public interface IContactService
    {
        ListContactsResponse ListContacts();

        CreateContactResponse CreateContact(CreateContactRequest contact);
    }
}
