using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Borrow.xaml
    /// </summary>
    public partial class Borrow : Window
    {
        private Globals globalValues;

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
                lblError.Foreground = Brushes.Red;
                lblError.Content = "Error: No input detected.";
                await Task.Delay(3000);
            }
            else if (globalValues.xmlC.BookCompiler().FindIndex(book => book.id == txtbxToBorrow.Text) != -1) //if the entered id exists in the database, proceeds
            {
                List<Book> books = globalValues.xmlC.BookCompiler();
                Book toBorrow = books.Find(book => book.id == txtbxToBorrow.Text);
                if (toBorrow.dueDate == DateTime.MinValue) //can be borrowed
                {
                    toBorrow.dueDate = DateTime.Now.AddMonths(1); //due in a month
                    txtbxToBorrow.Text = "";
                    globalValues.xmlC.UpdateRecord(toBorrow, true);
                    globalValues.currentUser.borrowedBooks.Add(toBorrow);
                    globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                    globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " borrowed " + toBorrow.id);
                    globalValues.BorrowedLog(toBorrow, globalValues.currentUser);
                    lblError.Foreground = Brushes.Black;
                    lblError.Content = "Book Borrowed, happy reading!";
                    await Task.Delay(3000);
                    lblError.Content = "";
                }
                else //needs to check if the borrowed book belongs to a user or is lost
                {
                    if (globalValues.currentUser.borrowedBooks.FindIndex(book => book.id == txtbxToBorrow.Text) != -1) //belongs to a user
                    {
                        lblError.Foreground = Brushes.Red;
                        lblError.Content = "This book has already been borrowed.";
                        await Task.Delay(3000);
                        lblError.Content = "";
                    }
                    else //borrowed book forgotten in library
                    {
                        lblError.Foreground = Brushes.Red;
                        lblError.Content = "Please report this book to a librarian immediately.";
                        globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " tried returning " + txtbxToBorrow.Text + ", this is not borrowed by them.");
                    }
                }
            }
            else
            {
                lblError.Foreground = Brushes.Red;
                lblError.Content = "Error: ID not found, please enter a valid book ID.";
                await Task.Delay(3000);
                lblError.Content = "";
            }
        }
        /// <summary>
        /// Displays borrowed book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbxToBorrow_TextChanged(object sender, TextChangedEventArgs e)
        {
            String id = txtbxToBorrow.Text;
            if (globalValues.xmlC.BookCompiler().FindIndex(book => book.id == id) != -1) //exists
            {
                lblError.Foreground = Brushes.Black;
                lblError.Content = globalValues.xmlC.BookCompiler().Find(book => book.id == id).title;
            }
            else
            {
                lblError.Content = "";
            }
        }
    }
}