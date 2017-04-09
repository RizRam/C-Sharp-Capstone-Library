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
    /// Interaction logic for CardholdersReport.xaml
    /// Window used to show a list of all Cardholders in collection
    /// </summary>
    public partial class CardholdersReport : Window
    {
        private LibraryClientController controller;  //holds a reference to the controller for Library Client

        /// <summary>
        /// Constructor for CardholdersReport.
        /// Initializes fields and loads the report into the CardholdersTreeView
        /// </summary>
        /// <param name="controller">controller for library client</param>
        public CardholdersReport(LibraryClientController controller)
        {
            InitializeComponent();

            this.controller = controller;
            LoadCardHoldersTree();
        }

        /// <summary>
        /// Loads the list of cardHolders into the CardholdersTreeView control
        /// </summary>
        private void LoadCardHoldersTree()
        {
            ObservableCollection<MCardholder> cardHoldersList = new ObservableCollection<MCardholder>(controller.GetCardholdersList());
            CardholdersTreeView.ItemsSource = cardHoldersList;
        }
    }
}
