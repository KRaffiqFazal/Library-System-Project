﻿using System.Threading.Tasks;
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
                LoadPage1();
            }
        }

        private async void picLogout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            globalValues = null;
            txtblkLogoutMessage.Margin = new Thickness(0, 0, 0, 0);
            txtblkLogoutMessage.Visibility = Visibility.Visible;
            await Task.Delay(2000);
            MainWindow window = new MainWindow();
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
            picSearchForBooks.Margin = new Thickness(221, 430, 0, 0); //move new elements to correct position
            picBorrowBooks.Margin = new Thickness(608, 430, 0, 0);
            picReturn.Margin = new Thickness(995, 430, 0, 0);
            picProfile.Margin = new Thickness(1382, 430, 0, 0);

            picSearchForBooks.Visibility = Visibility.Visible; //make elements visible
            picBorrowBooks.Visibility = Visibility.Visible;
            picReturn.Visibility = Visibility.Visible;
            picProfile.Visibility = Visibility.Visible;

            picManageBooks.Visibility = Visibility.Hidden; //hide unused elements
            picManageMembers.Visibility = Visibility.Hidden;
            picManageUsers.Visibility = Visibility.Hidden;
            picLogErrors.Visibility = Visibility.Hidden;
            picErrors.Visibility = Visibility.Hidden;

            btnNextBack.Content = "Next"; //change content of button
        }

        private void LoadPage2Librarian()
        {
            picManageBooks.Margin = new Thickness(221, 430, 0, 0);
            picManageMembers.Margin = new Thickness(608, 430, 0, 0);
            picLogErrors.Margin = new Thickness(1382, 430, 0, 0);

            picManageBooks.Visibility = Visibility.Visible;
            picManageMembers.Visibility = Visibility.Visible;
            picLogErrors.Visibility = Visibility.Visible;

            picSearchForBooks.Visibility = Visibility.Hidden;
            picBorrowBooks.Visibility = Visibility.Hidden;
            picReturn.Visibility = Visibility.Hidden;
            picProfile.Visibility = Visibility.Hidden;
            picErrors.Visibility = Visibility.Hidden;
            picManageUsers.Visibility = Visibility.Hidden;

            btnNextBack.Content = "Back";
        }

        private void LoadPage2Admin()
        {
            picManageBooks.Margin = new Thickness(221, 430, 0, 0);
            picManageUsers.Margin = new Thickness(608, 430, 0, 0);
            picErrors.Margin = new Thickness(1382, 430, 0, 0);

            picManageBooks.Visibility = Visibility.Visible;
            picManageUsers.Visibility = Visibility.Visible;
            picErrors.Visibility = Visibility.Visible;

            picSearchForBooks.Visibility = Visibility.Hidden;
            picBorrowBooks.Visibility = Visibility.Hidden;
            picReturn.Visibility = Visibility.Hidden;
            picProfile.Visibility = Visibility.Hidden;
            picLogErrors.Visibility = Visibility.Hidden;
            picManageMembers.Visibility = Visibility.Hidden;

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
    }
}
