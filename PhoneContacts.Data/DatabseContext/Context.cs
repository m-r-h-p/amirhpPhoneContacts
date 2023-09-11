
using Microsoft.EntityFrameworkCore;
using PhoneContacts.Data.Models;

namespace PhoneContacts.Data.DatabseContext
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactToUser> ContactToUser { get; set; }

    }
    // add-migration "Adding FamilyStatus To User Model"
    // update-databse
}
