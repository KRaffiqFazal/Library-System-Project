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

namespace Library_System
{
    public class XmlController
    {
        String userPath = "Users.xml"; //borrowed books displayed with "book_id,date_borrowed;book_id2"
        String bookPath = "Library.xml";

        public bool Exists(String enteredID)
        { 
            XmlDocument doc = new XmlDocument();
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
            XmlDocument doc = new XmlDocument();
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
            XmlDocument doc = new XmlDocument();
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
            XmlDocument doc = new XmlDocument();
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
                
                books.Add(temp);
            }
            return books;
        }
    }
}
