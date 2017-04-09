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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryClient
{
    /// <summary>
    /// Interaction logic for LibrarianMenu.xaml
    /// Window that displays the menu available only to Librarians
    /// </summary>
    public partial class LibrarianMenu : Window
    {
        /// <summary>
        /// Enum of all possible window phases/states
        /// </summary>
        private enum MenuPhase
        {
            Start,  //Starting phase of the menu
            Reports,  //menu is displaying reports buttons
            CheckOut  //menu is displaying checkout buttons
        }

        private LibraryClientController controller;  //Holds a reference to the controller for the Library Client
        private MainWindow mainWindow;  //Holds a reference to the MainWindow
        private Button[] reportsButtonArray;  //an array of all the Reports Buttons
        private Button[] mainButtonArray;  //an array of the main menu buttons
        private Button[] checkOutInButtonArray;  //an array of the check out, check in buttons
        private MenuPhase phase;  //the current phase of the window

        /// <summary>
        /// Constructor for LibrarianMenu.
        /// Initializes fields and preps the window for user interaction.
        /// </summary>
        /// <param name="controller">controller for library blient</param>
        /// <param name="mainWindow">reference to the MainWindow that opened this menu</param>
        public LibrarianMenu(LibraryClientController controller, MainWindow mainWindow)
        {
            InitializeComponent();            

            //initialize fields
            this.controller = controller;
            this.mainWindow = mainWindow;
            phase = MenuPhase.Start;
            reportsButtonArray = new Button[] {LibrariansReportButton, CardholdersReportButton,
                                               AuthorsReportButton, OverdueBooksReportButton};
            mainButtonArray = new Button[] { SearchButton, CheckOutInButton, ReportsButton, AddBook };
            checkOutInButtonArray = new Button[] { CheckOutButton, CheckInButon };

            //prep buttons for user interaction
            InitializeReportsButtons();
            InitializeCheckOutInButtons();
        }

        /// <summary>
        /// Preps all report buttons for window load.
        /// </summary>
        private void InitializeReportsButtons()
        {
            foreach (Button b in reportsButtonArray)
            {
                b.IsEnabled = false;
                b.Opacity = 0.0;
            }
        }

        /// <summary>
        /// Preps all Check Out, Check In buttons for window load.
        /// </summary>
        private void InitializeCheckOutInButtons()
        {
            foreach(Button b in checkOutInButtonArray)
            {
                b.IsEnabled = false;
                b.Opacity = 0.0;
            }
        }

        #region Search

        /// <summary>
        /// Event handler for SearchButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow();
        }

        /// <summary>
        /// Show the mainWindow from its hidden state.
        /// </summary>
        private void OpenSearchWindow()
        {
            mainWindow.Owner = this;
            mainWindow.Show();
        }

        #endregion Search

        #region Menu animations

        /// <summary>
        /// Event handler for ReportsButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            //Move and show the appropriate buttons and set to appropriate phase
            switch (phase)
            {
                case MenuPhase.Start:  //at start
                    ShowReportsButtons();
                    phase = MenuPhase.Reports;
                    break;
                case MenuPhase.Reports:  //reports buttons already showing, change to start
                    ResetWindow();
                    phase = MenuPhase.Start;
                    break;
                default:  //reset window then show reports
                    ResetWindow();
                    ShowReportsButtons();
                    phase = MenuPhase.Reports;
                    break;               
            }
        }

        /// <summary>
        /// Event Handler for CheckOutInButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckOutInButton_Click(object sender, RoutedEventArgs e)
        {
            //Move and show appropriate buttons and set to appropriate phase
            switch (phase)
            {
                case MenuPhase.CheckOut:  //already in checkout phase, change to start
                    ResetWindow();
                    phase = MenuPhase.Start;
                    break;
                case MenuPhase.Start:  //at start, show checkout buttons
                    ShowCheckOutInButtons();
                    phase = MenuPhase.CheckOut;
                    break;
                default:  //in other phase, reset and then show checkout/in buttons
                    ResetWindow();
                    ShowCheckOutInButtons();
                    phase = MenuPhase.CheckOut;
                    break;
            }
        }

        /// <summary>
        /// Resets the window to start phase
        /// </summary>
        private void ResetWindow()
        {
            switch(phase)
            {
                case MenuPhase.Reports:  //if in reports phase hide the reports buttons
                    HideReportsButtons();
                    break;
                case MenuPhase.CheckOut:  //if in checkout phase, hid the checkout/in butttons
                    HideCheckOutInButtons();
                    break;
                default:  //already at start, do nothing
                    break;                   
            }
        }

        /// <summary>
        /// Animates the Reports buttons to show.
        /// </summary>
        private void ShowReportsButtons()
        {
            double animationTime = 0.25;

            for (int i = 0; i < reportsButtonArray.Length; i++)
            {
                Button b = reportsButtonArray[i];
                DoubleAnimation animation = new DoubleAnimation();
                animation.BeginTime = TimeSpan.FromSeconds(animationTime * i);
                animation.From = 0.0;
                animation.To = 1.0;
                animation.Duration = new Duration(TimeSpan.FromSeconds(animationTime));
                b.BeginAnimation(Button.OpacityProperty, animation);

                //enable the button
                b.IsEnabled = true;
            }
        }

        /// <summary>
        /// Animates the reports buttons to hide
        /// </summary>
        private void HideReportsButtons()
        {
            double animationTime = 0.25;

            for (int i = 0; i < reportsButtonArray.Length; i++)
            {
                Button b = reportsButtonArray[reportsButtonArray.Length - 1 - i];
                DoubleAnimation animation = new DoubleAnimation();
                animation.BeginTime = TimeSpan.FromSeconds(animationTime * i);
                animation.From = 1.0;
                animation.To = 0.0;
                animation.Duration = new Duration(TimeSpan.FromSeconds(animationTime));
                b.BeginAnimation(Button.OpacityProperty, animation);

                //disable the button
                b.IsEnabled = false;
            }
        }

        /// <summary>
        /// Animates the buttons in the form to show the CheckOut/In buttons
        /// </summary>
        private void ShowCheckOutInButtons()
        {
            double xShift = -50;
            double animationTime = 0.5;
            double showTime = 0.25;

            //shift main buttons to the left
            foreach (Button b in mainButtonArray)
            {
                DoubleAnimation da = new DoubleAnimation();
                TranslateTransform tf = new TranslateTransform();
                da.Duration = TimeSpan.FromSeconds(animationTime);                
                da.From = 0.0;
                da.To = xShift;
                b.RenderTransform = tf;
                tf.BeginAnimation(TranslateTransform.XProperty, da);
            }

            //show check out/in buttons
            for (int i = 0; i < checkOutInButtonArray.Length; i++)
            {
                Button b = checkOutInButtonArray[checkOutInButtonArray.Length - 1 - i];                
                DoubleAnimation animation = new DoubleAnimation();
                animation.BeginTime = TimeSpan.FromSeconds(animationTime + (showTime * i));
                animation.From = 0.0;
                animation.To = 1.0;
                animation.Duration = new Duration(TimeSpan.FromSeconds(showTime));
                b.BeginAnimation(Button.OpacityProperty, animation);

                //Enable button
                b.IsEnabled = true;
            }

        }

        private void HideCheckOutInButtons()
        {
            double xShift = -50;
            double animationTime = 0.5;
            double showTime = 0.25;

            //hide check out/in buttons
            for (int i = 0; i < checkOutInButtonArray.Length; i++)
            {
                Button b = checkOutInButtonArray[checkOutInButtonArray.Length - 1 - i];
                DoubleAnimation animation = new DoubleAnimation();
                animation.BeginTime = TimeSpan.FromSeconds(animationTime + (showTime * i));
                animation.From = 1.0;
                animation.To = 0.0;
                animation.Duration = new Duration(TimeSpan.FromSeconds(showTime));
                b.BeginAnimation(Button.OpacityProperty, animation);

                //Disable button
                b.IsEnabled = false;
            }

            //shift main buttons back to center
            foreach (Button b in mainButtonArray)
            {
                DoubleAnimation da = new DoubleAnimation();
                TranslateTransform tf = new TranslateTransform();
                da.Duration = TimeSpan.FromSeconds(animationTime);
                da.From = xShift;
                da.To = 0.0;
                b.RenderTransform = tf;
                tf.BeginAnimation(TranslateTransform.XProperty, da);
            }
        }

        #endregion Menu animations

        #region LogOut

        /// <summary>
        /// Event handler for LogOutButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Logs out librarian, and shows the mainWindow.
        /// </summary>
        private void LogOut()
        {
            controller.LogOut();
            mainWindow.Owner = null;
            mainWindow.Show();
        }

        /// <summary>
        /// Event handler for Window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Display confirmation dialog for logging out
            if (MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButton.YesNo,
                                MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                LogOut();  //if user selects yes, logout
            }
            else  // cancel the window close.
            {
                e.Cancel = true;
            }
        }

        #endregion LogOut

        #region Reports

        /// <summary>
        /// Event handler for LibrariansReportButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LibrariansReportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenLibrariansReport();
        }

        /// <summary>
        /// Shows the LibrariansReportWIndow, if window is already opened, bring to focus
        /// </summary>
        private void OpenLibrariansReport()
        {
            //check if window is already opened
            foreach (Window w in OwnedWindows)
            {
                if (w is LibrarianReport)
                {
                    w.Focus();
                    return;
                }
            }

            //open a new LibrarianReport window.
            LibrarianReport librarianReport = new LibrarianReport(controller);
            librarianReport.Owner = this;
            librarianReport.Show();
        }

        /// <summary>
        /// Event handler for CardholdersReportButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardholdersReportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCardholdersReport();
        }

        /// <summary>
        /// Shows the CardholdersReport window, if window is already opened bring to focus.
        /// Otherwise, open a new window.
        /// </summary>
        private void OpenCardholdersReport()
        {
            //check if window is already opened
            foreach (Window w in OwnedWindows)
            {
                if (w is CardholdersReport)
                {
                    w.Focus();
                    return;
                }
            }

            //show new window
            CardholdersReport cardholdersReport = new CardholdersReport(controller);
            cardholdersReport.Owner = this;
            cardholdersReport.Show();
        }

        /// <summary>
        /// Event handler for AuthorsReportButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthorsReportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenAuthorsReport();
        }

        /// <summary>
        /// Shows the AuthorsReport window.  If window is already opened, bring to focus.
        /// Otherwise, opens a new window.
        /// </summary>
        private void OpenAuthorsReport()
        {
            //Check if window is already opened
            foreach (Window w in OwnedWindows)
            {
                if (w is AuthorsReport)
                {
                    w.Focus();
                    return;
                }
            }

            //Open and show new window
            AuthorsReport authorsReport = new AuthorsReport(controller);
            authorsReport.Owner = this;
            authorsReport.Show();
        }

        /// <summary>
        /// Event handler for OverdueBooksReportButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverdueBooksReportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenOverdueReport();
        }

        /// <summary>
        /// Shows OverdueReport Window.  If window already exists, bring to focus.
        /// Otherwise, open a new window.
        /// </summary>
        private void OpenOverdueReport()
        {
            //check if window is already opened
            foreach (Window w in OwnedWindows)
            {
                if (w is OverdueReport)
                {
                    w.Focus();
                    return;
                }
            }

            //Open and show new window
            OverdueReport overdueReport = new OverdueReport(controller);
            overdueReport.Owner = this;
            overdueReport.Show();
        }

        #endregion Reports

        #region Add Books

        /// <summary>
        /// Event handler for AddBook click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            AddNewBooks();
        }

        /// <summary>
        /// Opens an AddBookDialog window
        /// </summary>
        private void AddNewBooks()
        {
            AddBookDialog addBookDialog = new AddBookDialog(controller);
            addBookDialog.ShowDialog();
        }

        #endregion Add Books

        #region Check Out / In

        /// <summary>
        /// Event handler for CheckInButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckInButon_Click(object sender, RoutedEventArgs e)
        {
            OpenCheckInWindow();
        }

        /// <summary>
        /// Opens a new CheckInWindow dialog
        /// </summary>
        private void OpenCheckInWindow()
        {
            CheckInWindow ciw = new CheckInWindow(controller);
            ciw.ShowDialog();
        }

        /// <summary>
        /// Event handler for CheckOutButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCheckOutWindow();
        }

        /// <summary>
        /// Opens a new CheckOutWindow dialog
        /// </summary>
        private void OpenCheckOutWindow()
        {
            CheckOutWindow cow = new CheckOutWindow(controller);
            cow.ShowDialog();
        }

        #endregion Check Out / In

    }
}
