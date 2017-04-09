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
    /// Interaction logic for AddAuthorDialog.xaml
    /// Dialog window to create a new author
    /// </summary>
    public partial class AddAuthorDialog : Window
    {
        public string FirstNameResult { get; private set; }  //The FirstName string inputted by the user
        public string LastNameResult { get; private set; }  //The LastName string inputted by the user
        public string BioResult { get; private set; }  //The Bio string inputted by the user

        private const string BIO_TEXTBOX_DEFAULT = "(optional...)";  //default text to be displayed in BioTextBox

        /// <summary>
        /// Constructor for AddAuthorDialog
        /// Initializes fields and preps the Window for user interaction
        /// </summary>
        public AddAuthorDialog()
        {
            InitializeComponent();

            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            ErrorTextBlock.Text = string.Empty;

            ResetBioBox();

            //ensure that FirstNameTextBox is focused
            FirstNameTextBox.Focus();
        }

        /// <summary>
        /// Resets the text and style within BioTextBox
        /// </summary>
        private void ResetBioBox()
        {
            BioTextBox.Text = BIO_TEXTBOX_DEFAULT;
            BioTextBox.FontStyle = FontStyles.Italic;
            BioTextBox.FontWeight = FontWeights.Thin;
        }

        /// <summary>
        /// Event handler for CancelButton click
        /// Closes the form without setting the DialogResult to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        /// <summary>
        /// Event handler for CreateAuthorButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAuthor();
        }

        /// <summary>
        /// Creates a new author based on the user inputs
        /// </summary>
        private void CreateAuthor()
        {
            //if nothing was inputted to FirstNameTextBox, show error message and return
            if (FirstNameTextBox.Text.Length == 0)
            {
                ErrorTextBlock.Text = "Please enter a first name.";
                FirstNameTextBox.Focus();
                return;
            }

            //if nothing was entered in LastNameTextBox, show error message and return
            if (LastNameTextBox.Text.Length == 0)
            {
                ErrorTextBlock.Text = "Please enter a last name.";
                LastNameTextBox.Focus();
                return;
            }

            //Show confirmation messagebox, if the messagebox result is Yes, set the dialog result to true and close this window.
            if (MessageBox.Show("Are you sure you want to create this author?", "Confirm", MessageBoxButton.YesNo, 
                MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                FirstNameResult = FirstNameTextBox.Text;
                LastNameResult = LastNameTextBox.Text;
                
                if (BioTextBox.Text == BIO_TEXTBOX_DEFAULT)
                {
                    BioResult = string.Empty;
                }
                else
                {
                    BioResult = BioTextBox.Text;
                }
                

                DialogResult = true;
                this.Close();
            }            
        }

        /// <summary>
        /// Event handler for when BioTextBox gets focus
        /// Sets the text to an empty string and changes the font to normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BioTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            BioTextBox.Text = string.Empty;
            BioTextBox.FontStyle = FontStyles.Normal;
            BioTextBox.FontWeight = FontWeights.Normal;
        }
    }
}
