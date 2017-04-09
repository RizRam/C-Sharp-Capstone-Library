using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{

    /// <summary>
    /// An extension to the Entity framework code-generated CheckOutLog class of the LibraryDB.edmx
    /// </summary>
    public partial class CheckOutLog : IComparable<CheckOutLog>
    {
        //A Property that returns whether this CheckOutLog is overdue.
        //A CheckOutLog is overdue if the current date is past the overdue date, which is 30 days
        //after the CheckOutDate.
        public bool IsOverdue
        {
            get
            {
                DateTime dueDate = new DateTime(CheckOutDate.Year, CheckOutDate.Month, CheckOutDate.Day);
                dueDate.AddDays(30);
                if (DateTime.Now.Date > dueDate.Date)
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
        }

        /// <summary>
        /// Searches the CheckOutLog's Properties to see if this CheckOutLog contains the search token passed in the argument
        /// by comparing the search token passed in the argument with this object's Book title, Book ISBn and LibraryCardID
        /// </summary>
        /// <param name="searchToken">string used for comparison</param>
        /// <returns>Returns true if this CheckOutLog contains the search token passed in the argument, false otherwise</returns>
        public bool ContainsToken(string searchToken)
        {
            string token = searchToken.ToLower();

            if (Book.Title.ToLower().Contains(token)) return true;
            if (Book.ISBN.ToLower().Contains(token)) return true;
            if (Cardholder.LibraryCardId.ToLower().Contains(token)) return true;

            return false;
        }

        /// <summary>
        /// Override of this class's ToString methodusing the following format:
        /// bookId ISBN title checkOutDate CardholderId CardholderFirstName CardholderLastName CardholderLibraryCardId CardholderPhone
        /// </summary>
        /// <returns>Returns a string describing the contents of thi CheckOutLog</returns>
        public override string ToString()
        {            
            string composite = "{0,-4} {1,-20} {2,-15} {3,-15} {4, -4} {5,-15} {6,-15} {7, -10} {8, -11}";
            return string.Format(composite, Book_BookId, Book.ISBN, Book.Title, CheckOutDate.Date, Cardholder_PersonId, Cardholder.Person.FirstName,
                                 Cardholder.Person.LastName, Cardholder.LibraryCardId, Cardholder.Phone);
        }

        #region IComparable
        //IComparable implementation

        /// <summary>
        /// Compares this CheckOutLog to the CheckOutLog passed in the argument.  First compares by CheckOutDate, then by Book Title.
        /// </summary>
        /// <param name="other">CheckOutLog to be compared against</param>
        /// <returns>Returns a negative number is this object is less than other.
        /// Returns 0 if this object is the same as other
        /// Returns a positive integer if this object is greather than other</returns>
        public int CompareTo(CheckOutLog other)
        {
            int checkoutDateCompare = this.CheckOutDate.CompareTo(other.CheckOutDate);
            if (checkoutDateCompare == 0)
            {
                return this.Book.Title.CompareTo(other.Book.Title);
            }
            else
            {
                return checkoutDateCompare;
            }
        }

        #endregion IComparable
    }
}
