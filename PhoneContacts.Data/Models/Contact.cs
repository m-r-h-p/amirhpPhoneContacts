
namespace PhoneContacts.Data.Models
{
    public class Contact : Human

    {

        //Navigation Property
        public List<ContactToUser> ContactToUsers { get; set; }

    }
}
