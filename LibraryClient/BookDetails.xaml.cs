using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryClient
{
    /// <summary>
    /// Interaction logic for BookDetails.xaml
    /// Window used to show and edit Book details.
    /// </summary>
    public partial class BookDetails : Window
    {
        private const int MIN_YEAR_PUBLISHED = 1400;  //Minimum Year possible to publish a book

        private LibraryClientController controller;  //Holds a reference to the controller of the Library Client
        private SearchResults searchResultsWindow;  //Holds a reference to a SearchResultsWindow that opened this BookDetails window.
        private Author originalAuthor;  //A reference to the original Author of the book, Book will revert to this Author should the user choose to discard changes
        private MAuthor changedAuthor;  //An MAuthor object that is used to change the Author property of Book
        private bool isNewBook;  //A flag indicating whther the BookDetails is displaying the contents of a new Book for editing.
        private bool edited;  //A flag indicating whether the Book has been edited

        public Book Book { get; private set; }  //The Book that the window is currently displaying the contents of
        
        /// <summary>
        /// Constructor for BookDetailsWindow
        /// Initializes controls, fields and Properties depending on the contents of the Book
        /// and whether the user is a librarian or patron
        /// </summary>
        /// <param name="book">Book which details will be displayed</param>
        /// <param name="controller">Controller for the Library Client</param>
        /// <param name="isNewBook">If true, notifies the window that the Book object to be displayed
        /// is a new Book, or already exists in the database.</param>
        public BookDetails(Book book, LibraryClientController controller, bool isNewBook)
        {
            InitializeComponent();

            //Initialize fields and Properties
            this.controller = controller;
            Book = book;       
            originalAuthor = Book.Author;
            changedAuthor = null;
            edited = false;
            searchResultsWindow = null;
            this.isNewBook = isNewBook;

            //Subscribe to OnDatabaseError vent
            this.controller.OnDatabaseError += OnDatabaseErrorHandler;

            //Initialize Controls
            InitializeButtons();

            ErrorLabel.Content = string.Empty;
            SetUserType();

            PopulateWindow();

            //If the user is a librarian and Book is new, automatically enable edit buttons
            if (controller.IsLibrarian && isNewBook)
                EnableEditing();
        }

        /// <summary>
        /// Constructor for BookDetails
        /// Initializes the Window controls to display the contents of a Book after selecting the Book
        /// from a SearchResults window
        /// </summary>
        /// <param name="book">Book which details will be displayed</param>
        /// <param name="controller">Controller for the library client</param>
        /// <param name="searchResults">SearchResults window that created theis BookDetails Window</param>
        public BookDetails(Book book, LibraryClientController controller, SearchResults searchResults) : this(book, controller, false)
        {
            //store reference to SearchResults Window
            searchResultsWindow = searchResults;
        }

        /// <summary>
        /// Sets the visibility of Button controls for when the form loads depending on the user type
        /// </summary>
        private void InitializeButtons()
        {
            if (controller.IsLibrarian)
            {
                EditBookButton.Visibility = Visibility.Visible;
                AddCopyButton.Visibility = Visibility.Visible;
                RemoveCopyButton.Visibility = Visibility.Visible;

                SaveExitButton.Visibility = Visibility.Hidden;
                EditAuthorButton.Visibility = Visibility.Hidden;
            }
            else
            {
                EditBookButton.Visibility = Visibility.Hidden;
                AddCopyButton.Visibility = Visibility.Hidden;
                RemoveCopyButton.Visibility = Visibility.Hidden;
                SaveExitButton.Visibility = Visibility.Hidden;
                EditAuthorButton.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Sets the UserTypeLabel content to the current user's type
        /// </summary>
        private void SetUserType()
        {
            if (controller.IsLibrarian)
            {
                UserTypeLabel.Content = "Librarian";
            }
            else
            {
                UserTypeLabel.Content = "Patron";
            }
        }

        /// <summary>
        /// Loads the contents of the Book's Properties into the Window's TextBoxes.
        /// </summary>
        private void PopulateWindow()
        {
            TitleTextBox.Text = Book.Title;

            //Author
            if (Book.Author != null)  //if the author exists
            {
                if (Book.Author != originalAuthor)  //revert the author to original author (used when edits are canceled)
                {
                    RevertAuthor();
                }

                AuthorTextBox.Text = String.Format("{0} {1}", originalAuthor.Person.FirstName, originalAuthor.Person.LastName);  //display author
            }
            else
            {
                AuthorTextBox.Text = string.Empty;
            }            

            PagesTextBox.Text = Book.NumPages.ToString();
            SubjectTextBox.Text = Book.Subject;
            DescriptionTextBox.Text = Book.Description;
            PublisherTextBox.Text = Book.Publisher;
            YearTextBox.Text = Book.YearPublished.ToString();
            LanguagesTextBox.Text = Book.Language;
            CopiesAvailableTextBox.Text = Book.CopiesAvailable.ToString();
            TotalCopiesTextBox.Text = Book.NumberOfCopies.ToString();
        }

        /// <summary>
        /// Event handler for EditBookButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            EnableEditing();
        }

        /// <summary>
        /// Enables and Shows Buttons that allow Book editing
        /// </summary>
        private void EnableEditing()
        {
            //Buttons

            SaveExitButton.Visibility = Visibility.Visible;
            EditAuthorButton.Visibility = Visibility.Visible;

            //Textboxes
            TitleTextBox.IsEnabled = true;
            PagesTextBox.IsEnabled = true;
            SubjectTextBox.IsEnabled = true;
            DescriptionTextBox.IsEnabled = true;
            PublisherTextBox.IsEnabled = true;
            YearTextBox.IsEnabled = true;
            LanguagesTextBox.IsEnabled = true;            
        }

        /// <summary>
        /// Event Handler for SaveExitButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveExitButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmChangesAndExit();
        }
        
        /// <summary>
        /// Shows a confirmation to the user to confirm edits.  
        /// Saves the edits and closes the window if the user selects yes,
        /// and reverts the changes if the user selects No.
        /// </summary>
        private void ConfirmChangesAndExit()
        {
            if (MessageBox.Show("Are you sure you want to make theses changes?", "Confirm Changes",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                //Canceled edit, repopulate the window with the original content
                PopulateWindow();
            }
            else //confirm changes
            {
                if (SaveTextEdits() && CheckAuthor())  //If user input is valid
                {
                    edited = true;

                    if (changedAuthor != null)  //assign author to changedauthor if it has been changed
                        AssignAuthor(changedAuthor);

                    DialogResult = true;

                    this.Close();
                }
            }
        }

        /// <summary>
        /// Reverts the Author of the Book to the original Author prior to changes.
        /// </summary>
        private void RevertAuthor()
        {
            //Revert Book.Author
            if (originalAuthor != null)
            {
                //Revert Author
                Book.Author = originalAuthor;
                Book.Author_PersonId = originalAuthor.PersonId;

                //Revert MAuthor in local collection
                List<MAuthor> authorList = controller.GetAuthorsList();
                foreach (MAuthor ma in authorList)
                {
                    if (ma.ID == originalAuthor.PersonId)  //if MAuthor is original Author, add the Book to the MAuthor
                    {
                        ma.Books.Add(Book);
                    }
                    else  //Remove Book from MAuthor hwo is not original author
                    {
                        ma.Books.Remove(Book);
                    }                   
                }
            }
        }

        /// <summary>
        /// Performs checks on user input and displays appropriate error message if input
        /// is not valid.  If all inputs are valid, saves the edits in the database as well.
        /// </summary>
        /// <returns></returns>
        private bool SaveTextEdits()
        {
            //numpages
            int pages;
            if (int.TryParse(PagesTextBox.Text, out pages))  //make sure that input is an integer.
            {
                if (pages < 0)  //make sure that the integer is positive
                {
                    ErrorLabel.Content = "Please enter a positive number for the number of pages.";
                    PagesTextBox.Focus();
                    return false;
                }
            }
            else
            {
                ErrorLabel.Content = "Please enter an integer for the number of pages.";
                PagesTextBox.Focus();
                return false;
            }

            //year
            int year;
            if (int.TryParse(YearTextBox.Text, out year))  //make sure that input is an integer
            {
                if (year < MIN_YEAR_PUBLISHED)  //Ensure that the year published is not below minimum year
                {
                    ErrorLabel.Content = String.Format("The minimum year possible is {0}", MIN_YEAR_PUBLISHED);
                    YearTextBox.Focus();
                    return false;
                }
                else if (year > DateTime.Now.Year)  //Make sure that the year published is not at a future time.
                {
                    ErrorLabel.Content = "Year published cannot be in a future year.";
                    YearTextBox.Focus();
                    return false;
                }
            }
            else
            {
                ErrorLabel.Content = "Please enter a positive integer for the year published.";
                return false;
            }

            //Assign content to Book
            Book.Title = TitleTextBox.Text;
            Book.NumPages = pages;
            Book.Subject = SubjectTextBox.Text;
            Book.Description = DescriptionTextBox.Text;
            Book.Publisher = PublisherTextBox.Text;
            Book.YearPublished = year;
            Book.Language = LanguagesTextBox.Text;

            //Save Changes
            controller.Save();

            return true;
        }

        /// <summary>
        /// Checks whether author is correctly assigned (not null)
        /// </summary>
        /// <returns></returns>
        private bool CheckAuthor()
        {
            if (changedAuthor != null)  //if the changed author has been edited and correctly assigned
            {
                return true;
            }
            else
            {
                if (originalAuthor == null)  //if the original author is also null return false.
                {
                    ErrorLabel.Content = "Book must have an author.";
                    return false;
                }

                return true;  //returns true because original author is not null so there is currently a valid author assigned.
            }
        }

        /// <summary>
        /// Event handler for AddCopyButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCopyButton_Click(object sender, RoutedEventArgs e)
        {
            AddCopies();
        }

        /// <summary>
        /// Opens a BookCopiesDialog dialog window and adds the result of the dialog into the total copies of the Book
        /// </summary>
        private void AddCopies()
        {
            //show dialog
            BookCopiesDialog bcd = new BookCopiesDialog(this, BookCopiesDialog.AddOrRemove.Add);
            if ((bool)bcd.ShowDialog())  //if the dialog returns a true (changes are confirmed)
            {
                Book.NumberOfCopies += bcd.CopiesResult;  //add the copies
                controller.Save();  //save changes to the database

                //Update the controls to reflect changes
                CopiesAvailableTextBox.Text = Book.CopiesAvailable.ToString();
                TotalCopiesTextBox.Text = Book.NumberOfCopies.ToString();

                //set edited flag to true.
                edited = true;
            }
        }

        /// <summary>
        /// Event handler for RemoveCopyButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveCopyButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveCopies();
        }

        /// <summary>
        /// Opens a BookCopiesDialog dialog window and removes the result of the dialog into the total copies of the book
        /// </summary>
        private void RemoveCopies()
        {
            BookCopiesDialog bcd = new BookCopiesDialog(this, BookCopiesDialog.AddOrRemove.Remove);
            if ((bool)bcd.ShowDialog())  //if dialog result returns true (changes confirmed)
            {
                Book.NumberOfCopies -= bcd.CopiesResult;  //subtract the total copies
                controller.Save();  //save changes to the database

                //Update controls to reflect change
                CopiesAvailableTextBox.Text = Book.CopiesAvailable.ToString();
                TotalCopiesTextBox.Text = Book.NumberOfCopies.ToString();

                //set edited to true
                edited = true;
            }
        }

        /// <summary>
        /// Event handler for EditAuthorButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            //Opens a new EditAuthorDialog dialog window.
            EditAuthorDialog ead = new EditAuthorDialog(this, controller);
            if ((bool)ead.ShowDialog())  //if dialog returns true (changes confirmed)
            {
                changedAuthor = ead.Result;  //set changed author to the result of dialog window

                //reflect change in control
                AuthorTextBox.Clear();
                AuthorTextBox.Text = String.Format("{0} {1}", changedAuthor.FirstName, changedAuthor.LastName);
            }
        }

        /// <summary>
        /// Assigns the Book object with Author associated with the MAuthor passed in the argument.
        /// </summary>
        /// <param name="assignAuthor">MAuthor that corresponds to the Entity Author to be assigned to Book</param>
        private void AssignAuthor(MAuthor assignAuthor)
        {
            //change Author Property
            Book.Author = assignAuthor.dbAuthor;
            Book.Author_PersonId = assignAuthor.dbAuthor.PersonId;

            //Save Changes to database
            controller.Save();

            //remove book from original MAuthor
            foreach (MAuthor a in controller.GetAuthorsList())
            {
                if (a.Books.Remove(Book)) break;
            }

            //Add book to new MAuthor
            assignAuthor.Books.Add(Book);
        }

        /// <summary>
        /// Event handler for Window Closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (edited)  //if edits were made
            {
                if (searchResultsWindow != null)  //if book details was opened by a SearchResults window
                {
                    searchResultsWindow.LoadSearchList();  //reload the search query
                    searchResultsWindow.ViewDetailsButton.IsEnabled = false;  //disable the view details button.
                }
            }
            else
            {
                DialogResult = false;
            }
        }

        /// <summary>
        /// Event handler for OnDatabaseError event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDatabaseErrorHandler(object sender, DatabaseEventArgs e)
        {
            MessageBox.Show(e.Error, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
