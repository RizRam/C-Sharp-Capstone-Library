using System;
using System.Collections;
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
    /// Interaction logic for MainWindow.xaml
    /// Primary Window and allows user input for Book searches as well as Librarian LogIn
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibraryClientController controller;  //Holds a reference to the controller for the Library Client

        /// <summary>
        /// Constructor for MainWindow
        /// Initializes fields and preps window for user interaction
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //initialize fields
            controller = new LibraryClientController();  //create a new LibraryClientController

            //subscribe to OnDatabaseError event
            controller.OnDatabaseError += OnDatabaseErrorHandler;

            //Set the user type
            SetUserType();

            ErrorLabel.Content = string.Empty;
        }

        /// <summary>
        /// Event Handler for Window Visibility change
        /// Used when window shows after being hidden, preps the window to display certain controls,
        /// depending on the user type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //set user type   
            SetUserType();

            //reset the search bar
            ResetSearchBox();

            //Hide or display LogIn Button
            if (controller.IsLibrarian)
            {
                LogInButton.Visibility = Visibility.Hidden;
            }
            else
            {
                LogInButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Preps the SearchBox textbox for user interaction
        /// </summary>
        private void ResetSearchBox()
        {
            SearchBox.Text = "Please enter a search term...";
            SearchBox.FontStyle = FontStyles.Italic;
            SearchBox.FontWeight = FontWeights.Thin;
        }

        /// <summary>
        /// Event handler for ExitButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event handler for SearchButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.Length >= 3)
            {
                Search(SearchBox.Text);
            }
        }

        /// <summary>
        /// Event Handler for Key presses in SearchBox textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter  && SearchBox.Text.Length >= 3)  //If user press enter key and input is at least 3 characters long
            { 
                Search(SearchBox.Text);                
            }
        }

        /// <summary>
        /// Opens a SearchResult Dialog passing the user search input as the searchToken.
        /// If SearchResults does not have any results, no SearchResult window is opened.
        /// </summary>
        /// <param name="searchToken">string user search input, used to display search results in SearchResults Window</param>
        private void Search(string searchToken)
        {
            SearchResults searchResults = new SearchResults(controller, searchToken);
            if (searchResults.HasResult)  //SearchResult has results with the searchToken
            {
                searchResults.ShowDialog();
            }
            else  //SearchResult has not found any results.  Update the ErrorLabel and immediately close SearchResult window
            {
                ErrorLabel.Content = "No books found.";
                searchResults.Close();
            }
        }

        /// <summary>
        /// Event handler for when SearchBox gets focus
        /// Sets the SearchBox textbox text to an empty string and changes the font to normal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = null;
            SearchBox.FontStyle = FontStyles.Normal;
            SearchBox.FontWeight = FontWeights.Normal;
            SearchButton.IsEnabled = true;
        }

        /// <summary>
        /// Event Handler for LogInButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            LogInAsLibrarianAndOpenMenu();
        }

        /// <summary>
        /// Allows user to log in to client as a librarian.
        /// If login is successful, MainWindow will reflect the change and a Librarian Menu will be displayed
        /// </summary>
        private void LogInAsLibrarianAndOpenMenu()
        {
            LogIn logInWindow = new LogIn(controller);  //Open Login Window dialog
            if ((bool)logInWindow.ShowDialog())  //if Login is succcessful
            {
                //Hide the LogIn button and set the user Type.
                LogInButton.Visibility = Visibility.Hidden;
                SetUserType();

                //Hide main window and show librarian window.
                this.Hide();
                LibrarianMenu librarianMenu = new LibrarianMenu(controller, this);
                librarianMenu.Show();
            }
        }

        /// <summary>
        /// Changes the UserTypeLabel depending on whether the user is a librarian or patron.
        /// </summary>
        private void SetUserType()
        {
            if (controller.IsLibrarian)
            {
                UserTypeLabel.Content = "Librarian";
                ExitButton.Content = "Close";
            }
            else
            {
                UserTypeLabel.Content = "Patron";
                ExitButton.Content = "Exit";
            }
        }

        /// <summary>
        /// Event handler for Window closisng
        /// Used to prevent the client from fully closing this window if the client is a librarian.
        /// Instead the Ownder of this window will display (LibrarianMenu)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Owner != null)
            {
                e.Cancel = true;
                this.Hide();
                Owner.Show();
            }
        }

        /// <summary>
        /// Event handler for OnDatabaseError events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDatabaseErrorHandler(object sender, DatabaseEventArgs e)
        {
            MessageBox.Show(e.Error);
        }

        /// <summary>
        /// Event handler for Window closed
        /// Make sure to dispose of controller since it uses an external database resource.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            controller.Dispose();
        }
    }
     
}
