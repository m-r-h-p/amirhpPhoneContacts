﻿using System.ComponentModel.DataAnnotations;


namespace PhoneContacts.Data.Models
{

    public class Human: BaseModel
    {
        [MaxLength(250)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(250)]
        [Required]
        public string LastName { get; set; }

        public byte? Age { get; set; }

        [MaxLength(250)]
        [Required]
        public string Email { get; set; }

        [MaxLength(5)]
        public string? Avatar { get; set; }

        [MaxLength(11)]
        [Required]
        public string PhoneNumber { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

    }
}
