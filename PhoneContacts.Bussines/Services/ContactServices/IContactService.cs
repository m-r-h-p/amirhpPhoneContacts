using PhoneContacts.Data.DTOs;

namespace PhoneContacts.Business.Services.ContactServices
{
    public interface IContactService
    {
        public Task<ReturnDTO> CreateContactToThisUser(ContactDTO contact);
        public Task<ReturnDTO> GetAllContactsOfThisUser(long userId);
        public Task<ReturnDTO> UpdateContact(ContactDTO contact);
        public Task<ReturnDTO> RemoveContact(long contactId);

    }
}
