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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library_System //need to add the user control and update each value, kinda like the other one
{
    /// <summary>
    /// Interaction logic for SearchUsers.xaml
    /// </summary>
    public partial class SearchUsers : Window
    {
        Globals globalValues;
        public SearchUsers(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            Hide1();
            CreateScreen(globalValues.xmlC.GetAllUsers(globalValues.currentUser));
        }
        private void CreateScreen(List<User> toDisplay)
        { 
            stackPanel.Children.Clear();
            UserLine temp;
            String[] info;
            toDisplay = toDisplay.OrderByDescending(user => user.CalculateFine()).ToList();
            foreach (User user in toDisplay) 
            {
                temp = new UserLine();
                info = new String[5];
                info[0] = user.userID;
                info[1] = user.name;
                info[2] = user.email;
                info[3] = user.phoneNumber;
                info[4] = user.CalculateFineString();
                temp.UserInfo = info;
                temp.Cursor = Cursors.Hand;
                temp.MouseLeftButtonDown += OnMouseLeftButtonDown;
                stackPanel.Children.Add(temp);
            }
        }
        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e) //reveals the update screen
        {
            rctnglNewScreen.Visibility = Visibility.Visible;
            lblUserTypeFocus.Visibility = Visibility.Hidden;
            lblNameFocus.Visibility = Visibility.Visible;
            lblPhoneFocus.Visibility = Visibility.Visible;
            lblEmailFocus.Visibility = Visibility.Visible;
            lblUserIdFocus.Visibility = Visibility.Visible;
            lblFineFocus.Visibility = Visibility.Visible;
            lblFineFocusBox.Visibility = Visibility.Visible;
            lblUserIdFocusBox.Visibility = Visibility.Visible;
            txtbxName.Visibility = Visibility.Visible;
            txtbxMobile.Visibility = Visibility.Visible;
            txtbxEmail.Visibility = Visibility.Visible;
            rdbtnMember.Visibility = Visibility.Hidden;
            rdbtnLibrarian.Visibility = Visibility.Hidden;
            rdbtnAdmin.Visibility = Visibility.Hidden;
            btnCloseFocus.Visibility = Visibility.Visible;
            lblError.Visibility = Visibility.Visible;
            btnAddUser.Visibility = Visibility.Visible;
            btnFineRemove.Visibility = Visibility.Visible;
            
            btnAddUser.Content = "Save";

            UserLine temp = (UserLine)sender;
            String[] info = temp.UserInfo;
            lblUserIdFocusBox.Content = info[0];
            txtbxName.Text = info[1];
            txtbxMobile.Text = info[3];
            txtbxEmail.Text = info[2];
            lblFineFocusBox.Content = info[4];

        }
        private void Hide1() //hides the add/update/delete screen
        {
            rctnglNewScreen.Visibility = Visibility.Hidden;
            lblUserTypeFocus.Visibility = Visibility.Hidden;
            lblNameFocus.Visibility = Visibility.Hidden;
            lblPhoneFocus.Visibility = Visibility.Hidden;
            lblEmailFocus.Visibility = Visibility.Hidden;
            lblUserIdFocus.Visibility = Visibility.Hidden;
            lblFineFocus.Visibility = Visibility.Hidden;
            lblFineFocusBox.Visibility = Visibility.Hidden;
            lblUserIdFocusBox.Visibility = Visibility.Hidden;
            txtbxName.Visibility = Visibility.Hidden;
            txtbxMobile.Visibility = Visibility.Hidden;
            txtbxEmail.Visibility = Visibility.Hidden;
            rdbtnMember.Visibility = Visibility.Hidden;
            rdbtnLibrarian.Visibility = Visibility.Hidden;
            rdbtnAdmin.Visibility = Visibility.Hidden;
            btnCloseFocus.Visibility = Visibility.Hidden;
            lblError.Visibility = Visibility.Hidden;
            btnAddUser.Visibility = Visibility.Hidden;
            btnFineRemove.Visibility = Visibility.Hidden;
        }
        private void txtbxWatermarked_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarked.Visibility = Visibility.Collapsed;
            txtbxSearch.Visibility = Visibility.Visible;
            txtbxSearch.Focus();
        }

        private void txtbxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxSearch.Text))
            {
                txtbxSearch.Visibility = Visibility.Collapsed;
                txtbxWatermarked.Visibility = Visibility.Visible;
            }
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private void picBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }
        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<User> users = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
            List<User> updatedUsers = new List<User>();
            foreach (User user in users)
            {
                if (user.userID.Contains(txtbxSearch.Text) && chkbxID.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (user.name.Contains(txtbxSearch.Text) && chkbxName.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (user.email.Contains(txtbxSearch.Text) && chkbxEmail.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (user.phoneNumber.Contains(txtbxSearch.Text) && chkbxPhone.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (user.CalculateFineString().Contains(txtbxSearch.Text)) 
                {
                    updatedUsers.Add(user);
                }
            }
            CreateScreen(updatedUsers);
        }

        private void btnCloseFocus_Click(object sender, RoutedEventArgs e)
        {
            Hide1();
        }

        private void btnSaveFocus_Click(object sender, RoutedEventArgs e)
        {
            List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
            User toUpdate = allUsers.Find(user => user.userID == lblUserIdFocusBox.Content.ToString());
            toUpdate.name = txtbxName.Text;
            toUpdate.phoneNumber = txtbxMobile.Text;
            toUpdate.email = txtbxEmail.Text;
            globalValues.xmlC.UpdateUserRecord(toUpdate);
        }

        private void btnFineRemove_Click(object sender, RoutedEventArgs e)
        {
            List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
            User toUpdate = allUsers.Find(user => user.userID == lblUserIdFocusBox.Content.ToString());
            toUpdate.fine = 0;
            globalValues.xmlC.UpdateUserRecord(toUpdate); //need to add fine field in this method and in xml file and a way to load it in when signed in
            lblFineFocusBox.Content = "No Fine";
        }
    }
}
