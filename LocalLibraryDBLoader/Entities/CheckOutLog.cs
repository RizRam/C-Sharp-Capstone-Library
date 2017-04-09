using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace LocalLibraryDBLoader
{
    public partial class CheckOutLog
    {
        [Key]
        public int CheckOutLogId { get; set; }
        public DateTime CheckOutDate { get; set; }

        public virtual Cardholder Cardholder { get; set; }
        public virtual Book Book { get; set; }
    }
}
