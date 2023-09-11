using System.ComponentModel.DataAnnotations;

namespace PhoneContacts.Data.DTOs
{
    public class RegisterDTO
    {
        [MaxLength(250)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(250)]
        [Required]
        public string LastName { get; set; }

        [Required]
        public byte Age { get; set; }

        [MaxLength(50)]
        [Required]
        public string Password { get; set; }

        [MaxLength(250)]
        [Required]
        public string EmailAddress { get; set; }

        [MaxLength(11)]
        [Required]
        public string PhoneNumber { get; set; }
      
    }
}
