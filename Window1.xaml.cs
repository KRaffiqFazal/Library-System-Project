﻿using System;
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
        public async void SwitchScreen()
        {
            MemberPage win = new MemberPage(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (pswdbxPassword.Password.Equals(""))
            {
                txtblkLoginErrorOmission.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                txtblkLoginErrorOmission.Visibility = Visibility.Hidden;
            }
            else if (xmlC.Exists(pswdbxPassword.Password))
            {
                globalValues.currentUser = new User(pswdbxPassword.Password);
                String[] temp = xmlC.GetInfo(pswdbxPassword.Password);

                globalValues.currentUser.name = temp[0];
                globalValues.currentUser.phoneNumber = temp[1];
                globalValues.currentUser.email = temp[2];
                globalValues.currentUser.borrowedBooks = xmlC.GetBorrowedBooks(globalValues.currentUser.userID);
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
