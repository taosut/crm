using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Contact.Domain;
using CRM.Shared.Repository;
using Dapper;
using OpenTracing;
using OpenTracing.Tag;

namespace CRM.Contact.DataAccess
{
    public class ContactRepository : IContactRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly ITracer _tracer;

        public ContactRepository(IUnitOfWork uow, ITracer tracer)
        {
            _uow = uow;
            _tracer = tracer;
        }
        public async Task<IList<Domain.Contact>> ListContactsAsync()
        {
            using (var tracingScope = _tracer.BuildSpan(this.ToString()).StartActive(finishSpanOnDispose: true))
            {
                var sql = @"SELECT contact_id as ContactId, contact_type as ContactType, first_name as FirstName,
                                last_name as LastName, middle_name as MiddleName, title, company, description, photo,
                                cast(1 as decimal) as contactInfoId, email, mobile,work_phone as WorkPhone, home_phone as HomePhone,
                                cast(1 as decimal) addressid, mailing_street as MailingStreet, mailing_city as MailingCity,
                                mailing_zipcode as MailingZipcode, mailing_state as MailingState, mailing_country as MailingCountry
                            FROM contact";

                tracingScope.Span.Log("Processing method: 'GetLeadsAsync'");
                tracingScope.Span.SetTag(Tags.DbStatement, sql);
                try
                {
                    var contacts = await _uow.Connection.QueryAsync<Domain.Contact, ContactInformation, Address, Domain.Contact>(sql, (contact, contactInfo, address) =>
                    {
                        contact.ContactInfo = contactInfo;
                        contact.Address = address;
                        return contact;
                    }, splitOn: "contactInfoId, addressid");

                    return contacts.AsList();
                }
                catch (Exception ex)
                {

                }
                return null;
            }
        }
    }
}
