using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{

    /// <summary>
    /// A class meant to represent the Entity-Framework code-generated class Librarian locally in memory.
    /// Inherits from MPerson
    /// </summary>
    public class MLibrarian : MPerson
    {
        public string Phone { get; set; }  //Phone number of Librarian/MLibrarian
        public string UserID { get; set; }  //UserID of Librarian/MLibrarian
        public string Password { get; set; }  //Password of Librarian/MLibrarian

        /// <summary>
        /// Constructor for MLibrarian
        /// Copies the fields and properties of Librarian in the argument
        /// </summary>
        /// <param name="l">Librarian used to initialize fields and properties</param>
        public MLibrarian(Librarian l)
        {
            this.ID = l.PersonId;
            this.FirstName = l.Person.FirstName;
            this.LastName = l.Person.LastName;
            this.Phone = l.Phone;
            this.UserID = l.UserId;
            this.Password = l.Password;
        }

        /// <summary>
        /// ToStrign override
        /// </summary>
        /// <returns>returns a string describing this object</returns>
        public override string ToString()
        {
            string result = base.ToString();
            result += string.Format("\tPhone: {0}", Phone);
            return result;
        }
    }
}
