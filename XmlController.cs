using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Navigation;
using System.Windows.Automation.Peers;

namespace Library_System
{
    public class XmlController
    {
        public String userPath { get; set; }
        public String bookPath { get; set; }
        public XmlDocument doc { get; set; }


        public XmlController() 
        {
            userPath = "Users.xml";
            bookPath = "Library.xml";
            doc = new XmlDocument();

        }
        public bool Exists(String enteredID)
        { 
            doc.Load(userPath);
            XmlNode node = UserType(enteredID);
            
            if (node != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public String[] GetInfo(String userID)
        {
            doc.Load(userPath);
            XmlNode userNode = UserType(userID);
            String[] temp = new String[4];
            temp[0] = userNode.ChildNodes.Item(1).InnerText; //name
            temp[1] = userNode.ChildNodes.Item(2).InnerText; //phone number
            temp[2] = userNode.ChildNodes.Item(3).InnerText; //email
            temp[3] = userNode.ChildNodes.Item(5).InnerText; //notifications
            return temp;
        }

        public XmlNode UserType(String userID)
        {
            User currentUser = new User(userID);
            doc.Load(userPath);
            if (userID[0] == '1')
            {
                currentUser.userType = "member";
            }
            else if (userID[0] == '2')
            {
                currentUser.userType = "librarian";
            }
            else if (userID[0] == '3')
            {
                currentUser.userType = "admin";
            }
            else
            {
                return null;
            }
            if (currentUser.userType.Equals("member"))
            {
                return doc.SelectSingleNode("/users/members/user[id='" + currentUser.userID + "']");

            }
            else if (currentUser.userType.Equals("librarian"))
            {
                return doc.SelectSingleNode("/users/librarians/user[id='" + currentUser.userID + "']");
            }
            else
            {
                return doc.SelectSingleNode("/users/admins/user[id='" + currentUser.userID + "']");
            }
        }
        public List<Book> GetBorrowedBooks (String userID)
        {
            XmlNode userNode = UserType(userID);
            XmlNode bookNode;
            List<String> bookIDList = new List<String>();
            if (userNode.ChildNodes.Item(4).InnerText == "") //no borrowed books
            {
                return null;
            }
            bookIDList.AddRange(userNode.ChildNodes.Item(4).InnerText.Split(',')); //contains ids of borrowed books
            List<Book> books = new List<Book>();
            Book temp = new Book();
            doc.Load(bookPath);
            Decimal price;
            //update each empty book with values from database and put it in list of books that user has borrowed
            foreach (String bookID in bookIDList)
            {
                bookNode = doc.SelectSingleNode("/library/book[id='" + bookID + "']");
                temp.id = bookNode.ChildNodes.Item(0).InnerText;
                temp.title = bookNode.ChildNodes.Item(1).InnerText;
                temp.author = bookNode.ChildNodes.Item(2).InnerText;
                temp.year = bookNode.ChildNodes.Item(3).InnerText;
                temp.publisher = bookNode.ChildNodes.Item(4).InnerText;
                temp.edition = bookNode.ChildNodes.Item(5).InnerText;
                temp.isbn = bookNode.ChildNodes.Item(6).InnerText;
                temp.category = new List<String>();
                temp.category.AddRange(bookNode.ChildNodes.Item(7).InnerText.Split(';'));
                temp.description = bookNode.ChildNodes.Item(8).InnerText;

                // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-7.0
                temp.dueDate = DateTime.ParseExact(bookNode.ChildNodes.Item(9).InnerText, "ddMMyyyy", CultureInfo.InvariantCulture); //converts a string of numbers from database to dateTime;

                //https://www.educative.io/answers/how-to-convert-a-string-to-a-decimal-in-c-sharp
                Decimal.TryParse(bookNode.ChildNodes.Item(10).InnerText, out price);
                temp.price = price;

                books.Add(temp);
            }
            return books;
        }
        public List<Book> GetAvailableBooks()
        {

            List<Book> books = BookCompiler();
            for (int i = 0; i < books.Count; i++) //finds all duplicates and changes available copies value accordingly
            {
                books[i].availableCopies = 0;
                foreach (Book book in books)
                {
                    if (books[i].isbn.Equals(book.isbn) && book.dueDate == DateTime.MinValue) //if a duplicate has its date and time set to the min value it is not being borrowed
                    {
                        books[i].availableCopies++;
                    }
                }
            }
            IEnumerable<String> uniqueIsbns = books.Select(x => x.isbn).Distinct();
            List<String> uniqueIsbnsList = uniqueIsbns.ToList();
            List<Book> correctBooks = new List<Book>();
            for (int i=0; i < uniqueIsbnsList.Count; i++) //deletes duplicates to leave a correct list
            {
                for (int j = 0; j < books.Count; j++)
                {
                    if (uniqueIsbnsList[i].Equals(books[j].isbn))
                    {
                        correctBooks.Add(books[j]);
                        break;
                    }
                }
            }

            return correctBooks;
        }
        public List<Book> GetAllBooks()
        { 
            return BookCompiler();
        }

        private List<Book> BookCompiler()
        {
            List<Book> books = new List<Book>();
            Book currentBook;
            doc.Load(bookPath);
            //fix this
            XmlNode rootNode = doc.SelectSingleNode("/library");
            XmlNode currentNode;
            Decimal price;
            for (int i = 0; i < rootNode.ChildNodes.Count; i++) //adds all the books to a list based on xml node information
            {
                currentBook = new Book();
                currentNode = doc.SelectSingleNode("/library/book[id='" + (i + 1) + "']");
                currentBook.id = currentNode.ChildNodes.Item(0).InnerText;
                currentBook.title = currentNode.ChildNodes.Item(1).InnerText;
                currentBook.author = currentNode.ChildNodes.Item(2).InnerText;
                currentBook.year = currentNode.ChildNodes.Item(3).InnerText;
                currentBook.publisher = currentNode.ChildNodes.Item(4).InnerText;
                currentBook.edition = currentNode.ChildNodes.Item(5).InnerText;
                currentBook.isbn = currentNode.ChildNodes.Item(6).InnerText;
                currentBook.category = new List<String>();
                currentBook.category.AddRange(currentNode.ChildNodes.Item(7).InnerText.Split(';'));
                currentBook.description = currentNode.ChildNodes.Item(8).InnerText;
                currentBook.dueDate = DateTime.ParseExact(currentNode.ChildNodes.Item(9).InnerText, "ddMMyyyy", CultureInfo.InvariantCulture);
                Decimal.TryParse(currentNode.ChildNodes.Item(10).InnerText, out price);
                currentBook.price = price;
                books.Add(currentBook);
            }
            return books;
        }
        public void UpdateRecord(Book book)
        {
            doc.Load(bookPath);
            String[] date = book.dueDate.ToShortDateString().Split('/');
            String newDate = date[0] + date[1] + date[2];
            XmlNode insert = doc.SelectSingleNode("/library/book[id='" + book.id + "']");
            insert.ChildNodes.Item(9).InnerText = newDate;
        }

        public void UpdateUserRecord(User user)
        {
            doc.Load(userPath);
            XmlNode userNode = UserType(user.userID);
            if (userNode.ChildNodes.Item(4) == null)
            {
                userNode.ChildNodes.Item(4).InnerText = user.borrowedBooks[0].id;
            }
            else
            {
                String books = "";
                for (int i = 0; i < user.borrowedBooks.Count - 1; i++)
                {
                    books += user.borrowedBooks[i].id + ",";
                }
                books += user.borrowedBooks[user.borrowedBooks.Count - 1].id;
                userNode.ChildNodes.Item(4).InnerText = books;
            }   
        }
     }
}
