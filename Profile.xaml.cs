using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        private Globals globalValues;
        private String oldName;
        private String oldPhone;
        private String oldEmail;
        private bool page1 = true;

        public Profile(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            OnStartUp();
        }
        /// <summary>
        /// sets user information in fields
        /// </summary>
        private void OnStartUp()
        {
            lblID.Content = globalValues.currentUser.userID;
            txtbxName.Text = globalValues.currentUser.name;
            txtbxMobile.Text = globalValues.currentUser.phoneNumber;
            txtbxEmail.Text = globalValues.currentUser.email;
            List<Book> allBooks = globalValues.xmlC.BookCompiler();
            if (!globalValues.currentUser.reserved.Equals(""))
            {
                lblReserved.Content = allBooks.Find(x => x.id == globalValues.currentUser.reserved).isbn;
                btnCancelReservation.IsEnabled = true;
            }
            else
            {
                lblReserved.Content = "";
                btnCancelReservation.IsEnabled = false;
            }
            //remembers initial information if user wants to undo
            oldName = globalValues.currentUser.name;
            oldPhone = globalValues.currentUser.phoneNumber;
            oldEmail = globalValues.currentUser.email;

            rctnglCover.Visibility = Visibility.Hidden;
            scrlvwrNotifications.Visibility = Visibility.Hidden;
            btnClearNotif.Visibility = Visibility.Hidden;
        }

        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private void picBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page1)
            {
                Home();
            }
            else
            {
                rctnglCover.Visibility = Visibility.Hidden;
                scrlvwrNotifications.Visibility = Visibility.Hidden;
                btnClearNotif.Visibility = Visibility.Hidden;

                btnReset.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                picNotifications.Visibility = Visibility.Visible;
                btnCancelReservation.Visibility = Visibility.Visible;

                page1 = true;
            }
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //(Hellnar, 2010)
            Regex regex;
            bool pass = true; //if no errors generated information is saved
            //(Sergey, 2011)
            regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(txtbxEmail.Text))
            {
                Error("please enter a valid email address.");
                pass = false;
            }
            regex = new Regex("^0[0-9]{9,10}$"); //starts with 0, can be 10 or 11 digits in total
            if (!regex.IsMatch(txtbxMobile.Text)) //standard UK phone number
            {
                Error("please enter a valid UK phone number starting with 0.");
                pass = false;
            }
            regex = new Regex(@"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)"); //check for full name
            if (!regex.IsMatch(txtbxName.Text))
            {
                Error("please enter full name.");
                pass = false;
            }
            if (pass)
            {
                globalValues.currentUser.name = txtbxName.Text;
                globalValues.currentUser.phoneNumber = txtbxMobile.Text;
                globalValues.currentUser.email = txtbxEmail.Text;
                globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                txtblkErrorMessage.Foreground = Brushes.Black;
                txtblkErrorMessage.Text = "Data saved successfully!";
                await Task.Delay(10000);
                txtblkErrorMessage.Text = "";
            }
        }

        private async void Error(String msg)
        {
            txtblkErrorMessage.Foreground = Brushes.Red;
            txtblkErrorMessage.Text = "Save Unsuccessful: " + msg;
            await Task.Delay(10000);
            txtblkErrorMessage.Text = "";
        }
        /// <summary>
        /// Uses initial details to undo any changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtbxName.Text = oldName;
            txtbxMobile.Text = oldPhone;
            txtbxEmail.Text = oldEmail;
            txtblkErrorMessage.Foreground = Brushes.Black;
            txtblkErrorMessage.Text = "Changes undone, remember to save!";
            await Task.Delay(4000);
            txtblkErrorMessage.Text = "";
        }

        /// <summary>
        /// Creates a cover that hides the previous screen and displays notifications
        /// </summary>
        private void picNotifications_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rctnglCover.Visibility = Visibility.Visible;
            scrlvwrNotifications.Visibility = Visibility.Visible;
            txtblkNotifications.Text = "";
            btnClearNotif.Visibility = Visibility.Visible;

            btnSave.Visibility = Visibility.Hidden;
            btnReset.Visibility = Visibility.Hidden;
            picNotifications.Visibility = Visibility.Hidden;
            btnCancelReservation.Visibility = Visibility.Hidden;

            LoadNotifs();

            page1 = false;
        }

        private void txtbxMobile_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Clears last notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearNotif_Click(object sender, RoutedEventArgs e)
        {
            if (globalValues.currentUser.notifications.Count == 0)
            {
                return;
            }
            globalValues.currentUser.notifications.RemoveAt(0);
            globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
            LoadNotifs();
        }
        /// <summary>
        /// Loads all notifications and any pending activity that the user has: fines, borrowed books
        /// </summary>
        private void LoadNotifs()
        {
            txtblkNotifications.Text = "";
            globalValues.currentUser = globalValues.xmlC.CreateUser(globalValues.currentUser.userID);
            if (globalValues.currentUser.notifications.Count == 0 && globalValues.currentUser.borrowedBooks.Count == 0 && globalValues.currentUser.fine == 0)
            {
                txtblkNotifications.Text = "Clear!"; //no borrowed books and no active notifications
            }
            if (globalValues.currentUser.fine != 0) //shows fines
            {
                txtblkNotifications.Text += "Fine due: " + string.Format("{0:C}", globalValues.currentUser.fine) + ", please see a librarian immediately.\n\n";
            }
            if (!(globalValues.currentUser.borrowedBooks.Count == 0)) //there are books that are currently being borrowed
            {
                List<Book> books = globalValues.currentUser.borrowedBooks;
                foreach (Book book in books)
                {
                    if (book.dueDate > DateTime.Now)
                    {
                        txtblkNotifications.Text += book.id + " " + book.title + " has been borrowed until " + book.dueDate.ToShortDateString() + ", due in " + Math.Floor((book.dueDate - DateTime.Now).TotalDays).ToString() + " days.\n\n";
                    }
                    else
                    {
                        txtblkNotifications.Text += book.id + " " + book.title + " has been borrowed until " + book.dueDate.ToShortDateString() + ", due IMMEDIATELY " + "\n\n";
                    }
                }
            }
            if (!(globalValues.currentUser.notifications.Count == 0))//there are notifications to be displayed
            {
                List<String> notifications = globalValues.currentUser.notifications;
                notifications.Reverse();
                foreach (String notif in notifications)
                {
                    txtblkNotifications.Text += notif + "\n\n";
                }
            }
        }
        /// <summary>
        /// Cancels current book reservation so that the user can reserve another book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelReservation_Click(object sender, RoutedEventArgs e)
        {
            String idToDelete;
            if (!globalValues.currentUser.reserved.Equals(""))
            {
                idToDelete = globalValues.currentUser.reserved;
                Book cancelReservation = globalValues.xmlC.BookCompiler().Find(x => x.id == idToDelete);
                cancelReservation.reserved = DateTime.MinValue;
                globalValues.xmlC.UpdateRecord(cancelReservation, true);
                globalValues.currentUser.reserved = "";
                lblReserved.Content = "";
                globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                btnCancelReservation.IsEnabled = false;
            }
        }
    }
}