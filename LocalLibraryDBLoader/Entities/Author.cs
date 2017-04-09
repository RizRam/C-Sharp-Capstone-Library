using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace LocalLibraryDBLoader
{
    public partial class Author
    {
        [Key]
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public string Bio { get; set; }

        public virtual Person Person { get; set; }
    }
}
