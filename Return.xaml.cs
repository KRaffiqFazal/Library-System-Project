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

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Borrow.xaml
    /// </summary>
    public partial class Return : Window
    {
        Globals globalValues;
        public Return(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            lblError.Visibility = Visibility.Hidden;
        }
        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private void picBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private void txtbxToBorrow_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxToReturn.Text))
            {
                txtbxToReturn.Visibility = Visibility.Collapsed;
                txtbxWatermarked.Visibility = Visibility.Visible;
            }
        }

        private void txtbxWatermarked_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarked.Visibility = Visibility.Collapsed;
            txtbxToReturn.Visibility = Visibility.Visible;
            txtbxToReturn.Focus();
        }

        private async void btnReturn_Click(object sender, RoutedEventArgs e) //fix tomorrow
        {
            if (txtbxToReturn.Text.Equals(""))
            {
                lblError.Content = "Error: No input detected.";
                await Task.Delay(3000);
            }
            else if (globalValues.xmlC.GetAllBooks().FindIndex(book => book.id == txtbxToReturn.Text) != -1) //if the entered id exists in the database, proceeds
            {
                List<Book> books = globalValues.xmlC.GetAllBooks();
                int index = books.FindIndex(book => book.id == txtbxToReturn.Text);
                Book toReturn = books[index];
                if (toReturn.dueDate != DateTime.MinValue) //can be returned
                {
                    toReturn.dueDate = DateTime.MinValue; //due in a month
                    txtbxToReturn.Text = "";
                    globalValues.xmlC.UpdateRecord(toReturn);
                    globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                    lblError.Content = "Book returned!";
                    await Task.Delay(3000);
                    lblError.Content = "";

                }
                else
                {
                    lblError.Content = "Error: This book has not been borrowed.";
                }
            }
            else
            {
                lblError.Content = "Error: ID not found, please enter a valid book ID.";
                await Task.Delay(3000);
                lblError.Content = "";
            }
        }
    }
}
