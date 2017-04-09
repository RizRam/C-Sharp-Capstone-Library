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
    public partial class Book
    {
        [Key]       
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int NumPages { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
        public string Language { get; set; }
        public int NumberOfCopies { get; set; }

        public virtual Author Author { get; set; }

    }
}
