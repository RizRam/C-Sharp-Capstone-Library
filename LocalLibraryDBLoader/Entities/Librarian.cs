using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalLibraryDBLoader
{
    public partial class Librarian
    {
        [Key]
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public string Phone { get; set; }

        //[Index(IsUnique = true)]
        public string UserId { get; set; }
        public string Password { get; set; }

        public virtual Person Person { get; set; }
    }
}

