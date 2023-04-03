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
        /// <summary>
        /// Watermarked text box (1/2)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbxWatermarkedText_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarkedText.Visibility = Visibility.Collapsed;
            pswdbxPassword.Visibility = Visibility.Visible;
            pswdbxPassword.Focus();
        }
        /// <summary>
        /// Watermarked text (2/2)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pswdbxTerminalPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pswdbxPassword.Password))
            {
                pswdbxPassword.Visibility = Visibility.Collapsed;
                txtbxWatermarkedText.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Use enter key to login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pswdbxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }
        private async void Login()
        {
            //nothing entered
            if (pswdbxPassword.Password.Equals(""))
            {
                txtblkLoginErrorOmission.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                txtblkLoginErrorOmission.Visibility = Visibility.Hidden;
            }
            //checks if the entered id exists, then sets it as the current user
            else if (globalValues.xmlC.Exists(pswdbxPassword.Password))
            {
                globalValues.currentUser = new User(pswdbxPassword.Password);
                globalValues.currentUser = globalValues.xmlC.CreateUser(pswdbxPassword.Password);
                globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " Logged in.");
                List<User> sendNotifs = globalValues.xmlC.CancelReservations();
                //checks if any users need to notified that a reservation has been cancelled
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
            //wrong details
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
        /// <summary>
        /// only allows numbers to be entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pswdbxPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}