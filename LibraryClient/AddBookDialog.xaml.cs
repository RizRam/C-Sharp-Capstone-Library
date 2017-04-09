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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryClient
{
    /// <summary>
    /// Interaction logic for AddBookDialog.xaml
    /// Dialog window to add a new book
    /// </summary>
    public partial class AddBookDialog : Window
    {
        //enum that contains all possible inquiry state of the window
        private enum Question
        {
            ISBN,  //window is inquiring the ISBN of the book
            BookExists,  //the book already exists and asks for further confirmation
            NewBook  //isbn is a book that does not yet exist in the database
        };

        private const double FADE_TIME = 0.5;  //the duration of fade animations

        private Question phase;  //the current inquiry state of the winodw
        private Control[] controls;  //an array of all Control objects in the window
        private LibraryClientController controller;  //holds a reference to the controller object of the library client
        private string ISBN;  //the ISBN inputted by the user

        public Book Book { get; private set; }  //Book object that corresponds to the inputted ISBN, initialized to null.
        
        /// <summary>
        /// Constructor for AddBookDialog
        /// Initializes fields and preps the Window for user interaction
        /// </summary>
        /// <param name="controller"></param>
        public AddBookDialog(LibraryClientController controller)
        {
            //intialize fields and Properties
            InitializeComponent();

            controls = new Control[] { QuestionLabel, YesButon, NoButton, ISBNEntryTextBox };

            this.controller = controller;
            Book = null;
            ISBN = string.Empty;

            //subscribe to OnDatabaseError event
            this.controller.OnDatabaseError += OnDatabaseErrorHandler;

            //Initialize the controls
            InitializeControls();

            ResetDialog();
        }

        /// <summary>
        /// Initializes the controls for window load
        /// </summary>
        private void InitializeControls()
        {          
            foreach(Control c in controls)
            {
                c.Opacity = 0.0;
            }

            YesButon.IsEnabled = false;
            NoButton.IsEnabled = false;
        }

        /// <summary>
        /// Sets the window state to ISBN phase
        /// </summary>
        private void ResetDialog()
        {
            //fade out all controls
            foreach(Control c in controls)
            {
                FadeOutControl(c);
            }

            //set phase to ISBN
            phase = Question.ISBN;

            //QuestionLabel
            QuestionLabel.Content = "Please enter an ISBN exactly\nMust be a 10 or 13 digit code";
            FadeInControl(QuestionLabel);

            //ISBN Bar
            ISBNEntryTextBox.Text = "Press Enter to confirm...";
            ISBNEntryTextBox.FontStyle = FontStyles.Italic;
            ISBNEntryTextBox.FontWeight = FontWeights.Thin;
            FadeInControl(ISBNEntryTextBox);            
        }

        /// <summary>
        /// Sets the state of the window to a confirmation dialog that depends on the current phase of the hbook
        /// </summary>
        private void ConfirmDialog()
        {
            //Fadeout all controls
            foreach(Control c in controls)
            {
                FadeOutControl(c);
            }

            //Determine the QuestionLabel content.
            if (phase == Question.BookExists)
            {
                QuestionLabel.Content = "This Book already exists.\nWould you like to view\nand edit its details?";
            }
            else if (phase == Question.NewBook)
            {
                QuestionLabel.Content = "This Book does not yet exist.\nWould you like to enter its details?";
            }

            //Fade in relevant controls
            FadeInControl(QuestionLabel);
            FadeInControl(YesButon);
            FadeInControl(NoButton);
        }

        /// <summary>
        /// Fades out the control with an animation
        /// </summary>
        /// <param name="control">Control to be faded out</param>
        private void FadeOutControl(Control control)
        {
            if (control.Opacity != 0.0)  //only do it if the control is not already faded out.
            {
                //Create and initialize DoubleAnimation object
                DoubleAnimation fadeOut = new DoubleAnimation();
                fadeOut.From = 1.0;
                fadeOut.To = 0.0;
                fadeOut.Duration = TimeSpan.FromSeconds(FADE_TIME);
                fadeOut.BeginTime = TimeSpan.FromSeconds(0.0);

                //Animate and disable control
                control.BeginAnimation(Control.OpacityProperty, fadeOut);
                control.IsEnabled = false;                
            }   
        }

        /// <summary>
        /// Fades in control with an animation
        /// </summary>
        /// <param name="control">Control to be faded in</param>
        private void FadeInControl(Control control)
        {
            //Create and initialize DoubleAnimation object
            DoubleAnimation fadeIn = new DoubleAnimation();
            fadeIn.From = 0.0;
            fadeIn.To = 1.0;
            fadeIn.Duration = TimeSpan.FromSeconds(FADE_TIME);
            fadeIn.BeginTime = TimeSpan.FromSeconds(0.0);

            //Enable control and animate
            control.IsEnabled = true;
            control.BeginAnimation(Control.OpacityProperty, fadeIn);
        }

        /// <summary>
        /// Event handler for when ISBNEntryTextBox gets focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ISBNEntryTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //set the text to empty and change the font to normal.
            ISBNEntryTextBox.Text = string.Empty;
            ISBNEntryTextBox.FontStyle = FontStyles.Normal;
            ISBNEntryTextBox.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Event handler for key presses in ISBENtryTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ISBNEntryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)  //if the key is the Enter key, make a query
            {
                QueryISBN();
            }
        }

        /// <summary>
        /// Sets the phase of the window to its appropriate phase depending on whether the Book already exists.
        /// Changes the state of the window to the confirmation state.
        /// </summary>
        private void QueryISBN()
        {
            if (CheckISBN()) //check if ISBN is valid
            {
                Book = SearchISBN();  //search for the Book
                if (Book != null)  //if Book was found
                {
                    phase = Question.BookExists;
                }
                else  //Book not found
                {
                    phase = Question.NewBook;
                }

                //Confirm
                ConfirmDialog();
            }
        }

        /// <summary>
        /// Checks whether the ISBN entered in ISBNEntryTextBox is a valid ISBN (10 or 13 digit numeric code)
        /// Sets the ISBN property to the valid ISBN entry, if ISBN is not valid the window is reset.
        /// </summary>
        /// <returns>Returns true if valid, false otherwise</returns>
        private bool CheckISBN()
        {
            //remove non-alphanumeric numbers
            StringBuilder sb = new StringBuilder();
            foreach(char c in ISBNEntryTextBox.Text)
            {
                if (Char.IsLetterOrDigit(c)) //remove alphanumeric
                {
                    if (Char.IsDigit(c))  //append to StringBuilder only if it's a digit
                    {
                        sb.Append(c);
                    }
                    else  //not a digit, invalid ISBN
                    {
                        QuestionLabel.Content = "ISBN is not valid.\nMust be a 10 or 13 digit code.";
                        return false;
                    }
                }
            }

            ISBNEntryTextBox.Text = sb.ToString();  //change ISBNEntryTextBox text to appropriate ISBN format

            if (ISBNEntryTextBox.Text.Length != 10 && ISBNEntryTextBox.Text.Length != 13) //isbn not 10 or 13 digits long
            {
                QuestionLabel.Content = "ISBN is not valid.\nMust be a 10 or 13 digit code.";
                return false;
            }

            ISBN = ISBNEntryTextBox.Text;
            return true;
        }

        /// <summary>
        /// Search collection for a Book matching user input ISBN
        /// </summary>
        /// <returns>Returns Book that corresponds to ISBN Property value, returns null if Book is not found</returns>
        private Book SearchISBN()
        {
            return controller.SearchISBN(ISBN);
        }

        /// <summary>
        /// Event handler for YesButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesButon_Click(object sender, RoutedEventArgs e)
        {
            OpenBookDetails();
        }

        /// <summary>
        /// Shows a BookDetails dialog window that displays the information of the new or found Book
        /// </summary>
        private void OpenBookDetails()
        {
            BookDetails bookDetails;

            if (Book != null) //open BookDetails with existing book
            {
                bookDetails = new BookDetails(Book, controller, false);
                bookDetails.ShowDialog();
            }
            else  //open BookDetails with newly created book
            {
                Book book = new Book { Title = string.Empty, Description = string.Empty,
                                       Language = string.Empty, NumPages = 0, Author = null,
                                       Publisher = string.Empty, Subject = string.Empty, YearPublished = 1400,
                                       ISBN = this.ISBN, NumberOfCopies = 0 };

                //Add new Book to databse
                controller.AddBook(book);

                bookDetails = new BookDetails(book, controller, true);

                //if BookDetails Dialog returns false, meaning the user did not want to save changes,
                //remove the new book from the database and local collection.
                if (!(bool)bookDetails.ShowDialog())
                {
                    controller.RemoveBook(book);
                }
            }


            this.Close();
        }

        /// <summary>
        /// Event handler for NoButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            ResetDialog();
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
