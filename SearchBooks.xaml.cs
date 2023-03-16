using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for SearchBooks.xaml
    /// </summary>
    public partial class SearchBooks : Window
    {
        Globals globalValues;
        String[] filePaths;
        String[] availableImgs;
        String currentIsbn;
        public SearchBooks(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            FirstOpen();


        }
        private void FirstOpen()
        {
            filePaths = Directory.GetFiles(@"Books\", "*.png");
            availableImgs = new String[filePaths.Length];
            CreateScreen(globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler()));

            Hide1();
        }
        private void Hide1() //hides popups
        {
            picBookFocus.Visibility = Visibility.Hidden;
            rctnglFocus.Visibility = Visibility.Hidden;
            btnCloseFocus.Visibility = Visibility.Hidden;
            btnReserve.Visibility = Visibility.Hidden;
            txtblkBookInfoFocus.Visibility = Visibility.Hidden;
            scrlvwrFocus.Visibility = Visibility.Hidden;
        }
        private void CreateScreen(List<Book> books)
        {
            BookDisplay currentDisplayed;
            Trace.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            for (int i = 0; i < filePaths.Length; i++)
            {
                availableImgs[i] = filePaths[i].Substring(6, filePaths[i].Length - 10); //gets array of ids that have images
            }
            stackPanel.Children.Clear();
            foreach (Book book in books) //creates the elements in the scrollviewer
            {
                currentDisplayed = new BookDisplay();
                currentDisplayed.Padding = new Thickness(5);
                currentDisplayed.BookInfo = book.title + "\n\nAuthor: " + book.author + "\n\nPublisher: " + book.publisher + "\n\nEdition: " + book.edition;
                currentDisplayed.Cursor = Cursors.Hand;

                if (availableImgs.Contains(book.isbn))
                {
                    currentDisplayed.picBook.Source = new BitmapImage(new Uri(@"Books\" + book.isbn + ".png", UriKind.Relative));
                }
                else
                {
                    currentDisplayed.picBook.Source = new BitmapImage(new Uri(@"0.png", UriKind.Relative));
                }
                currentDisplayed.BookIsbn = book.isbn;
                currentDisplayed.MouseLeftButtonDown += OnMouseLeftButtonDown;
                stackPanel.Children.Add(currentDisplayed);
            }
        } //Mess around with this until it works
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) //work out why pictures don't show, change buttons so they don't have hover and have click hand cursor work out how reservations work, also change how availability works for borrowed books
        {
            rctnglFocus.Visibility = Visibility.Visible;
            BookDisplay temp = new BookDisplay();

            temp = (BookDisplay)sender;
            picBookFocus.Source = temp.picBook.Source;
            List<Book> bookList = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
            Book currentBook = bookList[bookList.FindIndex(book => book.isbn == temp.BookIsbn)];
            picBookFocus.Visibility = Visibility.Visible;
            txtblkBookInfoFocus.Text = currentBook.title + "\n\nAuthor: " + currentBook.author + "  Publisher: " + currentBook.publisher + "\n\nEdition: " + currentBook.edition
                + "  Year: " + currentBook.year + "\n\nCategory: " + String.Join(", ", currentBook.category) + "\n\nISBN: " + currentBook.isbn + "\n\n" + currentBook.description +
                "\n\nAvailable Copies: " + currentBook.availableCopies;


            txtblkBookInfoFocus.Visibility = Visibility.Visible;
            scrlvwrFocus.Visibility = Visibility.Visible;
            picBack.Visibility = Visibility.Visible;
            btnCloseFocus.Visibility = Visibility.Visible;

            if (txtblkBookInfoFocus.Text.Contains("Available Copies: 0"))
            {
                if (!globalValues.xmlC.ToReserve(currentBook).Equals("") && !globalValues.currentUser.reserved.Equals("") && globalValues.currentUser.borrowedBooks.FindIndex(book => book.isbn == currentBook.isbn) == -1) //a copy available that is not reserved, the user is not reserving a book and the user has not borrowed a copy of the book they wish to reserve
                {
                    currentIsbn = temp.BookIsbn;
                    btnReserve.Visibility = Visibility.Visible;
                }
                else if (!globalValues.currentUser.reserved.Equals(""))
                {
                    lblError.Content = "You have already reserved another book.";
                }
                else if (globalValues.currentUser.borrowedBooks.FindIndex(book => book.isbn == currentBook.isbn) == -1)
                {
                    lblError.Content = "You cannot reserve a copy of the same borrowed book.";
                }
                else
                {
                    lblError.Content = "All copies have been reserved";
                }

            }
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

        private void txtbxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxSearch.Text))
            {
                txtbxSearch.Visibility = Visibility.Collapsed;
                txtbxWatermarkedText.Visibility = Visibility.Visible;
            }
        }

        private void txtbxWatermarkedText_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarkedText.Visibility = Visibility.Collapsed;
            txtbxSearch.Visibility = Visibility.Visible;
            txtbxSearch.Focus();
        }

        private void btnCloseFocus_Click(object sender, RoutedEventArgs e)
        {
            Hide1();
        }

        private async void btnReserve_Click(object sender, RoutedEventArgs e) //button only seen if reservation possible
        {
            List<Book> booksAvailable = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
            Book currentBook = booksAvailable[booksAvailable.FindIndex(book => book.isbn == currentIsbn)];
            if (!globalValues.xmlC.ToReserve(currentBook).Equals("")) //checks if it has not been reserved whilst the page is open
            {
                globalValues.currentUser.reserved = globalValues.xmlC.ToReserve(currentBook);
                currentBook.reserved = true;
                globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                globalValues.xmlC.UpdateRecord(currentBook, true);
            }
            else
            {
                lblError.Content = "Error, reloading page";
                await Task.Delay(3000);
                SearchBooks win = new SearchBooks(globalValues);
                win.Show();
                await Task.Delay(250);
                Close();
                return;
            }
        }

        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Book> newList = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
            List<Book> updatedList = new List<Book>();
            foreach (Book item in newList)
            {
                if (chkbxTitle.IsChecked == true)
                {
                    if (item.title.ToLower().Contains(txtbxSearch.Text.ToLower()))
                    {
                        updatedList.Add(item);
                    }
                }
                else if (chkbxAuthor.IsChecked == true)
                {
                    if (item.author.ToLower().Contains(txtbxSearch.Text.ToLower()))
                    {
                        updatedList.Add(item);
                    }
                }
                else if (chkbxIsbn.IsChecked == true)
                {
                    if (item.isbn.ToLower().Contains(txtbxSearch.Text.ToLower()))
                    {
                        updatedList.Add(item);
                    }
                }
                else if (chkbxYear.IsChecked == true)
                {
                    if (item.year.ToLower().Contains(txtbxSearch.Text.ToLower()))
                    {
                        updatedList.Add(item);
                    }
                }
                else if (chkbxPublisher.IsChecked == true)
                {
                    if (item.publisher.ToLower().Contains(txtbxSearch.Text.ToLower()))
                    {
                        updatedList.Add(item);
                    }
                }
                else if (chkbxCategory.IsChecked == true)
                {
                    foreach (String category in item.category)
                    {
                        if (category.ToLower().Contains(txtbxSearch.Text.ToLower()))
                        {
                            updatedList.Add(item);
                            break;
                        }
                    }
                }
            }
            updatedList = globalValues.xmlC.GetAvailableBooks(updatedList);
            CreateScreen(updatedList);
        }
    }
}
