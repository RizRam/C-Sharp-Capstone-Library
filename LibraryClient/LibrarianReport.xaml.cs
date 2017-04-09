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
    /// Interaction logic for LibrarianReport.xaml
    /// Window used to show a list of all Librarians in the collection
    /// </summary>
    public partial class LibrarianReport : Window
    {
        private LibraryClientController controller;  //Holds a reference to the controller for Library Client

        /// <summary>
        /// Constructor for LibrarianReport
        /// Initializes fields and populates the listBox with all MLibrarians in the collection
        /// </summary>
        /// <param name="controller"></param>
        public LibrarianReport(LibraryClientController controller)
        {
            InitializeComponent();

            this.controller = controller;
            LoadList();
        }

        /// <summary>
        /// Loads all MLibrarians in the collection to an ObservableCollection.
        /// sets the DataContext to that collection
        /// </summary>
        private void LoadList()
        {
            ObservableCollection<MLibrarian> librarianList = new ObservableCollection<MLibrarian>(controller.GetLibrarianList());
            this.DataContext = librarianList;
        }

        /// <summary>
        /// Event handler for Window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner = null;
        }
    }
}
