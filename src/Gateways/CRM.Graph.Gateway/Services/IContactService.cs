using System.Threading.Tasks;
using CRM.Protobuf.Contacts.V1;

namespace CRM.Graph.Gateway.Services
{
    public interface IContactService
    {
        Task<ListContactsResponse> ListContacts();

        CreateContactResponse CreateContact(CreateContactRequest contact);
    }
}
