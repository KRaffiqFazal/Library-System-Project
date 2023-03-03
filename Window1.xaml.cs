using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Library_System
{
    public partial class Window1 : Window
    {
        Globals globalValues;
        XmlController xmlC = new XmlController();
        public Window1(Globals globals)
        {
            globalValues = globals;
            InitializeComponent();
            txtblkLoginError.Visibility = Visibility.Hidden;
            txtblkLoginErrorOmission.Visibility = Visibility.Hidden;
        }
        public void SwitchScreen()
        { 
            //load next screen
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (pswdbxPassword.Password.Equals(""))
            {
                txtblkLoginErrorOmission.Visibility = Visibility.Visible;
            }
            else if (xmlC.Exists(pswdbxPassword.Password))
            {
                globalValues.currentUser = new User(pswdbxPassword.Password);
                globalValues.currentUser.borrowedBooks = xmlC.GetBorrowedBooks(globalValues.currentUser);
                SwitchScreen();
            }
            else
            {
                txtblkLoginError.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                txtblkLoginError.Visibility = Visibility.Hidden;
                txtblkLoginErrorOmission.Visibility = Visibility.Hidden;



            }
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
    }
}
