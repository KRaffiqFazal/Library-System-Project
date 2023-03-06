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
using System.Xml;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        Globals globalValues;
        public Profile(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            lblID.Content = globalValues.currentUser.userID;
            txtbxName.Text = globalValues.currentUser.name;
            txtbxMobile.Text = globalValues.currentUser.phoneNumber;
            txtbxEmail.Text = globalValues.currentUser.email;
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
            Home();
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
    }
}
