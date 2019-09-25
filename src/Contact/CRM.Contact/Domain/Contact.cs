using System;

namespace CRM.Contact.Domain
{
    public class Contact
    {
        public Guid ContactId { get; set; }
        public ContactType ContactType { get; set; }
        public String FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public String Title { get; set; }
        public String Company { get; set; }
        public String Description { get; set; }
        public String Photo { get; set; }
        public ContactInformation ContactInfo { get; set; }
        public Address Address { get; set; }
    }

    public enum Gender
    {
        Mr = 0,
        Mrs = 1,
        Ms = 2
    }

    public enum ContactType
    {
        Lead = 0,
        Contact = 1,

    }
}
