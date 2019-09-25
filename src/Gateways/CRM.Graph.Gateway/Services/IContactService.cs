using CRM.Contact.V1;

namespace CRM.Graph.Gateway.Services
{
    public interface IContactService
    {
        ListContactsResponse ListContacts();
    }
}
