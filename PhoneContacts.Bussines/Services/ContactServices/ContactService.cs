using Microsoft.EntityFrameworkCore;
using PhoneContacts.Business.Messages;
using PhoneContacts.Data.DatabseContext;
using PhoneContacts.Data.DTOs;
using PhoneContacts.Data.Models;

namespace PhoneContacts.Business.Services.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly Context _context;
        public ContactService(Context context)
        {
            _context = context;
        }
        public async Task<ReturnDTO> CreateContactToThisUser(ContactDTO contact)
        {
            ReturnDTO returnDTO = new ReturnDTO();
            Contact NewContact = new Contact()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Age = contact.Age,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address,
                Avatar = contact.Avatar,
            };
            try
            {
                await _context.Contacts.AddAsync(NewContact);
                await _context.SaveChangesAsync();

                ContactToUser ctu = new ContactToUser()
                {
                    ContactId = NewContact.Id,
                    UserId = (long)contact.OwnerId
                };

                await _context.ContactToUser.AddAsync(ctu);
                await _context.SaveChangesAsync();



                returnDTO.data = new Data.DTOs.Data()
                {
                    values = null
                };
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.Success.ToString()
                };

                return returnDTO;
            }
            catch
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                };

                return returnDTO;
            }

        }

        public async Task<ReturnDTO> GetAllContactsOfThisUser(long userId)
        {
            List<ContactDTO> result = new List<ContactDTO>();
            ReturnDTO returnDTO = new ReturnDTO();

            if (userId > 0)
            {
                var query = await _context.ContactToUser.Include(c => c.Contact)
                    .Where(u => u.UserId == userId & u.Contact.IsDeleted == false)
                    .Select(u => u.Contact).ToListAsync();

                returnDTO.data = new Data.DTOs.Data()
                {
                    values = query
                };
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.Success.ToString()
                };


            }
            return returnDTO;
        }

        public async Task<ReturnDTO> RemoveContact(long contactId)
        {
            ReturnDTO returnDTO = new ReturnDTO();
            Contact contact = new Contact();
            if (contactId > 0)
            {
                try
                {
                    contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.IsDeleted != true);
                    if (contact != null)
                    {
                        contact.IsDeleted = true;
                        _context.Contacts.Update(contact);

                        await _context.SaveChangesAsync();

                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.Success.ToString()
                        };
                        return returnDTO;
                    }
                    else
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.ContactNotFound.ToString()
                        };
                        return returnDTO;
                    }
                }
                catch
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                    };
                    return returnDTO;
                }
            }
            else
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.ParameterIsNotValid.ToString()
                };
                return returnDTO;
            }
        }

        public async Task<ReturnDTO> UpdateContact(ContactDTO contact)
        {
            ReturnDTO returnDTO = new ReturnDTO();
            Contact ContactToUpdate = new Contact();
            try
            {
                ContactToUpdate = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);

                if (ContactToUpdate != null)
                {
                    ContactToUpdate.FirstName = contact.FirstName;
                    ContactToUpdate.LastName = contact.LastName;
                    ContactToUpdate.Age = contact.Age;
                    ContactToUpdate.Email = contact.Email;
                    ContactToUpdate.PhoneNumber = contact.PhoneNumber;
                    ContactToUpdate.Address = contact.Address;
                    ContactToUpdate.Avatar = contact.Avatar;

                    _context.Contacts.Update(ContactToUpdate);
                    await _context.SaveChangesAsync();

                    returnDTO.data = new Data.DTOs.Data()
                    {
                        values = ContactToUpdate
                    };
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.Success.ToString()
                    };
                    return returnDTO;
                }
                else
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.ContactNotFound.ToString()
                    };
                    return returnDTO;
                }
            }
            catch
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                };

                return returnDTO;
            }
        }
    }
}
