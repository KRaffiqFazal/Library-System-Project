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
    public partial class Borrow : Window
    {
        Globals globalValues;
        public Borrow(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            lblError.Content = "";
            UserCheck();
        }
        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private void UserCheck() 
        {
            if (globalValues.currentUser.borrowedBooks.Count >= 6)
            {
                lblError.Content = "Maximum books limit reached, please return your books before borrowing more.";
                btnBorrow.IsEnabled = false;
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

        private void txtbxToBorrow_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxToBorrow.Text))
            {
                txtbxToBorrow.Visibility = Visibility.Collapsed;
                txtbxWatermarked.Visibility = Visibility.Visible;
            }
        }

        private void txtbxWatermarked_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarked.Visibility = Visibility.Collapsed;
            txtbxToBorrow.Visibility = Visibility.Visible;
            txtbxToBorrow.Focus();
        }

        private async void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxToBorrow.Text.Equals(""))
            {
                lblError.Content = "Error: No input detected.";
                await Task.Delay(3000);
            }
            else if (globalValues.xmlC.GetAllBooks().FindIndex(book => book.id == txtbxToBorrow.Text) != -1) //if the entered id exists in the database, proceeds
            {
                List<Book> books = globalValues.xmlC.GetAllBooks();
                int index = books.FindIndex(book => book.id == txtbxToBorrow.Text);
                Book toBorrow = books[index];
                if (toBorrow.dueDate == DateTime.MinValue) //can be borrowed
                {
                    toBorrow.dueDate = DateTime.Now.AddDays(30); //due in a month
                    txtbxToBorrow.Text = "";
                    globalValues.xmlC.UpdateRecord(toBorrow);
                    globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                    lblError.Content = "Book Borrowed, happy reading!";
                    await Task.Delay(3000);
                    lblError.Content = "";

                }
                else
                {
                    lblError.Content = "Please report to a librarian with this book immediately."; //the book is in the library whilst it has been borrowed, librarian needs to report it.
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
