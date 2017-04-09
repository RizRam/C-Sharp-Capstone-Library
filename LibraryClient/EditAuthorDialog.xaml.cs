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
    /// Interaction logic for EditAuthorDialog.xaml
    /// Window used to change a Book's author
    /// </summary>
    public partial class EditAuthorDialog : Window
    {
        private LibraryClientController controller;  //Holds a reference to the controller for Library Client
        private ObservableCollection<MAuthor> authorList;  //ObservableCollection of all MAuthors in collection
        private ObservableCollection<MAuthor> filteredList;  //A filtred list of Authors that can be shown, depends on user type

        public MAuthor Result { get; private set; }  //The Result of the dialog window

        /// <summary>
        /// Constructor for EditAuthorDialog
        /// Initializes the fields and Properties and preps the window for user interaction
        /// </summary>
        /// <param name="bookDetails"></param>
        /// <param name="controller"></param>
        public EditAuthorDialog(BookDetails bookDetails, LibraryClientController controller)
        {
            InitializeComponent();

            //Initialize fields and properties
            Owner = bookDetails;
            this.controller = controller;

            LoadAuthors();
            DataContext = authorList;  //sets the DataContext to all available authors (shows all authors)

            //subscribe to OnDatabaseError event
            this.controller.OnDatabaseError += OnDatabaseErrorHandler;

            //prep controls for user interaction
            ResetSearchBar();
            SelectButton.IsEnabled = false;
        }

        /// <summary>
        /// Loads all MAuthors in collection to ObservableCollection authorsList
        /// </summary>
        private void LoadAuthors()
        {
            authorList = new ObservableCollection<MAuthor>(
                (from a in controller.GetAuthorsList()
                 select a).ToList());                        
        }        

        /// <summary>
        /// Preps SearchTextBox for user interaction
        /// </summary>
        private void ResetSearchBar()
        {
            SearchTextBox.Text = "Press Enter to search...";
            SearchTextBox.FontStyle = FontStyles.Italic;
            SearchTextBox.FontWeight = FontWeights.Thin;
        }

        /// <summary>
        /// Event handler for when SearchTextBox gets focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PrepSearchBar();
        }

        /// <summary>
        /// Changes the SearchTextBox text to an empty string and sets the font to normal
        /// </summary>
        private void PrepSearchBar()
        {
            SearchTextBox.Text = string.Empty;
            SearchTextBox.FontStyle = FontStyles.Normal;
            SearchTextBox.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Event handler for ClearButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            //Reset SearchTextBox
            ResetSearchBar();

            //display all available authors instead of filtered
            DataContext = authorList;
        }

        /// <summary>
        /// Event handler for key presses in SearchTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchTextBox.Text.Length >= 3)  //if key pressed is Enter key and the length of user input is at least 3
            {
                SearchAuthors(SearchTextBox.Text);  //filter down the author list
            }
        }
        
        /// <summary>
        /// Sets the DataContext to a filtered ObservableCollection of MAuthors.
        /// The filter is determined by the string passed in the argument.
        /// </summary>
        /// <param name="token">A search token to filter the list of authors</param>
        private void SearchAuthors(string token)
        {
            filteredList = new ObservableCollection<MAuthor>(
                (from a in authorList
                 where a.ContainsToken(token)
                select a).ToList());

            DataContext = filteredList;
        }

        /// <summary>
        /// Event handler for SelectButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectAuthor();
        }

        /// <summary>
        /// Displays a confirmation dialog.  If user confirms selection, change appropriate Properties and close the window
        /// </summary>
        private void SelectAuthor()
        {
            if (MessageBox.Show("Are you sure you would like to make this change?", "Confirm", MessageBoxButton.YesNo, 
                MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Result = (MAuthor)AuthorsListBox.SelectedItem;
                DialogResult = true;
                this.Close();
            }   
        }

        /// <summary>
        /// EventHandler for selection change in AuthorsListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //enable the select button
            SelectButton.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for AddAuthorButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewAuthor();
        }

        /// <summary>
        /// Opens an AddAuthorDialog window to allow user to create a new author to be added to the list.
        /// </summary>
        private void AddNewAuthor()
        {
            AddAuthorDialog addAuthorDialog = new AddAuthorDialog();
            if ((bool)addAuthorDialog.ShowDialog())  //if dialog result is true (user confirmed changes)
            {
                controller.CreateAuthor(addAuthorDialog.FirstNameResult, addAuthorDialog.LastNameResult, addAuthorDialog.BioResult);  //create new author
                LoadAuthors();  //retrieve updated list of authors
                ResetSearchBar();  //Reset the SearchTextBox
                DataContext = authorList;  //Populate the AuthorsTextBox with updated authorsList.
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
