using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Contact.Api.Domain
{
    public interface IContactRepository
    {
        Task<List<Contact>> FindAllContacts();
    }
}