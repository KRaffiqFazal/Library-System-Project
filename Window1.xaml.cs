using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Library_System
{
    public partial class Window1 : Window
    {
        private Globals globalValues;

        public Window1(Globals globals)
        {
            globalValues = globals;
            InitializeComponent();
            txtblkLoginError.Visibility = Visibility.Hidden;
            txtblkLoginErrorOmission.Visibility = Visibility.Hidden;
        }

        public async void SwitchScreen()
        {
            MemberPage win = new MemberPage(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void txtbxWatermarkedText_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarkedText.Visibility = Visibility.Collapsed;
            pswdbxPassword.Visibility = Visibility.Visible;
            pswdbxPassword.Focus();
        }

        private void pswdbxTerminalPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pswdbxPassword.Password))
            {
                pswdbxPassword.Visibility = Visibility.Collapsed;
                txtbxWatermarkedText.Visibility = Visibility.Visible;
            }
        }

        private void pswdbxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private async void Login()
        {
            if (pswdbxPassword.Password.Equals(""))
            {
                txtblkLoginErrorOmission.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                txtblkLoginErrorOmission.Visibility = Visibility.Hidden;
            }
            else if (globalValues.xmlC.Exists(pswdbxPassword.Password))
            {
                globalValues.currentUser = new User(pswdbxPassword.Password);
                globalValues.currentUser = globalValues.xmlC.CreateUser(pswdbxPassword.Password);
                globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " Logged in.");
                List<User> sendNotifs = globalValues.xmlC.CancelReservations();

                if (sendNotifs.Count != 0)
                {
                    foreach (User user in sendNotifs)
                    {
                        globalValues.SendNotifications(user);
                        globalValues.UpdateDetailedLog(user.userID + " was emailed regarding a cancelled reservation.");
                    }
                }
                SwitchScreen();
            }
            else
            {
                txtblkLoginError.Margin = new Thickness(0, 0, 0, 0);
                txtblkLoginError.Visibility = Visibility.Visible;
                pswdbxPassword.Password = "";
                await Task.Delay(3000);
                txtblkLoginError.Visibility = Visibility.Hidden;
                txtblkLoginErrorOmission.Visibility = Visibility.Hidden;
            }
        }

        private void pswdbxPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}