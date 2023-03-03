using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;

namespace Library_System
{
    public class XmlController
    {
        String userPath = "Users.xml"; //borrowed books displayed with "book_id,date_borrowed;book_id2"
        String bookPath = "Library.xml";
        XmlDocument doc = new XmlDocument();

        public bool Exists(String enteredID)
        { 
            doc.Load(userPath);
            XmlNode node;
            if (enteredID[0] == '1')
            {
                node = doc.SelectSingleNode("//users/members[id='" + enteredID + "']");
            }
            else if (enteredID[0] == '2')
            {
                node = doc.SelectSingleNode("//users/librarians[id='" + enteredID + "']");
            }
            else if (enteredID[0] == '3')
            {
                node = doc.SelectSingleNode("//users/admins[id='" + enteredID + "']");
            }
            else
            {
                return false;
            }
            
            if (node != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Book> GetBorrowedBooks (User currentUser)
        {
            doc.Load(userPath);
            doc.Load(bookPath);
            XmlNode userNode;
            XmlNode bookNode;
            if (currentUser.userType.Equals("member"))
            {
                userNode = doc.SelectSingleNode("//users/members[id='" + currentUser.userID + "']");
            }
            else if (currentUser.userType.Equals("librarian"))
            {
                userNode = doc.SelectSingleNode("//users/librarians[id='" + currentUser.userID + "']");
            }
            else
            {
                userNode = doc.SelectSingleNode("//users/admins[id='" + currentUser.userID + "']");
            }
            if (userNode == null)
            {
                return null;
            }
            List<String> bookIDList = new List<String>();
            bookIDList.AddRange(userNode.ChildNodes.Item(4).InnerText.Split(',')); //contains ids of borrowed books
            List<Book> books = new List<Book>();
            Book temp = new Book();
            foreach (String bookID in bookIDList)
            {
                bookNode = doc.SelectSingleNode("//library[id='" + bookID + "']");
                temp.id = bookNode.ChildNodes.Item(0).InnerText;
                temp.title = bookNode.ChildNodes.Item(1).InnerText;
                temp.author = bookNode.ChildNodes.Item(2).InnerText;
                temp.year = bookNode.ChildNodes.Item(3).InnerText;
                temp.publisher = bookNode.ChildNodes.Item(4).InnerText;
                temp.edition = bookNode.ChildNodes.Item(5).InnerText;
                temp.isbn = bookNode.ChildNodes.Item(6).InnerText;
                temp.category = new List<String>();
                temp.category.AddRange(bookNode.ChildNodes.Item(7).InnerText.Split(','));
                temp.availableCopies = Convert.ToInt32(bookNode.ChildNodes.Item(8).InnerText);
                temp.description = bookNode.ChildNodes.Item(9).InnerText;
                //add due date by using culture information
            }
            return null;
        }
    }
}
