using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for MemberPage.xaml
    /// </summary>
    public partial class MemberPage : Window
    {
        Globals globalValues;
        bool mainPage = true;
        public MemberPage(Globals global)
        {
            InitializeComponent();
            globalValues = global;
            txtblkLogoutMessage.Visibility = Visibility.Hidden;
            txtblkTitle.Text = "Welcome: " + globalValues.currentUser.name;

            if (globalValues.currentUser.userType.Equals("member"))
            {
                btnNextBack.Visibility = Visibility.Hidden; //members can't see additional functionality
            }
            LoadPage1();
        }

        private async void picLogout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            txtblkLogoutMessage.Margin = new Thickness(0, 0, 0, 0);
            txtblkLogoutMessage.Visibility = Visibility.Visible;
            globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " Logged out.");
            globalValues = null;
            await Task.Delay(2000);
            MainWindow window = new MainWindow(null);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private void btnNextBack_Click(object sender, RoutedEventArgs e)
        {
            if (mainPage)
            {
                if (globalValues.currentUser.userType.Equals("librarian"))
                {
                    LoadPage2Librarian();
                }
                else
                {
                    LoadPage2Admin();
                }
                mainPage = false;
            }
            else
            {
                LoadPage1();
                mainPage = true;
            }
        }

        private void LoadPage1()
        {
            if (!globalValues.currentUser.userType.Equals("member"))
            {
                picManageBooks.Visibility = Visibility.Visible;
                picSearchForBooks.Visibility = Visibility.Hidden;
            }
            else
            {
                picSearchForBooks.Visibility = Visibility.Visible;
                picManageBooks.Visibility = Visibility.Hidden;
            }
            //make elements visible
            picBorrowBooks.Visibility = Visibility.Visible;
            picReturn.Visibility = Visibility.Visible;
            picProfile.Visibility = Visibility.Visible;

            //hide unused elements
            picManageMembers.Visibility = Visibility.Hidden;
            picManageUsers.Visibility = Visibility.Hidden;
            picLogErrors.Visibility = Visibility.Hidden;
            picErrors.Visibility = Visibility.Hidden;
            picReports.Visibility = Visibility.Hidden;

            btnNextBack.Content = "Next"; //change content of button
        }

        private void LoadPage2Librarian()
        {
            picManageMembers.Visibility = Visibility.Visible;
            picLogErrors.Visibility = Visibility.Visible;
            picReports.Visibility = Visibility.Visible;
            

            picBorrowBooks.Visibility = Visibility.Hidden;
            picReturn.Visibility = Visibility.Hidden;
            picProfile.Visibility = Visibility.Hidden;
            picErrors.Visibility = Visibility.Hidden;
            picSearchForBooks.Visibility = Visibility.Hidden;
            picManageUsers.Visibility = Visibility.Hidden;
            picManageBooks.Visibility = Visibility.Hidden;
            picErrors.Visibility = Visibility.Hidden;

            btnNextBack.Content = "Back";
        }

        private void LoadPage2Admin()
        {
            picManageUsers.Visibility = Visibility.Visible;
            picErrors.Visibility = Visibility.Visible;
            picReports.Visibility = Visibility.Visible;

            picBorrowBooks.Visibility = Visibility.Hidden;
            picReturn.Visibility = Visibility.Hidden;
            picProfile.Visibility = Visibility.Hidden;
            picLogErrors.Visibility = Visibility.Hidden;
            picManageMembers.Visibility = Visibility.Hidden;
            picManageBooks.Visibility = Visibility.Hidden;
            picSearchForBooks.Visibility = Visibility.Hidden;

            btnNextBack.Content = "Back";
        }

        private async void picProfile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Profile win = new Profile(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private async void picSearchForBooks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchBooks win = new SearchBooks(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private async void picLogErrors_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ErrorLogger win = new ErrorLogger(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private async void picBorrowBooks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Borrow win = new Borrow(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private async void picReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Return win = new Return(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private void picManageMembers_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewUsers();
        }

        private void picManageUsers_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewUsers();
        }
        private async void ViewUsers()
        {
            SearchUsers win = new SearchUsers(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private async void picReports_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Report win = new Report(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }
    }
}
