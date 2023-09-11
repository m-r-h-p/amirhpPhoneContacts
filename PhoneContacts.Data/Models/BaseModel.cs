using System.ComponentModel.DataAnnotations;

namespace PhoneContacts.Data.Models
{
    public class BaseModel
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }=System.DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
