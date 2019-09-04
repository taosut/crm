using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Contact.Api.Domain;
using CRM.Shared.Repository;
using Dapper;

namespace CRM.Contact.Api.DataAccess
{
    public class ContactRepository : IContactRepository
    {
        private readonly IUnitOfWork _uow;

        public ContactRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<Domain.Contact>> FindAllContacts()
        {
            var sql = @"SELECT contact_id ContactId, first_name FirstName, last_name LastName, 
                            middle_name MiddleName, company Company, email Email, description Description
                        FROM contact";
                        
            var contacts = await _uow.Connection.QueryAsync<Domain.Contact>(sql);
            return contacts.AsList();
        }
    }
}