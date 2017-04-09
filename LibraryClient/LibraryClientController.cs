using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Xml;
using System.Data.SqlTypes;
using LocalLibraryDBLoader;



namespace LibraryClient
{
    /// <summary>
    /// Delegate associated with <event>OnDatabaseErrorRaised</event> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DatabaseErrorHandler(object sender, DatabaseEventArgs e);
    
    /// <summary>
    /// Controls the backend functions of the Library Client program.
    /// Provides an interface to a the Entity-Framework generated Database Context, as well as
    /// collections of database objects in memory.  
    /// </summary>
    public class LibraryClientController : IDisposable
    {
        #region Fields and Properties
        private const int MIN_YEAR_PUBLISHED = 1400;  //Minimum year that a book can be published

        private LibraryDB context;  //LibraryDB context created by Entity Framework
        private CheckOutLogList checkoutLogs;  //a collection of CheckOutLog objects stored in memory
        private PeopleList peopleList;  //a collection of MPerson objects stored in memoery
        private BookInventory bookInventory;  //a collection of Book objects stored in memory

        //A Dictionary that stores Librarian/MLibrarian UserID as Keys with the corresponding Password as the Value
        //Allows for O(log n) retrieval when user logs in
        private Dictionary<string, string> LibrarianUserIDs;

        //A Dictionary that stores Cardholder/MCardholder LibraryCardIDs as Keys with the corresponding MCardholder object as the Value.
        //Allows for O(log n) retrieval when checking out a book with a particular userID
        private Dictionary<string, MCardholder> CardholderLibraryCardIDs;

        public bool IsLibrarian { get; private set; }  //A flag that notifies whether the current user is a Librarian/MLibrarian or a regular patron.

        public event DatabaseErrorHandler OnDatabaseError;  //event to broadcast if a database error has occurred

        #endregion

        /// <summary>
        /// Constructor.  Opens a connection to the database using Entity Framework
        /// and loads all database entities into local collections and classes.
        /// </summary>
        public LibraryClientController()
        {
            context = new LibraryDB();           
            
            try
            {
                //If the context collections are null or contains no elements, load from XML using XMLSaverLoader
                //if (context.Books == null || context.Books.Count() <= 0 || context.People == null || context.People.Count() == 0)
                //{
                //    XMLSaverLoader xmlSaverLoader = new XMLSaverLoader();
                //    xmlSaverLoader.LoadFromLocalXML();
                //    Message = "Database Loaded";
                //}
                //else
                //{
                //    Message = "Database Already Loaded";
                //}

                //Add to collections
                LoadPeopleList();
                LoadToBookInventory();
                LoadCheckOutLogList();

                //Set isLibrarian
                IsLibrarian = false;                
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                //Message = ex.Message;
                DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                OnDatabaseErrorRaised(e);                
            }
        }

        #region Events

        /// <summary>
        /// Broadcasts OnDatabaseError events, takes a <c>DatabaseEventArgs</c> as argument.
        /// </summary>
        /// <param name="e"></param>
        private void OnDatabaseErrorRaised(DatabaseEventArgs e)
        {
            DatabaseErrorHandler handler = OnDatabaseError;
            if (handler != null)  //make sure that there are subscribers to the event
            {
                handler(this, e);
            }            
        }

        #endregion Events

        #region Load to collections

        /// <summary>
        /// Retrieves all Librarians, Cardholders, and Authors from database context to generate local memory objects.
        /// Populates local memory collections with the newly created objects.
        /// </summary>
        private void LoadPeopleList()
        {   
            //Initialize peopleList
            peopleList = new PeopleList();
            LibrarianUserIDs = new Dictionary<string, string>();
            CardholderLibraryCardIDs = new Dictionary<string, MCardholder>();

            //Librarians
            var dbLibrarians = (from l in context.Librarians
                                select l).ToList();

            foreach(Librarian l in dbLibrarians)
            {
                MLibrarian ml = new MLibrarian(l);
                peopleList.Add(ml);
                LibrarianUserIDs.Add(ml.UserID, ml.Password);
            }

            //Cardholders
            var dbCardholders = (from ch in context.Cardholders
                                 select ch).ToList();

            foreach(Cardholder ch in dbCardholders)
            {
                MCardholder mch = new MCardholder(ch);
                peopleList.Add(mch);
                CardholderLibraryCardIDs.Add(mch.LibraryCardId, mch);
            }

            //Authors
            var dbAuthors = (from a in context.Authors
                             select a).ToList();

            foreach(Author a in dbAuthors)
            {
                MAuthor ma = new MAuthor(a);
                peopleList.Add(ma);
            }
        }

        /// <summary>
        /// Initializes and populates bookInventory with Book objects from database context.
        /// </summary>
        private void LoadToBookInventory()
        {
            //Get a list of Book objects from database context
            var dbBooks = (from b in context.Books
                           select b).ToList();

            //create and assign bookInventory
            bookInventory = new BookInventory(dbBooks);
        }

        /// <summary>
        /// Initializes and populates checkoutLogs with CheckOutLog objects from datbase context.
        /// </summary>
        private void LoadCheckOutLogList()
        {
            //Get a list of CheckoutLog objects from database context
            var dbLogs = (from l in context.CheckOutLogs
                          select l).ToList();

            //Create and assign checkoutLogs
            checkoutLogs = new CheckOutLogList(dbLogs);
        }

        #endregion Load to collections

        #region Check Out/In

        /// <summary>
        /// Check Out Book, make changes to the database and local collections
        /// </summary>
        /// <param name="book">Book to be checked out</param>
        /// <param name="cardHolder">MCardholder that is checking out the book</param>
        /// <param name="error">an error string to be outputted should a check out error occur</param>
        /// <returns></returns>
        public bool CheckOut(Book book, MCardholder cardHolder, out string error)
        {
            if (!IsLibrarian)  //if not a librarian
            {
                error = "You are not authorized to check out a book.";
                return false;
            }

            if (cardHolder.HasOverdueBooks)
            {
                error = "User has Overdue books, cannot check out additional books.";
                return false;
            }                

            if (book.CopiesAvailable > 0)  //Make sure that there is a copy available
            {                
                //Create a checkout log
                CheckOutLog log = new CheckOutLog
                {
                    Book = book,
                    Book_BookId = book.BookId,
                    CheckOutDate = DateTime.Now,
                    Cardholder_PersonId = cardHolder.ID,
                    Cardholder = cardHolder.Cardholder                
                };

                
                try
                {
                    //Add log to context and save
                    context.CheckOutLogs.Add(log);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        ex = ex.InnerException;
                    }
                    DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                    OnDatabaseErrorRaised(e);

                    error = "Cannot check out book.  Please try again later.";
                    return false;
                }

                //add log to memory collection
                checkoutLogs.Add(log);

                error = "";

                return true;
            }

            error = string.Format("{0} could not be checked out, no copies available", book.Title);
            return false;
        }

        /// <summary>
        /// Checks in a book.  Makes appropriate changes to the database and local memory collections
        /// </summary>
        /// <param name="log">CheckOut log to be used for the check in operations</param>
        /// <returns></returns>
        public bool CheckIn(CheckOutLog log)
        {
            if (!IsLibrarian) //if not a librarian
                return false;
            
            try
            {
                //remove log from context and save changes
                context.CheckOutLogs.Remove(log);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                while(ex != null)
                {
                    ex = ex.InnerException;
                }
                DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                OnDatabaseErrorRaised(e);

                return false;
            }

            //remove log from memory collection
            checkoutLogs.Remove(log);
            return true;
            
        }

        #endregion Check Out/In

        #region Search

        /// <summary>
        /// Searches local collection to find all Book objects that contain the searchtoken
        /// </summary>
        /// <param name="searchToken">string used to find Books in collection</param>
        /// <returns>BookInventory that contains all Book objects that contain searchToken</returns>
        public BookInventory SearchBookInventory(string searchToken)
        {
            return bookInventory.FindAll(searchToken);
        }

        /// <summary>
        /// Finds a Book object that has the same exact ISBN as the string passed in the argument
        /// </summary>
        /// <param name="isbn">isbn string used to search Books</param>
        /// <returns>Book that matches the ISBN string exactly</returns>
        public Book SearchISBN(string isbn)
        {
            return bookInventory.SearchByISBNExact(isbn);
        }

        /// <summary>
        /// Finds all CheckOutLogs that contain the search token
        /// </summary>
        /// <param name="token">string used to find Book objets</param>
        /// <returns>A generic list of CheckOutLogs that contain the search token string</returns>
        public List<CheckOutLog> SearchLogs(string token)
        {
            return checkoutLogs.FindAll(token);
        }

        #endregion Search

        #region LogIn

        /// <summary>
        /// Logs in librarian if provided valid userID and password.  Sets IsLibrarian Property to true if successful
        /// </summary>
        /// <param name="userId">string of the librarian's userID</param>
        /// <param name="password">string of the associated password</param>
        /// <returns>returns true if userID exists and password matches the password value in LibrarianUserIDs
        /// returns false otherwise</returns>
        public bool LogIn(string userId, string password)
        {
            string storedPassword;
            if (LibrarianUserIDs.TryGetValue(userId, out storedPassword) && storedPassword == password)
            {
                IsLibrarian = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logs out the user.  Sets the IsLibrarian Property to false
        /// </summary>
        public void LogOut()
        {
            IsLibrarian = false;
        }

        #endregion LogIn

        #region Save

        /// <summary>
        /// Saves changes to database context.
        /// Raises <event>OnDatabaseError</event> when exception occurs
        /// </summary>
        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    ex = ex.InnerException;
                }
                DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                OnDatabaseErrorRaised(e);
            }
        }

        #endregion Save

        #region Add / Remove

        /// <summary>
        /// Adds a new Book to database context and to local memory collection.
        /// Raises <event>OnDatabaseError</event> when exception is thrown from changing the database context.
        /// </summary>
        /// <param name="book">Book object to be added</param>
        /// <returns>Returns true if operation is successful, false otherwise</returns>
        public bool AddBook(Book book)
        {
            if (!IsLibrarian) return false;  //if not a librarian return false

            try
            {
                //add book to database context
                context.Books.Add(book);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                //send OnDatabaseError event
                while (ex != null)
                {
                    ex = ex.InnerException;
                }
                DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                OnDatabaseErrorRaised(e);
                return false;
            }

            bookInventory.Add(book);

            return true;
        }

        /// <summary>
        /// Removes Book object in the argument from database context and local memory collection.
        /// Raises <event>OnDatabaseError</event> when exception is caught from attempting to change the database context.
        /// </summary>
        /// <param name="b">Book object to be removed</param>
        /// <returns>returns true if operation is successful, false otherwise.</returns>
        public bool RemoveBook(Book b)
        {
            if (!IsLibrarian) return false;  //return false if IsLibrarian is false.

            if (b.IsCheckedOut)  //check if books are checked out or not
            {
                OnDatabaseErrorRaised(new DatabaseEventArgs("Cannot remove book, all copies must be checked in"));
                return false;
            }
            else  //books are all checked in
            {
                try
                {
                    //remove from database
                    context.Books.Remove(b);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        ex = ex.InnerException;
                    }
                    DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                    OnDatabaseErrorRaised(e);
                    return false;
                }

                //remove from memory
                bookInventory.Remove(b);

                return true;
            }
        }

        //Returns the newly created Author object when opration is successful, returns null otherwise.
        /// <summary>
        /// Creates a new Author and MAuthor object with the properties set to the arguments passed.
        /// Author and MAuthor are added to the database context and local collection respectively.
        /// Raises <event>OnDatabaseError</event> when exception is caught from attempting to change database context.
        /// </summary>
        /// <param name="firstName">First name of author</param>
        /// <param name="lastName">Last name of author</param>
        /// <param name="bio">Bio of author</param>
        /// <returns>Returns true if operation is successful, false otherwise</returns>
        public Author CreateAuthor(string firstName, string lastName, string bio)
        {
            try
            {
                //create person
                Person addPerson = new Person();
                addPerson.FirstName = firstName;
                addPerson.LastName = lastName;

                //add to database to get personID
                context.People.Add(addPerson);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                //Rase OnDatabaseError event
                while (ex != null)
                {
                    ex = ex.InnerException;
                }
                DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                OnDatabaseErrorRaised(e);
                return null;
            }

            try
            { 
               //retrieve Person from database with assigned ID
               Author result = new Author();
               Person addPerson = (from p in context.People
                             where p.FirstName == firstName && p.LastName == lastName
                             select p).First();

                result.Person = addPerson;
                result.PersonId = addPerson.PersonId;
                result.Bio = bio;

                //Add author to people list
                MAuthor ma = new MAuthor(result);
                peopleList.Add(ma);

                //Add author to database
                context.Authors.Add(result);
                context.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                //Raise OnDatabaseError event
                while (ex != null)
                {
                    ex = ex.InnerException;
                }
                DatabaseEventArgs e = new DatabaseEventArgs(ex.Message);
                OnDatabaseErrorRaised(e);
                return null;
            }

        }

        #endregion Add / Remove

        #region Accessors

        /// <summary>
        /// Retrieves a list of all MLibrarian objects in collection
        /// </summary>
        /// <returns>A generic list of MLibrarian objects</returns>
        public List<MLibrarian> GetLibrarianList()
        {
            return peopleList.Librarians;
        }

        /// <summary>
        /// Retrieves a list of all MCardholder objects in collection
        /// </summary>
        /// <returns>A generic list of MCardholder objects</returns>
        public List<MCardholder> GetCardholdersList()
        {
             return peopleList.Cardholders;
        }

        /// <summary>
        /// Retrieves a list of all MAuthor objects in collection
        /// </summary>
        /// <returns>A generic list of MAuthor objects</returns>
        public List<MAuthor> GetAuthorsList()
        {
            return peopleList.Authors;
        }

        //Returns a generic list of 
        /// <summary>
        /// Retrives a list of KeyvaluePair &ltCheckOutLog, MCardholder&gt Where the key value is an overdue CheckOutLog and the value is
        /// the MCardholder that is associated with that CheckOutLog.
        /// </summary>
        /// <returns>A generic list of KeyValuePair &ltCheckOutLog, MCardholder&gt</returns>
        public List<KeyValuePair<CheckOutLog, MCardholder>> GetOverdueList()
        {
            //initialize list
            List<KeyValuePair<CheckOutLog, MCardholder>> overdueList = new List<KeyValuePair<CheckOutLog, MCardholder>>();

            //get overdue logs
            List<CheckOutLog> overdueLogs = checkoutLogs.OverdueLogs;

            //find the corresponding MCardholder from peopleList
            foreach(CheckOutLog key in overdueLogs)
            {
                MCardholder value = (from p in peopleList.Cardholders
                                     where p.ID == key.Cardholder_PersonId
                                     select p).SingleOrDefault();

                //create KeyValuePair and add to list
                KeyValuePair<CheckOutLog, MCardholder> kvp = new KeyValuePair<CheckOutLog, MCardholder>(key, value);
                overdueList.Add(kvp);
            }

            return overdueList;
        }

        /// <summary>
        /// Retrives a MCardholder object that is associated with the libraryCardID passed in the argument
        /// </summary>
        /// <param name="libraryCardID">libraryCardID of MCardholder</param>
        /// <returns>Returns MCardholder found in collection, returns null otherwise.</returns>
        public MCardholder GetCardHolder(string libraryCardID)
        {
            MCardholder result = null;
            try
            {
                result = CardholderLibraryCardIDs[libraryCardID];
            }
            catch (KeyNotFoundException)
            {
                result = null;
            }

            return result;            
        }

        #endregion Accessors

        #region Dispose
        
        /// <summary>
        /// Disposes all outside resources
        /// </summary>
        public void Dispose()
        {            
            try
            {
                //Save to XML
                //XMLSaverLoader saver = new XMLSaverLoader();
                //saver.SaveToXML();
                //Message = "Saved";
            }
            catch (Exception ex)
            {
                //Raise an OnDatabaseError event when exception is caught.
                while (ex.InnerException!= null)
                {
                    ex = ex.InnerException;
                }
                //Message = ex.Message;
                OnDatabaseErrorRaised(new DatabaseEventArgs(ex.Message));
            }
            finally
            {
                //ensure that database context is disposed.
                context.Dispose();
            }
        }

        #region IDisposable
        //IDisposable implementation

        void IDisposable.Dispose()
        {
            Dispose();
        }

        //Destructor, calls Dispose to ensure all outside resources are closed.
        /// <summary>
        /// Destructor.
        /// Calls <c>Dispose</c> to ensure all outside resources are closed.
        /// </summary>
        ~LibraryClientController()
        {
            Dispose();
        }

        #endregion IDisposable
        #endregion Dispose
    }
}
