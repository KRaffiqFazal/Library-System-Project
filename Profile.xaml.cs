using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        Globals globalValues;
        String oldName;
        String oldPhone;
        String oldEmail;
        bool page1 = true;
        public Profile(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            lblID.Content = globalValues.currentUser.userID;
            txtbxName.Text = globalValues.currentUser.name;
            txtbxMobile.Text = globalValues.currentUser.phoneNumber;
            txtbxEmail.Text = globalValues.currentUser.email;

            oldName = globalValues.currentUser.name;
            oldPhone = globalValues.currentUser.phoneNumber;
            oldEmail = globalValues.currentUser.email;

            rctnglCover.Visibility = Visibility.Hidden;
            scrlvwrNotifications.Visibility = Visibility.Hidden;
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

                btnReset.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                picNotifications.Visibility = Visibility.Visible;

                page1 = true;
            }
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            String name = txtbxName.Text; //check data note to self
            String phoneNumber = txtbxMobile.Text;
            String email = txtbxEmail.Text;
            if (name[0] == ' ' || name[name.Length - 1] == ' ')
            {
                Error("please remove trailing and leading spaces before name.");
            }
            else if (phoneNumber[0] == ' ' || phoneNumber[phoneNumber.Length - 1] == ' ')
            {
                Error("please remove trailing and leading spaces before phone number.");
            }
            else if (email[0] == ' ' || email[email.Length - 1] == ' ')
            {
                Error("please remove trailing and leading spaces before email address.");
            }
            else if (!name.Contains(" "))
            {
                Error("please enter full name.");
            }
            else if (phoneNumber[0] != '0' || phoneNumber.Length != 11) //standard UK phone number
            {
                if (phoneNumber.Length != 10) //some phone numbers are 10 digits long
                {
                    Error("please enter a valid UK phone number starting with 0.");
                }
            }
            else if (!email.Contains("@"))
            {
                Error("please enter a valid email address.");
            }
            else
            {

                globalValues.currentUser.name = txtbxName.Text;
                globalValues.currentUser.phoneNumber = txtbxMobile.Text;
                globalValues.currentUser.email = txtbxEmail.Text;
                globalValues.xmlC.doc.Load(globalValues.xmlC.userPath);
                XmlNode userNode = globalValues.xmlC.UserType(globalValues.currentUser.userID);
                userNode.ChildNodes.Item(1).InnerText = globalValues.currentUser.name;
                userNode.ChildNodes.Item(2).InnerText = globalValues.currentUser.phoneNumber;
                userNode.ChildNodes.Item(3).InnerText = globalValues.currentUser.email;
                globalValues.xmlC.doc.Save(globalValues.xmlC.userPath);
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

        private void picNotifications_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rctnglCover.Margin = new Thickness(59, 437, 0, 0);
            rctnglCover.Visibility = Visibility.Visible;
            scrlvwrNotifications.Margin = new Thickness(85, 473, 0, 0);
            scrlvwrNotifications.Visibility = Visibility.Visible;
            txtblkNotifications.Text = "";

            btnSave.Visibility = Visibility.Hidden;
            btnReset.Visibility = Visibility.Hidden;
            picNotifications.Visibility = Visibility.Hidden;

            if (globalValues.currentUser.notifications.Count == 0 && globalValues.currentUser.borrowedBooks.Count == 0 && globalValues.currentUser.reserved.Equals(""))
            {
                txtblkNotifications.Text = "Clear!"; //no borrowed books and no active notifications
            }
            if (!(globalValues.currentUser.reserved.Equals(""))) //there is a book being reserved
            {
                txtblkNotifications.Text += globalValues.xmlC.BookCompiler()[globalValues.xmlC.BookCompiler().FindIndex(book => book.id == globalValues.currentUser.reserved)].title + " is being reserved.\n";
            }
            if (!(globalValues.currentUser.borrowedBooks.Count == 0)) // there are books that are currently being borrowed
            {
                List<Book> books = globalValues.currentUser.borrowedBooks;
                foreach (Book book in books)
                {
                    txtblkNotifications.Text += book.id + " " + book.title + " has been borrowed until " + book.dueDate.ToShortDateString() + "\n";
                }
            }
            if (!(globalValues.currentUser.notifications.Count == 0))//there are notifications to be displayed
            {
                List<String> notifications = globalValues.currentUser.notifications;
                foreach (String notif in notifications)
                {
                    txtblkNotifications.Text += notif + "\n";
                }
            }
            page1 = false;
        }
    }
}
