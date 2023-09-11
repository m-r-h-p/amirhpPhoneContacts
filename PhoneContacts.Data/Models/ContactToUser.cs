using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneContacts.Data.Models
{
    public class ContactToUser:BaseModel
    {
        [ForeignKey("Users")]
        public long UserId { get; set; }

        [ForeignKey("Contact")]
        public long ContactId { get; set; }


        //Navigation Property
        public User Users { get; set; }
        public Contact Contact { get; set; }


    }
}
