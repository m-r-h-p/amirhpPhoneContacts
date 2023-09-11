
using System;
using System.ComponentModel.DataAnnotations;

namespace PhoneContacts.Data.Models
{
    public class User: Human
    {
        [MaxLength(50)]
        [Required]
        public string Password { get; set; }

        [Required]
        public int VerficationCode { get; set; } 

        [Required]
        public bool IsActive { get; set; } = false;



        //Navigation Property
        public List<ContactToUser> ContactToUsers { get; set; }

    }
}
