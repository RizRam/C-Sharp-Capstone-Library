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
    /// Interaction logic for BookCopiesDialog.xaml
    /// Dialog window used to add or remove a Book's total copies
    /// </summary>
    public partial class BookCopiesDialog : Window
    {
        //Enum containing all the possible states of the window
        public enum AddOrRemove { Add, Remove };

        public AddOrRemove addOrRemove;  //a flag of the state of the window
        public int CopiesResult { get; private set; }  //Copies of books inputted by the user

        //Constructor
        //Uses a BookDetails window as a parameter and AddOrRemove enum
        /// <summary>
        /// Constructor for BookCopiesDialog
        /// Initializes fields and preps the Window for user interaction
        /// </summary>
        /// <param name="bookDetails">BookDetails window that opened this window</param>
        /// <param name="addOrRemove">AddorRemove enum that preps the interaction for adding or removing Book copies</param>
        public BookCopiesDialog(BookDetails bookDetails, AddOrRemove addOrRemove)
        {
            InitializeComponent();

            //Initialize properties and fields
            Owner = bookDetails;
            this.addOrRemove = addOrRemove;

            //Set the QuestionTextBlock content
            if (addOrRemove == AddOrRemove.Add)  //if user wants to add copies
            {
                QuestionTextBlock.Text = "How many copies would you like to add?";
            }
            else if (addOrRemove == AddOrRemove.Remove)  //if user wants to deletec copies
            {
                string question = string.Format("How many copies would you like to remove?\n\n({0} copies are available for removal)", 
                    (Owner as BookDetails).Book.CopiesAvailable);
                QuestionTextBlock.Text = question;
            }

            NumberTextBox.Text = "0";
            ErrorLabel.Content = string.Empty;
        }

        /// <summary>
        /// Event handler for CancelButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Event handler for OKButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmInput();
        }

        /// <summary>
        /// Confirms whether the user input is valid. (Positive integers only).
        /// </summary>
        private void ConfirmInput()
        {
            //get number
            int copies;
            if (int.TryParse(NumberTextBox.Text, out copies) && copies > 0)  //if input is positive integer
            {
                //show confirm message box and only proceed if user clicks Yes
                if (MessageBox.Show("Confirm", "Confirm", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (addOrRemove == AddOrRemove.Add)  //if window is in Add state
                    {
                        AddCopy(copies);
                    }
                    else if (addOrRemove == AddOrRemove.Remove)  //if window is in Remove state
                    {
                        RemoveCopy(copies);
                    }
                }
            }
            else  //input is not valid
            {
                ErrorLabel.Content = "Please enter a positive integer larger than 0";
                NumberTextBox.Focus();
                return;
            }
        }

        /// <summary>
        /// Sets the CopiesResult Property to copies value passed in argument.
        /// Sets the DialogResult to true and cloes the window.
        /// </summary>
        /// <param name="copies">number of copies to add</param>
        private void AddCopy(int copies)
        {
            CopiesResult = copies;
            DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Sets the CopiesResult Property to copies value passed in argument and closes the window if copies is a valid amount.
        /// If copies exceeds the amount of copis available for checkout, then displays an error and focuses back on NumberTextBox
        /// </summary>
        /// <param name="copies">number of copies to remove</param>
        private void RemoveCopy(int copies)
        {
            //Check if can remove that amount of copies
            BookDetails bd = Owner as BookDetails;
            if (bd != null)
            {
                if (copies <= bd.Book.CopiesAvailable)  //if copies is a valid number
                {
                    CopiesResult = copies;
                    DialogResult = true;
                    this.Close();
                }
                else  //if copies is not a valid number
                {
                    ErrorLabel.Content = String.Format("Only {0} copies are available to remove", bd.Book.CopiesAvailable);
                    NumberTextBox.Focus();
                    return;
                }
            }
        }
        
        /// <summary>
        /// Event handler for when NumberTextBox gets focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NumberTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Event handler for keypresses in NumberTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmInput();
            }
        }
    }
}
