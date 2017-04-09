using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CheckOutWindow.xaml
    /// Dialog used to check out books to a patron
    /// </summary>
    public partial class CheckOutWindow : Window
    {

        private LibraryClientController controller;  //Holds a reference to controller for Library Client
        private MCardholder cardHolder;  //MCardholder to check out Book
        private ObservableCollection<Book> books;  //A list of books to check out

        /// <summary>
        /// Constructor for CheckOutWindow
        /// Initializes fields and preps the window for user interaction
        /// </summary>
        /// <param name="controller"></param>
        public CheckOutWindow(LibraryClientController controller)
        { 
            InitializeComponent();

            //initialize fields
            this.controller = controller;
            cardHolder = null;

            books = new ObservableCollection<Book>();
            BooksListView.ItemsSource = books;

            //subscribe to OnDatabaseError event
            this.controller.OnDatabaseError += OnDatabaseErrorHandler;

            //preps window for user interaction
            ResetWindow();  
        }

        /// <summary>
        /// Resets the window for user interaction
        /// </summary>
        private void ResetWindow()
        {
            ErrorLabel.Content = string.Empty;

            //sets the font to thin and Italic
            TextBox[] textBoxes = new TextBox[] { LibraryCardIDTextBox, ISBNTextBox };
            foreach(TextBox tb in textBoxes)
            {
                tb.FontStyle = FontStyles.Italic;
                tb.FontWeight = FontWeights.Thin;
            }

            //show instructions in textboxes
            LibraryCardIDTextBox.Text = "Please enter a library card ID...";
            ISBNTextBox.Text = "Press Enter to add the book to the check out list...";

            CheckOutButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;

            //Clear books
            books.Clear();

        }

        /// <summary>
        /// Event handler for LibraryCardIDTextBox getting focus.
        /// Sets the text to an empty string and changes the font to normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LibraryCardIDTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LibraryCardIDTextBox.Text = string.Empty;
            LibraryCardIDTextBox.FontStyle = FontStyles.Normal;
            LibraryCardIDTextBox.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Event handler for ISBNTextBox getting focus.
        /// Sets the text to an empty string and changes the font to normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ISBNTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ISBNTextBox.Text = string.Empty;
            ISBNTextBox.FontStyle = FontStyles.Normal;
            ISBNTextBox.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Event handler for CheckOutButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyUserID())  //if userID is valid, check out the book
            {
                CheckOutBooks();
            }
        }

        /// <summary>
        /// Verifies that the UserID exists and retrives the associated MCardholder from collection
        /// </summary>
        /// <returns>Returns true if UserID is valid, false otherwise</returns>
        private bool VerifyUserID()
        {
            cardHolder = controller.GetCardHolder(LibraryCardIDTextBox.Text);  //Retrieve MCardholder
            if (cardHolder == null)  //MCardholder not found, notify user and prep for user input
            {
                ErrorLabel.Content = "Library Card ID not found.";
                LibraryCardIDTextBox.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks out all the books in the BookListView.
        /// Shows an error in ErrorLabel if book cannot be checked out.
        /// </summary>
        private void CheckOutBooks()
        {
            if (books.Count > 0)
            {
                foreach (Book b in books)
                {
                    string error;
                    if (!controller.CheckOut(b, cardHolder, out error))
                    {
                        ErrorLabel.Content = error;
                        return;
                    }
                }

                //successfully checked out books
                MessageBox.Show("Check Out Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                //Reset window
                ResetWindow();
            }
        }

        /// <summary>
        /// Event handler for key presses in ISBNtextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ISBNTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)  //if enter key is pressed, attempt to add the book to the BookListView
            {
                AddBookToList();
                ISBNTextBox.Text = string.Empty;
                ISBNTextBox.Focus();
            }
        }

        /// <summary>
        /// Verifies the user's ISBN entry which must be a 10 or 13 digit code.
        /// If ISBN is valid, matches a Book in the collection, and has copies available for checkout,
        /// add the Book to be shown in BooksListView.
        /// </summary>
        private void AddBookToList()
        {            
            //Edit the user input to remove non-digit characters (spaces, etc.)
            StringBuilder sb = new StringBuilder();
            foreach(char c in ISBNTextBox.Text)
            {
                if (Char.IsDigit(c))  //check if character is a digit
                {
                    sb.Append(c);
                }
                else if (Char.IsLetter(c))  //if a letter is in the code, not valid.
                {
                    ErrorLabel.Content = "ISBN is not valid, must be a 10 or 13 digit code.";
                    return;
                }
            }

            string token = sb.ToString(); //get the resulting isbn

            if (token.Length != 10 && token.Length != 13)  //if isbn length is incorrect, return
            {
                ErrorLabel.Content = "ISBN is not valid, must be a 10 or 13 digit code";
                return;
            }

            Book b = controller.SearchISBN(token);  //isbn is valid, retrieve book from collection
            if (b == null)  //book not found
            {
                ErrorLabel.Content = "Book not found.  Please verify the ISBN.";
                return;
            }
            else  //book found
            {
                if (b.CopiesAvailable == 0)  //no available copies
                {
                    ErrorLabel.Content = "No copy of this book available for check out.";
                    return;
                }
                else  //Add book to ObservableCollection and set focus to ISBNTextBox
                {
                    books.Add(b);
                    ISBNTextBox.Focus();
                    CheckOutButton.IsEnabled = true;
                }
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

        /// <summary>
        /// Event handler for RemoveButton Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveBook((Book)BooksListView.SelectedItem);
        }

        /// <summary>
        /// Removes Book from Check Out list
        /// </summary>
        /// <param name="book"></param>
        private void RemoveBook(Book book)
        {
            books.Remove(book);
        }

        /// <summary>
        /// Event handler for when BookListView selection has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemoveButton.IsEnabled = true;
        }
    }
}
