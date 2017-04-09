using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    /// <summary>
    /// An extension to the Entity framework code-generated Book class of the LibraryDB.edmx
    /// </summary>
    public partial class Book : IComparable<Book>
    {
         /// <summary>
        /// Returns true if any copies of this Book is currently checked out.
        /// </summary>
        public bool IsCheckedOut
        {
            get
            {
                return CheckOutLogs.Count != 0;
            }
        }
        
        /// <summary>
        /// Returns the number of copies of this book that are available for check out.
        /// </summary>
        public int CopiesAvailable
        {
            get
            {
                return NumberOfCopies - CheckOutLogs.Count;
            }
        }

        /// <summary>
        /// Checks whether and of the Properties of this object contains the string passed in the argument
        /// </summary>
        /// <param name="searchToken">string used to compare with this object's fields and properties</param>
        /// <returns>Returns true if this object contains the search token, false otherwise</returns>
        public bool ContainsToken(string searchToken)
        {
            //remove white spaces
            string token = new string(searchToken.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());            

            if (Title.ToLower().Contains(token)) return true;

            string authorName = Author.Person.FirstName + Author.Person.LastName;
            if (authorName.ToLower().Contains(token.ToLower())) return true;

            if (ISBN.Contains(token)) return true;
            if (Subject.ToLower().Contains(token)) return true;

            return false;
        }

        #region IEnumerable
        //IEnumerable implementation

        /// <summary>
        /// Compares this Book object to the Book object in the argument.  First compares Title, then compares by ISBN.
        /// </summary>
        /// <param name="other">Book to be compared against</param>
        /// <returns>Returns a negative int if this CheckOutLog is less than other
        /// Returns 0 if this ChekOutLog is the same as other
        /// Returns neagtive int if this Book is greater than other</returns>
        public int CompareTo(Book other)
        {
            int titleCompare = this.Title.CompareTo(other.Title);
            if (titleCompare == 0)
            {
                return this.ISBN.CompareTo(other.ISBN);
            }
            else
            {
                return titleCompare;
            }
        }
        #endregion IEnumerable

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>Returns a string that describes this object</returns>
        public override string ToString()
        {
            string composite = "{0,-4} {1,-50} {2,15} {3,-15} {4,-15} {5,-5}";
            return string.Format(composite, BookId, Title, Author.Person.FirstName, Author.Person.LastName, Publisher, YearPublished);
        }
    }
}
