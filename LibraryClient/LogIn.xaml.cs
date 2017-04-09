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
    /// Interaction logic for LogIn.xaml
    /// Dialog window used to allow user to log in as a librarian
    /// </summary>
    public partial class LogIn : Window
    {
        private LibraryClientController controller;  //Holds a reference to controller for Library Client

        /// <summary>
        /// Constructor for LogIn
        /// Initializes fields and preps window for user interaction
        /// </summary>
        /// <param name="controller">controller for Library Client</param>
        public LogIn(LibraryClientController controller)
        {
            InitializeComponent();

            //initialize field
            this.controller = controller;

            //prep controls
            ErrorLabel.Content = string.Empty;
            UserNameTextBox.Text = string.Empty;

            UserNameTextBox.Focus();
        }
        
        /// <summary>
        /// Event handler for LogInButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            VerifyLogIn(UserNameTextBox.Text, passwordBox.Password);
        }

        /// <summary>
        /// Verify tha tthe userName and password entries are valid
        /// </summary>
        /// <param name="userName">string for librarian user name</param>
        /// <param name="password">string for librarian password</param>
        private void VerifyLogIn(string userName, string password)
        {
            if (userName.Length == 0)  //Ensure that user name input is not empty
            {
                ErrorLabel.Content = "Please enter your user name.";
                ErrorLabel.Foreground = Brushes.Red;
                return;
            }

            if (password.Length == 0)
            {
                ErrorLabel.Content = "Please enter your password.";
                ErrorLabel.Foreground = Brushes.Red;
                return;
            }

            if (controller.LogIn(userName, password))  //Verify user name and password, set DialogResult to true and close window.
            {
                DialogResult = true;
                this.Close();
            }
            else  //user name and/or password incorrect
            {
                ErrorLabel.Content = "UserName / Password is invalid.";
            }
        }

        /// <summary>
        /// Event Handler for CancelButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Eventhandler for Key presses in passwordBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VerifyLogIn(UserNameTextBox.Text, passwordBox.Password);
            }
        }
    }
}
