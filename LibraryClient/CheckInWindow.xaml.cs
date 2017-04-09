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
    /// Interaction logic for CheckInWindow.xaml
    /// Dialog window used to check in books/checkout logs
    /// </summary>
    public partial class CheckInWindow : Window
    {

        private LibraryClientController controller;  //Holds a reference to the controller for Library Client

        /// <summary>
        /// Constructor for CheckInWindow
        /// Initializes fields and prepares the controls for user interaction
        /// </summary>
        /// <param name="controller">controller for library client</param>
        public CheckInWindow(LibraryClientController controller)
        {
            InitializeComponent();

            //intialize fields
            this.controller = controller;

            //subscribe to OnDatabaseError event
            this.controller.OnDatabaseError += OnDatabaseErrorHandler;

            //prep the controls for user interaction
            ResetSearchBar();
            CheckInButton.IsEnabled = false;
            ErrorLabel.Content = string.Empty;      
        }

        /// <summary>
        /// Preps the search bar for user interaction
        /// </summary>
        private void ResetSearchBar()
        {
            SearchTextBox.Text = "Press Enter to search...";
            SearchTextBox.FontStyle = FontStyles.Italic;
            SearchTextBox.FontWeight = FontWeights.Thin;
        }

        /// <summary>
        /// Event handler for SearchTextBox focus.
        /// Sets the text into an empty string and changes the font to normal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            SearchTextBox.FontStyle = FontStyles.Normal;
            SearchTextBox.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Event handler for KeyPresses in SearchTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Search only if Enter Key was pressed and the length of user input is at least 3 characters
            if (e.Key == Key.Enter && SearchTextBox.Text.Length >= 3)  
            {
                SearchCheckOutLogs();
            }
        }

        /// <summary>
        /// Populate the CheckOutListView with the search results.
        /// </summary>
        private void SearchCheckOutLogs()
        {
            ObservableCollection<CheckOutLog> logList = new ObservableCollection<CheckOutLog>(controller.SearchLogs(SearchTextBox.Text));
            CheckOutListView.ItemsSource = logList;
            if (logList.Count == 0)
            {
                ErrorLabel.Content = "No entries found";
            }
            else
            {
                ErrorLabel.Content = string.Empty;
            }
            
        }

        /// <summary>
        /// Event handler for CheckOutListView selection changed.  Enables the CheckInButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckOutListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckInButton.IsEnabled = true;
        }

        /// <summary>
        /// Event Handler for CheckInButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            LogCheckIn();
        }

        /// <summary>
        /// Checks in the selected CheckOutLog
        /// </summary>
        private void LogCheckIn()
        {
            CheckOutLog log = CheckOutListView.SelectedItem as CheckOutLog;
            if (log != null)  //ensure the log is not null
            {
                if (controller.CheckIn(log))  //if check in is successful
                {
                    SearchCheckOutLogs();  //repopulate the CheckOutListView with updated search query result
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
    }
}
