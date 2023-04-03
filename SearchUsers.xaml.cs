using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Library_System
{
    public partial class SearchUsers : Window
    {
        private Globals globalValues;

        public SearchUsers(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            txtbxNewNotification.MaxLength = 512;
            Hide1();
            CreateScreen(globalValues.xmlC.GetAllUsers(globalValues.currentUser));
            txtbxMobile.MaxLength = 11;
        }
        /// <summary>
        /// Displays a list of users that is parsed into function with user control, shows a table
        /// </summary>
        /// <param name="toDisplay"></param>
        private void CreateScreen(List<User> toDisplay)
        {
            stackPanel.Children.Clear();
            UserLine temp;
            String[] info;
            toDisplay = toDisplay.OrderByDescending(user => user.fine).ToList();
            foreach (User user in toDisplay)
            {
                temp = new UserLine();
                info = new String[5];
                info[0] = user.userID;
                info[1] = user.name;
                info[2] = user.email;
                info[3] = user.phoneNumber;
                info[4] = string.Format("{0:C}", user.fine);
                temp.UserInfo = info;
                temp.Cursor = Cursors.Hand;
                temp.MouseLeftButtonDown += OnMouseLeftButtonDown;
                stackPanel.Children.Add(temp);
            }
        }
        /// <summary>
        /// Shows the overlay for update
        /// </summary>
        private void Show1()
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
            btnSaveFocus.Visibility = Visibility.Visible;
            btnFineRemove.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnMore.Visibility = Visibility.Visible;
            lblError.Content = "";
        }
        /// <summary>
        /// reveals the update screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            UserLine temp = (UserLine)sender;
            String[] info = temp.UserInfo;
            lblUserIdFocusBox.Content = info[0];
            txtbxName.Text = info[1];
            txtbxMobile.Text = info[3];
            txtbxEmail.Text = info[2];
            lblFineFocusBox.Content = info[4];
            Show1();
            btnSaveFocus.Content = "Save";
        }
        /// <summary>
        /// hides overlay for librarian
        /// </summary>
        private void Hide1()
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
            btnSaveFocus.Visibility = Visibility.Hidden;
            btnFineRemove.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            txtblkDeleteConfirm.Visibility = Visibility.Hidden;
            btnDeleteConfirm.Visibility = Visibility.Hidden;
            btnDeleteConfirmCancel.Visibility = Visibility.Hidden;
            btnMore.Visibility = Visibility.Hidden;
            CloseFocus2.Visibility = Visibility.Hidden;
            lblErrorCharacterCounter.Visibility = Visibility.Hidden;
            txtbxNewNotification.Visibility = Visibility.Hidden;
            scrlvwrNotifsBorrowed.Visibility = Visibility.Hidden;
            btnAddNotification.Visibility = Visibility.Hidden;
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
        /// <summary>
        /// Checks which filters are active and displays results that match
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                else if (user.name.ToLower().Contains(txtbxSearch.Text.ToLower()) && chkbxName.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (user.email.ToLower().Contains(txtbxSearch.Text.ToLower()) && chkbxEmail.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (user.phoneNumber.Contains(txtbxSearch.Text) && chkbxPhone.IsChecked == true)
                {
                    updatedUsers.Add(user);
                }
                else if (string.Format("{0:C}", user.fine).Contains(txtbxSearch.Text))
                {
                    updatedUsers.Add(user);
                }
            }
            CreateScreen(updatedUsers);
        }
        /// <summary>
        /// Close detailed user screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseFocus_Click(object sender, RoutedEventArgs e)
        {
            CreateScreen(globalValues.xmlC.GetAllUsers(globalValues.currentUser));
            Hide1();
            txtbxName.Text = "";
            txtbxMobile.Text = "";
            txtbxEmail.Text = "";
            lblUserIdFocusBox.Content = "";
            rdbtnLibrarian.IsChecked = false;
            rdbtnMember.IsChecked = false;
            rdbtnAdmin.IsChecked = false;
        }
        /// <summary>
        /// Saves changed details to specific users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSaveFocus_Click(object sender, RoutedEventArgs e)
        {
            if (btnSaveFocus.Content.ToString().Equals("Save"))
            {
                List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
                User toUpdate = allUsers.Find(user => user.userID == lblUserIdFocusBox.Content.ToString());
                Trace.WriteLine(toUpdate.name);
                toUpdate.name = txtbxName.Text;
                toUpdate.phoneNumber = txtbxMobile.Text;
                toUpdate.email = txtbxEmail.Text;
                globalValues.xmlC.UpdateUserRecord(toUpdate);
                lblError.Content = "User Information Updated.";
            }
            else
            {
                Regex regex;
                bool pass = true;
                //checks if details are valid with regex, if not, does not save
                regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!regex.IsMatch(txtbxEmail.Text))
                {
                    Error("please enter a valid email address.");
                    pass = false;
                }
                regex = new Regex("^0[0-9]{9,10}$");
                if (!regex.IsMatch(txtbxMobile.Text)) //standard UK phone number
                {
                    Error("please enter a valid UK phone number starting with 0.");
                    pass = false;
                }
                regex = new Regex(@"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)");
                if (!regex.IsMatch(txtbxName.Text))
                {
                    Error("please enter full name.");
                    pass = false;
                }
                if (lblUserIdFocusBox.Content.ToString().Equals(""))
                {
                    Error("please select a user type");
                    pass = false;
                }
                if (pass)
                {
                    //saves details to a new user record
                    User newUser = new User(lblUserIdFocusBox.Content.ToString());
                    newUser.name = txtbxName.Text;
                    newUser.phoneNumber = txtbxMobile.Text;
                    newUser.email = txtbxEmail.Text;
                    globalValues.xmlC.AddUserRecord(newUser);
                    lblError.Foreground = Brushes.Black;
                    lblError.Content = "User added successfully!";
                    rdbtnMember.IsChecked = false; //unchecks buttons and clears field
                    rdbtnLibrarian.IsChecked = false;
                    rdbtnAdmin.IsChecked = false;
                    lblUserIdFocusBox.Content = "";
                    txtbxName.Text = "";
                    txtbxMobile.Text = "";
                    txtbxEmail.Text = "";
                    await Task.Delay(10000);
                    lblError.Content = "";
                }
            }
        }
        /// <summary>
        /// Displays an error message explaining why save not possible
        /// </summary>
        /// <param name="msg"></param>
        private async void Error(String msg)
        {
            lblError.Foreground = Brushes.Red;
            lblError.Content = "Save Unsuccessful: " + msg;
            await Task.Delay(10000);
            lblError.Content = "";
        }
        /// <summary>
        /// Sets fine to 0 if it has been paid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFineRemove_Click(object sender, RoutedEventArgs e)
        {
            List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
            User toUpdate = allUsers.Find(user => user.userID == lblUserIdFocusBox.Content.ToString());
            toUpdate.fine = 0;
            globalValues.xmlC.UpdateUserRecord(toUpdate);
            lblFineFocusBox.Content = "£0.00";
            globalValues.SendNotifications(toUpdate);
        }
        /// <summary>
        /// Reveals fields that allow a user to be added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            Show1();
            btnFineRemove.Visibility = Visibility.Hidden;
            btnSaveFocus.Content = "Add";
            lblFineFocus.Visibility = Visibility.Hidden;
            lblFineFocusBox.Visibility = Visibility.Hidden;
            lblUserTypeFocus.Visibility = Visibility.Visible;
            btnMore.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            rdbtnMember.Visibility = Visibility.Visible;
            if (globalValues.currentUser.userType.Equals("admin"))
            {
                rdbtnLibrarian.Visibility = Visibility.Visible;
                rdbtnAdmin.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Dynamically creates a new member ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbtnMember_Checked(object sender, RoutedEventArgs e) //create unique member id
        {
            List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
            String[] today = DateTime.Now.ToShortDateString().Split('/');
            String tempID = "1" + today[0] + today[1];
            int i = 0;
            while (allUsers.FindIndex(user => user.userID == tempID + i.ToString()) != -1) //finds available id
            {
                i++;
            }
            lblUserIdFocusBox.Content = tempID + i.ToString();
        }
        /// <summary>
        /// Dynamically creates a new librarian ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbtnLibrarian_Checked(object sender, RoutedEventArgs e)
        {
            if (btnSaveFocus.Content.ToString().Equals("Add")) //create unique member id
            {
                List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
                String[] today = DateTime.Now.ToShortDateString().Split('/');
                String tempID = "2" + today[0] + today[1];
                int i = 0;
                while (allUsers.FindIndex(user => user.userID == tempID + i.ToString()) != -1) //finds available id
                {
                    i++;
                }
                lblUserIdFocusBox.Content = tempID + i.ToString();
            }
        }
        /// <summary>
        /// Dynamically creates a new admin ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbtnAdmin_Checked(object sender, RoutedEventArgs e)
        {
            if (btnSaveFocus.Content.ToString().Equals("Add")) //create unique member id
            {
                List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.currentUser);
                String[] today = DateTime.Now.ToShortDateString().Split('/');
                String tempID = "3" + today[0] + today[1];
                int i = 0;
                while (allUsers.FindIndex(user => user.userID == tempID + i.ToString()) != -1) //finds available id
                {
                    i++;
                }
                lblUserIdFocusBox.Content = tempID + i.ToString();
            }
        }

        private void txtbxMobile_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Asks for confirmation to delete a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            txtblkDeleteConfirm.Visibility = Visibility.Visible;
            btnDeleteConfirm.Visibility = Visibility.Visible;
            btnDeleteConfirmCancel.Visibility = Visibility.Visible;
            txtblkDeleteConfirm.Text = "Are you sure that you want to delete this user?";

            System.Media.SystemSounds.Beep.Play();
        }
        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDeleteConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (globalValues.currentUser.userID.Equals(lblUserIdFocusBox.Content.ToString()))
            {
                globalValues = null;
                MainWindow window = new MainWindow(null);
                window.Show();
                await Task.Delay(250);
                Close();
            }
            globalValues.xmlC.DeleteUserRecord(lblUserIdFocusBox.Content.ToString());
            Hide1();
        }

        private void btnDeleteConfirmCancel_Click(object sender, RoutedEventArgs e)
        {
            txtblkDeleteConfirm.Visibility = Visibility.Hidden;
            btnDeleteConfirm.Visibility = Visibility.Hidden;
            btnDeleteConfirmCancel.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Shows additional user information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            txtblkDeleteConfirm.Visibility = Visibility.Visible;
            txtblkDeleteConfirm.Text = "";
            CloseFocus2.Visibility = Visibility.Visible;
            btnAddNotification.Visibility = Visibility.Visible;
            btnDeleteConfirmCancel.Visibility = Visibility.Hidden;
            btnDeleteConfirm.Visibility = Visibility.Hidden;
            scrlvwrNotifsBorrowed.Visibility = Visibility.Visible;
            lblErrorCharacterCounter.Visibility = Visibility.Visible;
            txtbxNewNotification.Visibility = Visibility.Visible;

            UpdateNotifs();
        }
        /// <summary>
        /// Displays all borrowed books and user notifications
        /// </summary>
        private void UpdateNotifs()
        {
            txtblkNotifsBorrowed.Text = "Borrowed Books:\n\n";
            User user = new User(lblUserIdFocusBox.Content.ToString());
            user = globalValues.xmlC.CreateUser(lblUserIdFocusBox.Content.ToString());
            foreach (Book book in user.borrowedBooks)
            {
                txtblkNotifsBorrowed.Text += book.id + " " + book.title + "\n\n";
            }
            txtblkNotifsBorrowed.Text += "Notifications:\n\n";
            foreach (String notif in user.notifications)
            {
                txtblkNotifsBorrowed.Text += notif + "\n\n";
            }
        }
        /// <summary>
        /// Adds entered notification to user and informs them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAddNotification_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxNewNotification.Text.Equals(""))
            {
                lblErrorCharacterCounter.Content = "Please enter a notification";
                await Task.Delay(2000);
                if (lblErrorCharacterCounter.Content.ToString().Equals("Added notification."))
                {
                    lblErrorCharacterCounter.Content = "Characters left: 512";
                }
                return;
            }
            String toSave = txtbxNewNotification.Text.Replace("}", ")"); //changes closed curly brackets to normal brackets to prevent errrors when reading from xml file
            toSave = toSave.Replace("{", "(");
            toSave = DateTime.Now.ToShortDateString() + " " + globalValues.currentUser.userType.ToUpper() + " has said: " + toSave;
            User updateUser = new User(lblUserIdFocusBox.Content.ToString());
            updateUser = globalValues.xmlC.CreateUser(lblUserIdFocusBox.Content.ToString());
            updateUser.notifications.Add(toSave);
            globalValues.xmlC.UpdateUserRecord(updateUser);
            globalValues.SendNotifications(updateUser);
            String oldVal = lblErrorCharacterCounter.Content.ToString();
            lblErrorCharacterCounter.Content = "Added notification.";
            UpdateNotifs();
            txtbxNewNotification.Text = "";
            await Task.Delay(2000);
            if (lblErrorCharacterCounter.Content.ToString().Equals("Added notification."))
            {
                lblErrorCharacterCounter.Content = oldVal;
            }
        }

        private void txtbxNewNotification_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorCharacterCounter.Content = "Characters left: " + (512 - txtbxNewNotification.Text.Length);
        }

        private void CloseFocus2_Click(object sender, RoutedEventArgs e)
        {
            CloseFocus2.Visibility = Visibility.Hidden;
            txtblkDeleteConfirm.Visibility = Visibility.Hidden;
            scrlvwrNotifsBorrowed.Visibility = Visibility.Hidden;
            lblErrorCharacterCounter.Visibility = Visibility.Hidden;
            txtbxNewNotification.Visibility = Visibility.Hidden;
            btnAddNotification.Visibility = Visibility.Hidden;
        }
    }
}