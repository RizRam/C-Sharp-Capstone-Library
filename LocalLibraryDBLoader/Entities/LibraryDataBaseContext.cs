using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LocalLibraryDBLoader
{
    public class LibraryDataBaseContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Librarian> Librarians { get; set; } 
        public DbSet<Cardholder> Cardholders { get; set; }
        public DbSet<CheckOutLog> CheckOutLogs { get; set; }

        //Constructor
        public LibraryDataBaseContext() : base()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LibraryDataBaseContext>());
        }
    }
}
