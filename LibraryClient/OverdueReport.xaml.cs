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
    /// Interaction logic for OverdueReport.xaml
    /// Displays all overdue checkouts currently in the system.
    /// </summary>
    public partial class OverdueReport : Window
    {
        private LibraryClientController controller;  //Holds a reference to controller for Library Client

        /// <summary>
        /// Constructor for OverdueReport
        /// Initializes fields and Populates the list with overdue logs.
        /// </summary>
        /// <param name="controller"></param>
        public OverdueReport(LibraryClientController controller)
        {
            InitializeComponent();

            this.controller = controller;
            LoadOverdueList();
        }

        /// <summary>
        /// Retrieves a list of overdue CheckOutLogs and Binds the list to overDueListView 
        /// </summary>
        private void LoadOverdueList()
        {
            ObservableCollection<KeyValuePair<CheckOutLog, MCardholder>> overdueList = 
                new ObservableCollection<KeyValuePair<CheckOutLog, MCardholder>>(controller.GetOverdueList());            

            overDueListView.ItemsSource = overdueList;
        }
    }
}
