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
    /// Interaction logic for SearchResults.xaml
    /// Displays the search results based on the query given by MainWindow
    /// </summary>  
    public partial class SearchResults : Window
    {
        private LibraryClientController controller;  //Holds a reference to the controller for Library Client
        private string searchToken;  //the search token passed by MainWindow, used to obtain the book list
        private Book selectedBook;  //Book selected from ResultsListview.
           
        public bool HasResult { get; private set; }  //Property to show whether search token has yielded any results

        /// <summary>
        /// Constructor for SearchResults
        /// Initializes fields and preps the window for user interaction
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="searchToken"></param>
        public SearchResults(LibraryClientController controller, string searchToken)
        {
            InitializeComponent();

            //initialize fields
            this.controller = controller;
            this.searchToken = searchToken;

            //prep controls for user interaction
            SearchLabel.Content = String.Format("Search: {0}", searchToken);

            //populate ResultsListView with search results
            LoadSearchList();

            //Set the name of the UserLabel
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
        /// Retrives a list of all Book objects in collection that contain the searchToken
        /// </summary>
        public void LoadSearchList()
        {
            //reset ItemsSource
            ResultsListView.ItemsSource = null;

            //Obtain search results
            BookInventory results = controller.SearchBookInventory(this.searchToken);

            //Copy search results to observable collection which is filtered depending on user type
            ObservableCollection<Book> filtered = new ObservableCollection<Book>();

            //if the user is a patron, do not show any Book objects that have 0 total copies
            foreach (Book b in results)
            {
                if (b.NumberOfCopies == 0)
                {
                    if (controller.IsLibrarian)
                    {
                        filtered.Add(b);
                    }
                }
                else
                {
                    filtered.Add(b);
                }
            }

            //Bind the filtered list
            ResultsListView.ItemsSource = filtered;

            //Update HasResult property based on the filtered count
            if (filtered.Count == 0)
            {
                HasResult = false;
            }
            else
            {
                HasResult = true;
            }
        }

        /// <summary>
        /// Event handler for CloseButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;  //DialogResult will always be true since Window will not open if there are no results.
            this.Close();
        }

        /// <summary>
        /// Event handler for when ResultsListView selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewDetailsButton.IsEnabled = true;  //enable ViewDetailsButton
            selectedBook = (Book)ResultsListView.SelectedItem;  //Set selectedBook field to the selected item.
        }

        /// <summary>
        /// Event handler for ViewDetailsButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewDetails();
        }

        /// <summary>
        /// Opens a BookDetais window with the selectedBook as the Book to be displayed.
        /// </summary>
        private void ViewDetails()
        {
            BookDetails bookDetailsWindow = new BookDetails(selectedBook, controller, this);
            bookDetailsWindow.ShowDialog();
        }
    }
}
