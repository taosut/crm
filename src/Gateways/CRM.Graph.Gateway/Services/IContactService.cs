using CRM.Protobuf.Contact.V1;

namespace CRM.Graph.Gateway.Services
{
    public interface IContactService
    {
        ListContactsResponse ListContacts();
    }
}
