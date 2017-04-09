using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    /// <summary>
    /// An abstract class meant to represent the Entity-Framework code-generated class Person locally in memory.
    /// </summary>
    public abstract class MPerson : IComparable<MPerson>
    {
        public int ID { get; set; }  //ID of MPerson
        public string FirstName { get; set; }  //First Name of MPerson
        public string LastName { get; set; }  //Last Name of MPerson
        
        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>returns a string that describes this object</returns>
        public override string ToString()
        {
            string composite = "{0,-4} {1} {2,-15}";
            return string.Format(composite, ID, FirstName, LastName);
        }

        #region IComparable
        //IComprable implementaation

        /// <summary>
        /// Compares this MPerson object to the MPerson in the argument.  Does comparison by LastName then by FirstName.
        /// </summary>
        /// <param name="other">MPerson to be compared against</param>
        /// <returns>Returns negative integer if this object is less than other
        /// Returns 0 if this object is the same as other
        /// Returns positive int if this object is greater than other</returns>
        public int CompareTo(MPerson other)
        {
            int lastNameCompare = this.LastName.CompareTo(other.LastName);
            if (lastNameCompare == 0)
            {
                return this.FirstName.CompareTo(other.FirstName);
            }
            else
            {
                return lastNameCompare;
            }
        }

        #endregion IComparable
        //A Comparer class for MPerson object for use in PeopleList
        public class PeopleComparer : IComparer<MPerson>
        {
            //Compares MPerson x to MPerson y.  Returns a negative int if x is less than y,
            //0 if x is the same as y and a positive int if x is greater than y.
            //Uses MPerson's CompareTo method to return the value.
            /// <summary>
            /// Compares MPerson x to MPerson y. Uses MPerson's CompareTo method to return the value
            /// </summary>
            /// <param name="x">MPerson used for comparison</param>
            /// <param name="y">MPerson used for comparison</param>
            /// <returns>Returns negative integer if x is less than y
            /// Returns 0 if x is the same as y
            /// Returns positive int if x is greater than y</returns>
            public int Compare(MPerson x, MPerson y)
            {
                return x.CompareTo(y);
            }
        }
    }
}
