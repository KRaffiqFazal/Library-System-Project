using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        private Globals globalValues;
        private String[] filePaths;
        private String[] availableImgs;
        private String currentIsbn;
        private String deleteId = "";

        public SearchBooks(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            FirstOpen();
            if (!globalValues.currentUser.userType.Equals("member"))
            {
                Permissions();
            }
            txtbxIsbn.MaxLength = 13;
        }
        /// <summary>
        /// Only librarians/admins can add/delete books
        /// </summary>
        private void Permissions()
        {
            btnAdd.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Gets book images so that they can be displayed
        /// </summary>
        private void FirstOpen()
        {
            filePaths = Directory.GetFiles(Directory.GetCurrentDirectory());
            availableImgs = new String[filePaths.Length];
            CreateScreen(globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler()));

            Hide1();
            Hide2();
        }
        /// <summary>
        /// hides detailed book information
        /// </summary>
        private void Hide1()
        {
            picBookFocus.Visibility = Visibility.Hidden;
            rctnglFocus.Visibility = Visibility.Hidden;
            btnCloseFocus.Visibility = Visibility.Hidden;
            btnReserve.Visibility = Visibility.Hidden;
            txtblkBookInfoFocus.Visibility = Visibility.Hidden;
            scrlvwrFocus.Visibility = Visibility.Hidden;

            btnAdd.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Hides adding/deleting screen
        /// </summary>
        private void Hide2()
        {
            lblId.Visibility = Visibility.Hidden;
            lblIsbn.Visibility = Visibility.Hidden;
            lblTitle.Visibility = Visibility.Hidden;
            lblAuthor.Visibility = Visibility.Hidden;
            lblYear.Visibility = Visibility.Hidden;
            lblPublisher.Visibility = Visibility.Hidden;
            lblEdition.Visibility = Visibility.Hidden;
            lblCategory.Visibility = Visibility.Hidden;
            lblPrice.Visibility = Visibility.Hidden;
            lblDescription.Visibility = Visibility.Hidden;
            txtbxId.Visibility = Visibility.Hidden;
            txtbxIsbn.Visibility = Visibility.Hidden;
            txtbxTitle.Visibility = Visibility.Hidden;
            txtbxAuthor.Visibility = Visibility.Hidden;
            txtbxYear.Visibility = Visibility.Hidden;
            txtbxPublisher.Visibility = Visibility.Hidden;
            txtbxEdition.Visibility = Visibility.Hidden;
            txtbxCategory.Visibility = Visibility.Hidden;
            txtbxPrice.Visibility = Visibility.Hidden;
            scrlvwrDescription.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Reveals adding/deleting screen
        /// </summary>
        private void Show2()
        {
            btnCloseFocus.Visibility = Visibility.Visible;
            rctnglFocus.Visibility = Visibility.Visible;
            lblId.Visibility = Visibility.Visible;
            lblIsbn.Visibility = Visibility.Visible;
            lblTitle.Visibility = Visibility.Visible;
            lblAuthor.Visibility = Visibility.Visible;
            lblYear.Visibility = Visibility.Visible;
            lblPublisher.Visibility = Visibility.Visible;
            lblEdition.Visibility = Visibility.Visible;
            lblCategory.Visibility = Visibility.Visible;
            lblPrice.Visibility = Visibility.Visible;
            lblDescription.Visibility = Visibility.Visible;

            txtbxId.Visibility = Visibility.Visible;
            txtbxIsbn.Visibility = Visibility.Visible;
            txtbxTitle.Visibility = Visibility.Visible;
            txtbxAuthor.Visibility = Visibility.Visible;
            txtbxYear.Visibility = Visibility.Visible;
            txtbxPublisher.Visibility = Visibility.Visible;
            txtbxEdition.Visibility = Visibility.Visible;
            txtbxCategory.Visibility = Visibility.Visible;
            txtbxPrice.Visibility = Visibility.Visible;
            scrlvwrDescription.Visibility = Visibility.Visible;

            lblError.Visibility = Visibility.Visible;
            lblError.Content = "";

            txtbxId.Text = "";
            txtbxIsbn.Text = "";
            txtbxTitle.Text = "";
            txtbxAuthor.Text = "";
            txtbxYear.Text = "";
            txtbxPublisher.Text = "";
            txtbxEdition.Text = "";
            txtbxCategory.Text = "";
            txtbxPrice.Text = "";
            txtbxDescription.Text = "";
        }
        /// <summary>
        /// Displays all books in the parsed list with their images and information
        /// </summary>
        /// <param name="books"></param>
        private void CreateScreen(List<Book> books)
        {
            BookDisplay currentDisplayed;
            for (int i = 0; i < filePaths.Length; i++)
            {
                availableImgs[i] = filePaths[i].Substring(6, filePaths[i].Length - 10); //gets array of ids that have images
                availableImgs[i] = availableImgs[i].Substring(availableImgs[i].Length - 13);
            }
            String currentDir = Directory.GetCurrentDirectory();
            stackPanel.Children.Clear();
            foreach (Book book in books) //creates the elements in the scrollviewer
            {
                currentDisplayed = new BookDisplay();
                currentDisplayed.Padding = new Thickness(5);
                currentDisplayed.BookInfo = book.title + "\n\nAuthor: " + book.author + "\n\nPublisher: " + book.publisher + "\n\nEdition: " + book.edition;
                currentDisplayed.Cursor = Cursors.Hand;

                if (availableImgs.Contains(book.isbn))
                {
                    currentDisplayed.picBook.Source = new BitmapImage(new Uri(currentDir + @"\" + book.isbn + ".png", UriKind.Absolute));
                }
                else
                {
                    currentDisplayed.picBook.Source = new BitmapImage(new Uri("/0.png", UriKind.Relative));
                    currentDisplayed.picBook.Visibility = Visibility.Visible;
                }
                currentDisplayed.BookIsbn = book.isbn;
                currentDisplayed.MouseLeftButtonDown += OnMouseLeftButtonDown;
                stackPanel.Children.Add(currentDisplayed);
            }
        }
        /// <summary>
        /// When a book's information/image is clicked, detailed information is revealed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rctnglFocus.Visibility = Visibility.Visible;
            BookDisplay temp = new BookDisplay();

            temp = (BookDisplay)sender;
            picBookFocus.Source = temp.picBook.Source;
            List<Book> bookList = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
            Book currentBook = bookList.Find(book => book.isbn == temp.BookIsbn);
            picBookFocus.Visibility = Visibility.Visible;
            txtblkBookInfoFocus.Text = currentBook.title + "\n\nAuthor: " + currentBook.author + "  Publisher: " + currentBook.publisher + "\n\nEdition: " + currentBook.edition
                + "  Year: " + currentBook.year + "\n\nCategory: " + String.Join(", ", currentBook.category) + "\n\nISBN: " + currentBook.isbn + "\n\n" + currentBook.description +
                "\n\nAvailable Copies: " + currentBook.availableCopies;

            txtblkBookInfoFocus.Visibility = Visibility.Visible;
            scrlvwrFocus.Visibility = Visibility.Visible;
            picBack.Visibility = Visibility.Visible;
            btnCloseFocus.Visibility = Visibility.Visible;
            //if no copies available, reserve button needs to be displayed if there are enough copies that can be reserved
            if (txtblkBookInfoFocus.Text.Contains("Available Copies: 0"))
            {
                if (!globalValues.xmlC.ToReserve(currentBook).Equals("") && globalValues.currentUser.reserved.Equals("") && !(globalValues.currentUser.borrowedBooks.FindIndex(book => book.isbn == currentBook.isbn) != -1)) //a copy available that is not reserved, the user is not reserving a book and the user has not borrowed a copy of the book they wish to reserve
                {
                    currentIsbn = temp.BookIsbn;
                    btnReserve.Visibility = Visibility.Visible;
                    btnReserve.Content = "Reserve";
                }
                else if (!globalValues.currentUser.reserved.Equals(""))
                {
                    lblError.Content = "You have already reserved another book.";
                }
                else if (globalValues.currentUser.borrowedBooks.FindIndex(book => book.isbn == currentBook.isbn) != -1)
                {
                    lblError.Content = "You cannot reserve a copy of the same borrowed book.";
                }
                else
                {
                    lblError.Content = "All copies have been reserved";
                }
            }
        }
        /// <summary>
        /// Sends to homepage
        /// </summary>
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
        /// <summary>
        /// When specific book details are closed, books are reloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseFocus_Click(object sender, RoutedEventArgs e)
        {
            Hide1();
            Hide2();
            if (!globalValues.currentUser.userType.Equals("member"))
            {
                btnAdd.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
            }
            lblError.Content = "";
            CreateScreen(globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler()));
        }

        private async void btnReserve_Click(object sender, RoutedEventArgs e) //button only seen if reservation possible
        {
            //checks if book can be reserved
            if (btnReserve.Content.ToString().Equals("Reserve"))
            {
                List<Book> booksAvailable = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
                Book currentBook = booksAvailable.Find(book => book.isbn == currentIsbn);
                if (!globalValues.xmlC.ToReserve(currentBook).Equals("")) //checks if it has not been reserved whilst the page is open
                {
                    globalValues.currentUser.reserved = globalValues.xmlC.ToReserve(currentBook);
                    currentBook.reserved = DateTime.MaxValue; //when it comes back in stock it will change
                    globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                    globalValues.xmlC.UpdateRecord(currentBook, true);
                    globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " has reserved " + currentBook.id);
                    lblError.Content = "Reservation Successful";
                    btnReserve.Visibility = Visibility.Hidden;
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
            //checks if the entered details are sufficient to add a book to the library
            else if (btnReserve.Content.ToString().Equals("Add"))
            {
                Decimal test;
                //gets information from text box to form new book
                if ((txtbxIsbn.Text.Length != 10 && txtbxIsbn.Text.Length != 13) || !Decimal.TryParse(txtbxPrice.Text, out test) || txtbxTitle.Text.Equals("") || txtbxAuthor.Text.Equals("") ||
                    txtbxYear.Text.Equals("") || txtbxPublisher.Text.Equals("") || txtbxEdition.Text.Equals("") || txtbxCategory.Text.Equals("") || txtbxDescription.Text.Equals(""))
                {
                    lblError.Content = "Error: Please fill out empty spaces and enter valid values for 'ISBN', 'year' and 'price' ";
                }
                else //if entered details are suitable values and not empty, saves book
                {
                    Book newBook = new Book();
                    newBook.id = txtbxId.Text;
                    newBook.isbn = txtbxIsbn.Text;
                    newBook.title = txtbxTitle.Text;
                    newBook.author = txtbxAuthor.Text;
                    newBook.year = txtbxYear.Text;
                    newBook.publisher = txtbxPublisher.Text;
                    newBook.edition = txtbxEdition.Text;
                    newBook.category = txtbxCategory.Text;
                    newBook.price = Decimal.Parse(txtbxPrice.Text);
                    newBook.dueDate = DateTime.MinValue;
                    newBook.description = txtbxDescription.Text;
                    globalValues.xmlC.AddBookRecord(newBook);
                    lblError.Content = "Book Added!";
                    globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " added a new book to the library with id: " + newBook.id);
                    ResetDeletionFields();
                }
            }
            else //Delete a book from the library
            {
                List<Book> books = globalValues.xmlC.BookCompiler();
                int index = books.FindIndex(book => book.id == txtbxId.Text); //position of book to delete in list books
                if (index != -1) //value entered exists
                {
                    if (globalValues.xmlC.GetAllUsers(globalValues.adminUser).FindIndex(user => user.reserved == books[index].id) != -1) //if a user has reserved this, it is deleted from their reservation list
                    {
                        User temp = globalValues.xmlC.GetAllUsers(globalValues.adminUser).Find(user => user.reserved == books[index].id);
                        temp.reserved = "";
                        temp.notifications.Add(books[index].title + " has been removed from our library, sorry for the inconvenience caused.");
                        globalValues.xmlC.UpdateUserRecord(temp);
                        globalValues.SendNotifications(temp);
                    }
                    if (books[index].dueDate == DateTime.MinValue) //book is in stock and not borrowed
                    {
                        globalValues.xmlC.DeleteBookRecord(txtbxId.Text);
                        lblError.Content = "Book Deleted.";
                        ResetDeletionFields();
                    }
                    else //book is currently borrowed by a user
                    {
                        if (deleteId.Equals(txtbxId.Text)) //user presses the delete button again to confirm deletion
                        {
                            List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.adminUser);
                            foreach (User temp in allUsers)
                            {
                                if (BookChecker(temp, books[index]))
                                {
                                    temp.borrowedBooks.Remove(books[index]);
                                    temp.notifications.Add(books[index].title + " has been deleted off our system, please return the book to us as soon as you can. Thanks!");
                                    globalValues.xmlC.UpdateUserRecord(temp);
                                    globalValues.SendNotifications(temp);
                                    globalValues.xmlC.DeleteBookRecord(books[index].id);
                                    lblError.Content = "Book Deleted.";
                                    ResetDeletionFields();
                                    deleteId = "";
                                    break;
                                }
                            }
                        }
                        else //user presses delete for first time and system informs them that a member possesses the book
                        {
                            deleteId = txtbxId.Text;
                            lblError.Content = "Book is currently borrowed, please confirm by deletion.";
                        }
                    }
                    globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " deleted " + books[index].id);
                }
                else //value entered does not exist
                {
                    lblError.Content = "Cannot find book ID.";
                }
            }
        }

        private bool BookChecker(User checkUser, Book toCheck)
        {
            List<Book> borrowedBooks = checkUser.borrowedBooks;
            foreach (Book borrow in borrowedBooks)
            {
                if (borrow.id == toCheck.id)
                {
                    return true;
                }
            }
            return false;
        }

        private async void ResetDeletionFields()
        {
            txtbxId.Text = "";
            txtbxIsbn.Text = "";
            txtbxTitle.Text = "";
            txtbxAuthor.Text = "";
            txtbxYear.Text = "";
            txtbxPublisher.Text = "";
            txtbxEdition.Text = "";
            txtbxCategory.Text = "";
            txtbxPrice.Text = "";
            txtbxDescription.Text = "";
            await Task.Delay(3000);
            lblError.Content = "";
        }
        /// <summary>
        /// Checks which filters are active and which books correspond to them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e) //displays books based on filters selected
        {
            List<Book> newList = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
            List<Book> updatedList = new List<Book>();
            if (txtbxSearch.Text.Equals("")) //if nothing has been entered, all results shown
            {
                CreateScreen(newList);
                return;
            }
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
                    if (item.category.ToLower().Contains(txtbxSearch.Text.ToLower()))
                    {
                        updatedList.Add(item);
                    }
                }
            }
            updatedList = globalValues.xmlC.GetAvailableBooks(updatedList);
            CreateScreen(updatedList);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnReserve.Content = "Add";
            btnReserve.Visibility = Visibility.Visible;
            Show2();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            btnReserve.Content = "Delete";
            btnReserve.Visibility = Visibility.Visible;
            Show2();
        }

        private void txtbxIsbn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Autofills book details if ID is entered and makes other fields immutable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtbxId_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Delay(250);
            List<Book> books = globalValues.xmlC.BookCompiler();
            if (btnReserve.Content.ToString().Equals("Delete"))
            {
                txtbxId.IsEnabled = true;
                if (books.FindIndex(book => book.id == txtbxId.Text) != -1) //book exists
                {
                    Book book = books.Find(x => x.id == txtbxId.Text);
                    txtbxIsbn.Text = book.isbn;
                    txtbxTitle.Text = book.title;
                    txtbxAuthor.Text = book.author;
                    txtbxYear.Text = book.year;
                    txtbxPublisher.Text = book.publisher;
                    txtbxEdition.Text = book.edition;
                    txtbxCategory.Text = book.category;
                    txtbxPrice.Text = string.Format("{0:C}", book.price).Substring(1);
                    txtbxDescription.Text = book.description;

                    txtbxIsbn.IsEnabled = false;
                    txtbxTitle.IsEnabled = false;
                    txtbxAuthor.IsEnabled = false;
                    txtbxYear.IsEnabled = false;
                    txtbxPublisher.IsEnabled = false;
                    txtbxEdition.IsEnabled = false;
                    txtbxCategory.IsEnabled = false;
                    txtbxPrice.IsEnabled = false;
                    txtbxDescription.IsEnabled = false;
                }
                else //if the book does not exist, nothing displayed
                {
                    txtbxIsbn.Text = "";
                    txtbxTitle.Text = "";
                    txtbxAuthor.Text = "";
                    txtbxYear.Text = "";
                    txtbxPublisher.Text = "";
                    txtbxEdition.Text = "";
                    txtbxCategory.Text = "";
                    txtbxPrice.Text = "";
                    txtbxDescription.Text = "";
                }
            }
            else //add
            {
                txtbxIsbn.IsEnabled = true;
                txtbxTitle.IsEnabled = true;
                txtbxAuthor.IsEnabled = true;
                txtbxYear.IsEnabled = true;
                txtbxPublisher.IsEnabled = true;
                txtbxEdition.IsEnabled = true;
                txtbxCategory.IsEnabled = true;
                txtbxPrice.IsEnabled = true;
                txtbxDescription.IsEnabled = true;

                int i = 1;
                while (books.FindIndex(book => book.id == i.ToString()) != -1) //finds available id
                {
                    i++;
                }
                txtbxId.Text = i.ToString();
                txtbxId.IsEnabled = false;
            }
        }
        /// <summary>
        /// Autofills details if ISBN is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtbxIsbn_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Delay(250);
            List<Book> books = globalValues.xmlC.BookCompiler();
            if (books.FindIndex(book => book.isbn == txtbxIsbn.Text) != -1) //book exists
            {
                Book book = books.Find(x => x.isbn == txtbxIsbn.Text);
                txtbxTitle.Text = book.title;
                txtbxAuthor.Text = book.author;
                txtbxYear.Text = book.year;
                txtbxPublisher.Text = book.publisher;
                txtbxEdition.Text = book.edition;
                txtbxCategory.Text = book.category;
                txtbxPrice.Text = string.Format("{0:C}", book.price).Substring(1);
                txtbxDescription.Text = book.description;
            }
            else
            {
                txtbxTitle.Text = "";
                txtbxAuthor.Text = "";
                txtbxYear.Text = "";
                txtbxPublisher.Text = "";
                txtbxEdition.Text = "";
                txtbxCategory.Text = "";
                txtbxPrice.Text = "";
                txtbxDescription.Text = "";
            }
        }
    }
}