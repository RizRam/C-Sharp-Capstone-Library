using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{

    /// <summary>
    /// An abstract data type which represents a collection of Book objects.
    /// Uses an internal data structure of a generic SortedSet.This ensures that there are no duplicate books,
    /// and provides a search retrieval of O(log n).
    /// </summary>
    public class BookInventory : IEnumerable<Book>
    {
        private const int MINIMUM_CAPACITY = 1000;  //the minimum capacity of the SortedSet

        private SortedSet<Book> bookInventory;  //the internal data structure of BookInventory.  Uses .NET SortedSet

        public int Count { get { return bookInventory.Count; } }  //returns a count of elements in bookInventory

        /// <summary>
        /// Constructor for BookInventory
        /// Initializes internal SortedSet with BookComparer for sorting operations.
        /// </summary>
        public BookInventory()
        {
            bookInventory = new SortedSet<Book>(new BookComparer());
        }

        /// <summary>
        /// Constructor for BookInventory
        /// Initializes internal SortedSet, and populates the set with all elements in the collection passed by the argument
        /// </summary>
        /// <param name="books">Generic ICollection of Book objects to be added into the collection</param>
        public BookInventory(ICollection<Book> books) : this()
        {
            Add(books);
        }

        #region Add

        /// <summary>
        /// Adds Book object into collection.  If Book already exists in the collection, it will not be added or replace.
        /// </summary>
        /// <param name="book">Book to be added into collection</param>
        public void Add(Book book)
        {
            try
            {
                bookInventory.Add(book);
            }
            catch
            {
                return;
            }            
        }

        /// <summary>
        /// Adds all of the elements in the collection ppassed in the argument into this collection
        /// </summary>
        /// <param name="books">A generic ICollection of Book objects to be added into the collection</param>
        public void Add(ICollection<Book> books)
        {
            foreach (Book b in books)
            {
                Add(b);
            }
        }
        #endregion Add

        #region Remove

        /// <summary>
        /// Removes Book object from collection
        /// </summary>
        /// <param name="book">Book to be removed from collection</param>
        public void Remove(Book book)
        {
            bookInventory.Remove(book);
        }

        #endregion Remove

        #region Search

        ////Returns a generic list of all Book objects in bookInventory whose ISBN contains the string in the argument.
        //public List<Book> SearchByISBN(string isbn)
        //{
        //    var result = (from b in bookInventory
        //                  where b.ISBN.ToLower().Contains(isbn.ToLower())
        //                  select b).ToList();            

        //    return result;
        //}



        ////Returns a generic list of all Book objects in bookInventory whose title contains the the string in the argument.
        //public List<Book> SearchByTitle(string title)
        //{
        //    var result = (from b in bookInventory
        //                  where b.Title.ToLower().Contains(title.ToLower())
        //                  select b).ToList();

        //    return result;
        //}

        ////Returns a generic list of all Book objects in bookInventory whose author's first name or last name contains
        ////the string in the argument.
        //public List<Book> SearchByAuthor(string author)
        //{
        //    var result = (from b in bookInventory
        //                  where b.Author.Person.FirstName.ToLower().Contains(author.ToLower()) ||
        //                        b.Author.Person.LastName.ToLower().Contains(author.ToLower())
        //                  select b).ToList();

        //    return result;
        //}

        ////Returns a generic list of all Book objects in bookInventory whose subject contains the string in the argument.
        //public List<Book> SearchBySubject(string subject)
        //{
        //    var result = (from b in bookInventory
        //                  where b.Subject.ToLower().Contains(subject.ToLower())
        //                  select b).ToList();

        //    return result;
        //}

        /// <summary>
        /// Finds all Book objects in the collection that contain the search token
        /// </summary>
        /// <param name="searchToken">string used to find Book objects</param>
        /// <returns>Returns a BookInventory that contains all Book objects in this collection that match the search token</returns>
        public BookInventory FindAll(string searchToken)
        {
            string token = searchToken.ToLower();
            BookInventory result = new BookInventory();
            foreach (Book b in bookInventory)
            {
                if (b.ContainsToken(token))
                {
                    result.Add(b);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a Book object from this collection that has the same ISBN as the string passed in the argument
        /// </summary>
        /// <param name="isbn">ISBN string used to retrieve the specific book from the collection</param>
        /// <returns>Returns true if a Book has been found with the same ISBn.  Returns false otherwise.</returns>
        public Book SearchByISBNExact(string isbn)
        {
            var result = (from b in bookInventory
                          where b.ISBN.ToLower() == isbn.ToLower()
                          select b).SingleOrDefault();

            return result;
        }

        #endregion Search

        /// <summary>
        /// Checks whether any Book object's ISBN property is the same as the string passed in the argument.
        /// </summary>
        /// <param name="isbn">ISBN string used to find Book</param>
        /// <returns>returns true if Book has been found, false otherwise.</returns>
        public bool IsUniqueISBN(string isbn)
        {
            foreach (Book b in bookInventory)
            {
                if (b.ISBN.ToLower() == isbn.ToLower())
                    return false;
            }

            return true;
        }
        
        #region IEnumerable
        //IEnumerable implementation
        
        public IEnumerator<Book> GetEnumerator()
        {
            return bookInventory.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable

        #region IComparer
        /// <summary>
        /// A Comparer class for Book objects to be used in the SortedSet of BookInventory
        /// </summary>
        private class BookComparer : IComparer<Book>
        {
            /// <summary>
            /// Compares Book x to Book y using Book class's <c>CompareTo</c> method for implementation
            /// </summary>
            /// <param name="x">Book to be compared</param>
            /// <param name="y">Book to be compared</param>
            /// <returns>Returns a negative int if x is less than y
            /// Returns 0 if x equals y, and a positive int if x is greater than y</returns>
            public int Compare(Book x, Book y)
            {
                return x.CompareTo(y);
            }
        }

        #endregion IComparer

    }
}
