using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Contact.Domain
{
    public interface IContactRepository
    {
        Task<IList<Contact>> ListContactsAsync();
    }
}
