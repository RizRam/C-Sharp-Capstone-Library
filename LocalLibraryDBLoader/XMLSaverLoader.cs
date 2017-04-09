using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.Entity;
using System.Data.SqlTypes;

namespace LocalLibraryDBLoader
{
    public class XMLSaverLoader : IDisposable
    {
        private LibraryDataBaseContext context;       

        public XMLSaverLoader()
        {
            context = new LibraryDataBaseContext();
        }

        #region Load
        public bool LoadFromLocalXML()
        {
            try
            {
              

                LoadPeopleFromXML();
                LoadAuthorsFromXML();  
                LoadCardHoldersFromXML();
                LoadLibrariansFromXML();

                LoadBooksFromXML();
                LoadCheckOutLogFromXML();

                return true;
            }
            catch (Exception ex)
            {
                Exception top = new Exception("Something happened", ex);
                throw top;
            }
        }

        #region People

        private void LoadPeopleFromXML()
        {
            try
            {
                XDocument peopleDocument = XDocument.Load("XML Files/People.xml");

                var people = (from p in peopleDocument.Descendants("Person")
                            select p).ToList();

                foreach(var person in people)
                {
                    int id; int.TryParse(person.Element("PersonID").Value, out id);
                    Person newPerson = new Person
                    {
                        PersonId = id,
                        FirstName = person.Element("FirstName").Value,
                        LastName = person.Element("LastName").Value                    
                    };

                    context.People.Add(newPerson);
                }

                context.SaveChanges();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Loads Authors table from local xml file
        private void LoadAuthorsFromXML()
        {
            try
            {
                //XDocument peopleDocument = XDocument.Load("XML Files/People.xml");
                XDocument authorsDocument = XDocument.Load("XML Files/Authors.xml");


                //var people = (from p in peopleDocument.Descendants("Person")
                //              select p).ToList();

                var authors = (from a in authorsDocument.Descendants("Author")
                               select a).ToList();

                foreach (var author in authors)
                {
                    int id; int.TryParse(author.Element("ID").Value, out id);

                    //string firstName, lastName;
                    //LoadFirstLastNameFromXML(people, id.ToString(), out firstName, out lastName);

                    Author newAuthor = new Author
                    {
                        PersonId = id,
                        //FirstName = firstName,
                        //LastName = lastName,
                        Bio = author.Element("Bio").Value,
                        Person = (from p in context.People
                                  where p.PersonId == id
                                  select p).First()                      
                        
                    };
                    
                    context.Authors.Add(newAuthor);
                    //context.People.Add(newAuthor);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Author " + ex.Message);
                Exception authorException = new Exception("Author", ex);
                throw authorException;
            }
        }

        //Loads the CardHolders Table from local XML file
        private void LoadCardHoldersFromXML()
        {
            try
            {
                //XDocument peopleDocument = XDocument.Load("XML Files/People.xml");
                XDocument cardholdersDocument = XDocument.Load("XML Files/Cardholders.xml");

               // var people = (from p in peopleDocument.Descendants("Person")
               //               select p).ToList();

                var cardholders = (from ch in cardholdersDocument.Descendants("Cardholder")
                                   select ch).ToList();

                foreach (var cardholder in cardholders)
                {
                    //string firstName, lastName;
                    //LoadFirstLastNameFromXML(people, cardholder.Element("ID").Value, out firstName, out lastName);

                    int id; int.TryParse(cardholder.Element("ID").Value, out id);

                    Cardholder newCardHolder = new Cardholder
                    {
                        PersonId = id,
                        //FirstName = firstName,
                        //LastName = lastName,
                        Phone = cardholder.Element("Phone").Value,
                        LibraryCardId = cardholder.Element("LibraryCardID").Value,
                        Person = (from p in context.People
                                  where p.PersonId == id
                                  select p ).First()
                    };

                    context.Cardholders.Add(newCardHolder);
                    //context.People.Add(newCardHolder);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exception chEx = new Exception("CardHolder", ex);
                throw chEx;
            }
        }

        //Loads the Librarians Table from local XML file
        private void LoadLibrariansFromXML()
        {
            try
            {
                XDocument librariansDocument = XDocument.Load("XML Files/Librarians.xml");

                //var people = (from p in peopleDocument.Descendants("Person")
                //              select p).ToList();

                var librarians = (from l in librariansDocument.Descendants("Librarian")
                                  select l).ToList();

                foreach (var librarian in librarians)
                {
                    int id; int.TryParse(librarian.Element("ID").Value, out id);

                    //string firstName, lastName;
                    //LoadFirstLastNameFromXML(people, librarian.Element("ID").Value, out firstName, out lastName);

                    Librarian newLibrarian = new Librarian
                    {
                        PersonId = id,
                        //FirstName = firstName,
                        //LastName = lastName,
                        Phone = librarian.Element("Phone").Value,
                        UserId = librarian.Element("UserID").Value,
                        Password = librarian.Element("Password").Value,
                        Person = (from p in context.People
                                  where p.PersonId == id
                                  select p).First()
                        
                    };

                    context.Librarians.Add(newLibrarian);
                    //context.People.Add(newLibrarian);
                }

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exception librarianException = new Exception("Librarian", ex);
                throw librarianException;
            }
        }

        //Assigns the first name and last name of a Person entity given the PersonID
        private void LoadFirstLastNameFromXML(ICollection<XElement> people, string ID, out string firstName, out string lastName)
        {
            firstName = (from p in people
                         where p.Element("PersonID").Value == ID
                         select p).First().Element("FirstName").Value;

            lastName = (from p in people
                        where p.Element("PersonID").Value == ID
                        select p).First().Element("LastName").Value;
        }

        #endregion People

        #region Books

        //Fill Books table with data from local XML
        private void LoadBooksFromXML()
        {
            try
            {
                XDocument bookDocument = XDocument.Load("XML Files/Books.xml");

                var books = (from b in bookDocument.Descendants("Book")
                             select b).ToList();

                foreach (var book in books)
                {

                    int id; int.TryParse(book.Element("BookID").Value, out id);
                    int pages; int.TryParse(book.Element("NumPages").Value, out pages);
                    int yearPublished; int.TryParse(book.Element("YearPublished").Value, out yearPublished);
                    int copies; int.TryParse(book.Element("NumberOfCopies").Value, out copies);
                    string authorID = book.Element("AuthorID").Value;
                    Author author = (from a in context.Authors
                                     where a.PersonId.ToString() == authorID
                                     select a).First();

                    Book newBook = new Book
                    {
                        BookId = id,                        
                        ISBN = book.Element("ISBN").Value,
                        Title = book.Element("Title").Value,
                        NumPages = pages,
                        Subject = book.Element("Subject").Value,
                        Description = book.Element("Description").Value,
                        Publisher = book.Element("Publisher").Value,
                        YearPublished = yearPublished,
                        Language = book.Element("Language").Value,
                        NumberOfCopies = copies,

                        Author = author
                    };

                    context.Books.Add(newBook);

                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exception bookException = new Exception("Book", ex);
                throw bookException;
            }

        }

        //Fills the CheckOutLogs table with data from local xml
        private void LoadCheckOutLogFromXML()
        {
            try
            {
                XDocument checkoutDocument = XDocument.Load("XML Files/CheckOutLog.xml");

                var checkOutLogs = (from col in checkoutDocument.Descendants("CheckOutLog")
                                    select col).ToList();

                foreach (var checkOutLog in checkOutLogs)
                {

                    int id; int.TryParse(checkOutLog.Element("CheckOutLogID").Value, out id);

                    string cardholderID = checkOutLog.Element("CardholderID").Value;
                    Cardholder cardHolder = (from ch in context.Cardholders
                                             where ch.PersonId.ToString() == cardholderID
                                             select ch).First();

                    string bookID = checkOutLog.Element("BookID").Value;
                    Book book = (from b in context.Books
                                 where b.BookId.ToString() == bookID
                                 select b).First();

                    DateTime checkOutDate; DateTime.TryParse(checkOutLog.Element("CheckOutDate").Value, out checkOutDate);
                    if (checkOutDate.CompareTo(DateTime.MinValue) == 0)
                    {
                        checkOutDate = SqlDateTime.MinValue.Value;
                    }

                    DateTime checkInDate; DateTime.TryParse(checkOutLog.Element("CheckInDate").Value, out checkInDate);
                    if (checkInDate.CompareTo(DateTime.MinValue) == 0)
                    {
                        checkInDate = SqlDateTime.MinValue.Value;
                    }

                    //Only create and add checkout log if the book is checked out.
                    if (checkInDate < checkOutDate)
                    {
                        CheckOutLog newCheckOutLog = new CheckOutLog
                        {
                            CheckOutLogId = id,
                            Cardholder = cardHolder,
                            Book = book,
                            CheckOutDate = checkOutDate
                            //CheckInDate = checkInDate
                        };

                        context.CheckOutLogs.Add(newCheckOutLog);
                    }
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exception checkoutException = new Exception("CheckOut", ex);
                throw checkoutException;
            }
        }

        #endregion Books

        #endregion Load

        #region Save

        public void SaveToXML()
        {
            try
            {
                SaveCheckOutLogToXMl();
                SaveBooksToXML();
                SaveCardholdersToXML();
                SaveAuthorsToXML();
                SaveLibrariansToXML();
                SavePeopleToXML();
            }
            catch (Exception ex)
            {
                throw new Exception("Save Failed", ex);
            }
            
        }

        #region Books

        private void SaveCheckOutLogToXMl()
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Local CheckOutLog Table XML Save"),
                    new XElement("CheckOutLogs",
                        from log in context.CheckOutLogs
                        select new XElement("CheckOutLog", new XAttribute("CheckOutLogID", log.CheckOutLogId),
                               new XElement("CardholderID", log.Cardholder.PersonId),
                               new XElement("BookID", log.Book.BookId),
                               new XElement("CheckOutDate", log.CheckOutDate)
                               //new XElement("CheckInDate", log.CheckInDate)
                        )
                    )
                );

            document.Save("XML Files/CheckOutLog.xml");
        }

        private void  SaveBooksToXML()
        {
            XDocument booksDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Local Books Table XML Save"),
                new XElement("Books",
                    from book in context.Books  // Result is an IEnumerable<XElement> collection 
                    select new XElement("Book", new XAttribute("BookID", book.BookId),
                         new XElement("ISBN", book.ISBN),
                         new XElement("Title", book.Title),
                         new XElement("AuthorID", book.Author.PersonId),
                         new XElement("NumPages", book.NumPages),
                         new XElement("Subject", book.Subject),
                         new XElement("Description", book.Description),
                         new XElement("Publisher", book.Publisher),
                         new XElement("YearPublished", book.YearPublished),
                         new XElement("Language", book.Language),
                         new XElement("NumberOfCopies", book.NumberOfCopies))
                )
            );

            booksDocument.Save("XML Files/Books.xml");
                       
        }

        #endregion Books

        #region People

        private void SaveCardholdersToXML()
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Local Cardholders Table XML Save"),
                new XElement("Cardholders",
                    from ch in context.Cardholders
                    select new XElement("Cardholder", new XAttribute("ID", ch.PersonId),
                           new XElement("Phone", ch.Phone),
                           new XElement("LibraryCardID", ch.LibraryCardId))
                           )
            );

            document.Save("XML Files/Cardholders.xml");
        }

        private void SaveAuthorsToXML()
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Local Authors Table XML Save"),
                new XElement("Authors",
                    from author in context.Authors
                    select new XElement("Author", new XAttribute("ID", author.PersonId),
                           new XElement("Bio", author.Bio)
                    )
                )
            );

            document.Save("XML Files/Authors.xml");
        }

        private void SaveLibrariansToXML()
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Local Librarians Table XML Save"),
                new XElement("Librarians",
                    from librarian in context.Librarians
                    select new XElement("Librarian", new XAttribute("ID", librarian.PersonId),
                           new XElement("Phone", librarian.Phone),
                           new XElement("UserID", librarian.UserId),
                           new XElement("Password", librarian.Password)
                    )
                )
            );

            document.Save("XML Files/Librarians.xml");
        }

        private void SavePeopleToXML()
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Local People Table XML Save"),
                new XElement("People",
                    from person in context.People
                    select new XElement("Person", new XAttribute("PersonID", person.PersonId),
                           new XElement("FirstName", person.FirstName),
                           new XElement("LastName", person.LastName)
                    )
                )
            );

            document.Save("XML Files/People.xml");
        }



        #endregion People

        #endregion Save

        #region Dispose

        public void Dispose()
        {
            context.Dispose();
        }

        ~XMLSaverLoader()
        {
            Dispose();
        }

        #endregion Dispose
    }
}
