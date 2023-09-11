

namespace PhoneContacts.Data.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>() { "SuperAdmin", "Admin" };
    }
}
