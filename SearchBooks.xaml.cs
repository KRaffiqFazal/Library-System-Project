using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Linq;

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
        public SearchBooks(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            FirstOpen();

            
        }
        private void FirstOpen()
        {
            filePaths = Directory.GetFiles(@"Books\", "*.png");
            foreach (String item in filePaths)
            { 
                Trace.WriteLine(item);
            }
            availableImgs = new String[filePaths.Length];
            CreateScreen(globalValues.xmlC.GetAvailableBooks());

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
                Trace.WriteLine(availableImgs[i]);
            }
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
            List<Book> bookList = globalValues.xmlC.GetAvailableBooks();
            Book currentBook = bookList[bookList.FindIndex(book => book.isbn == temp.BookIsbn)];
            picBookFocus.Visibility = Visibility.Visible;
            txtblkBookInfoFocus.Text = currentBook.title + "\n\nAuthor: " + currentBook.author + "  Publisher: " + currentBook.publisher + "\n\nEdition: " + currentBook.edition
                + "  Year: " + currentBook.year + "\n\nCategory: " + String.Join(", ", currentBook.category) + "\n\nISBN: " + currentBook.isbn + "\n\n" + currentBook.description + 
                "\n\nAvailable Copies: " + currentBook.availableCopies;


            txtblkBookInfoFocus.Visibility = Visibility.Visible;
            scrlvwrFocus.Visibility = Visibility.Visible;
            picBack.Visibility = Visibility.Visible;
            btnCloseFocus.Visibility = Visibility.Visible;

            if (txtblkBookInfoFocus.Text.Contains("Available Copies: 0")) //reserve when no copies available
            {
                btnReserve.Visibility = Visibility.Visible;
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
            if(string.IsNullOrEmpty(txtbxSearch.Text))
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
    }
}
