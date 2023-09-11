using PhoneContacts.Data.DTOs;
using PhoneContacts.Data.Models;

namespace PhoneContacts.Business.Services.UserService
{
    public interface IUserServices
    {
        public Task<ReturnDTO> GetAllUsers();
        public Task<ReturnDTO> GetUsersById(long id);
        public Task<ReturnDTO> CreateUser(RegisterDTO user);
        public Task<ReturnDTO> LoginUser(string email,string password);
        public Task<ReturnDTO> UserActivition(string emailAddress,int activisionCode);
        public Task<Enum> UpdateUser(User user);
        public Task<string> IsEmailExist(string emailAddress);
        public Task<string> IsPhoneNumberExist(string phoneNumber);
    }
}
