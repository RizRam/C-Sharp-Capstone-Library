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
    /// Interaction logic for AuthorsReport.xaml
    /// Window to show a list of all availaable authors in collection and their authored books
    /// </summary>
    public partial class AuthorsReport : Window
    {

        private LibraryClientController controller;  //Holds a reference to the controller of the library client

        /// <summary>
        /// Constructor for AuthorsReport
        /// Initializes fields and populate the AuthorsListView with all MAuthor objects in collection
        /// </summary>
        /// <param name="controller"></param>
        public AuthorsReport(LibraryClientController controller)
        {
            InitializeComponent();

            this.controller = controller;

            //Populate AuthorsTreeView
            LoadAuthorsList();
        }

        /// <summary>
        /// Populates AuthorsTreeView with all MAuthor objects in collection
        /// </summary>
        private void LoadAuthorsList()
        {
            ObservableCollection<MAuthor> authorsList = new ObservableCollection<MAuthor>(controller.GetAuthorsList());
            AuthorsTreeView.ItemsSource = authorsList;
        }

        /// <summary>
        /// Event handler for Window Closing
        /// Ensures that the Owner of this Window is null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner = null;
        }
    }
}
