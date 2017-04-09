using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    /// <summary>
    /// A class meant to represent the Entity-Framework code-generated class Author locally in memory.
    /// Inherits from MPerson class.
    /// </summary>
    public class MAuthor : MPerson
    {
        public string Bio { get; set; } //Bio of MAuthor/Author
        public Author dbAuthor { get; private set; } //A read-only Property of this object's associated Author
        public SortedSet<Book> Books { get; private set; } //A SortedSet that contains all Book objects associated with this MAuthor/Author 

        /// <summary>
        /// Constructor for MAuthor
        /// Copies the fields and Properties of Author passed in argument
        /// </summary>
        /// <param name="a">Author used to initialize fields and Properties</param>
        public MAuthor(Author a)
        {
            dbAuthor = a;

            this.ID = a.PersonId;
            this.FirstName = a.Person.FirstName;
            this.LastName = a.Person.LastName;
            this.Bio = a.Bio;
            this.Books = new SortedSet<Book>(a.Books);
        }

        /// <summary>
        /// Checks whether this object's Properties contains the string passed in the argument.
        /// </summary>
        /// <param name="token">string used to make comparison</param>
        /// <returns>Returns true if any of the Properties contains the token, false otherwise</returns>
        public bool ContainsToken(string token)
        {
            if (FirstName.ToLower().Contains(token.ToLower()))
                return true;

            if (LastName.ToLower().Contains(token.ToLower()))
                return true;

            if (Bio.ToLower().Contains(token.ToLower()))
                return true;

            return false;
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>string description of this object</returns>
        public override string ToString()
        {
            string result = base.ToString() + Bio;
            return result;
        }
    }
}
