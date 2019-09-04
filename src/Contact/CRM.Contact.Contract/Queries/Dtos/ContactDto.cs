using System;

namespace CRM.Contact.Contract.Queries.Dtos
{
    public class ContactDto
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Description { get; set; }
    }
}