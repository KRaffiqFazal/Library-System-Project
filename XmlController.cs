using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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
        /// <summary>
        /// Returns user information based on their node
        /// </summary>
        public String[] GetInfo(String userID)
        {
            doc.Load(userPath);
            XmlNode userNode = UserType(userID);
            String[] temp = new String[6];
            temp[0] = userNode.ChildNodes.Item(1).InnerText; //name
            temp[1] = userNode.ChildNodes.Item(2).InnerText; //phone number
            temp[2] = userNode.ChildNodes.Item(3).InnerText; //email
            temp[3] = userNode.ChildNodes.Item(5).InnerText; //notifications
            temp[4] = userNode.ChildNodes.Item(6).InnerText; //reserved book
            temp[5] = userNode.ChildNodes.Item(7).InnerText; //fine
            return temp;
        }
        public User CreateUser(String userID)
        {
            doc.Load(userPath);
            User newUser = new User(userID);
            String[] userInfo = GetInfo(userID);
            XmlNode userNode = UserType(userID);
            newUser.name = userInfo[0];
            newUser.phoneNumber = userInfo[1];
            newUser.email = userInfo[2];
            newUser.borrowedBooks = GetBorrowedBooks(userID);
            if (userInfo[3].Equals("") || !userInfo[3].Contains("}"))
            {
                if (!userInfo[3].Equals("")) //new notification
                {
                    newUser.notifications.Add(userInfo[3]);
                }
            }
            else // >1 notifications
            {
                newUser.notifications.AddRange(userInfo[3].Split('}'));
            }
            newUser.reserved = userInfo[4];
            newUser.fine = Decimal.Parse(userInfo[5]);
            return newUser;

        }
        /// <summary>
        /// Returns node that the entered user is on
        /// </summary>
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
        public List<Book> GetBorrowedBooks(String userID)
        {
            XmlNode userNode = UserType(userID);
            XmlNode bookNode;
            List<String> bookIDList = new List<String>();
            List<Book> books = new List<Book>();
            if (userNode.ChildNodes.Item(4).InnerText == "") //no borrowed books
            {
                return books;
            }
            bookIDList.AddRange(userNode.ChildNodes.Item(4).InnerText.Split(',')); //contains ids of borrowed books
            Book temp;
            doc.Load(bookPath);
            Decimal price;
            //update each empty book with values from database and put it in list of books that user has borrowed
            foreach (String bookID in bookIDList)
            {
                bookNode = doc.SelectSingleNode("/library/book[id='" + bookID + "']");
                if (bookNode == null)
                {
                    continue;
                }
                temp = new Book();
                temp.id = bookNode.ChildNodes.Item(0).InnerText;
                temp.title = bookNode.ChildNodes.Item(1).InnerText;
                temp.author = bookNode.ChildNodes.Item(2).InnerText;
                temp.year = bookNode.ChildNodes.Item(3).InnerText;
                temp.publisher = bookNode.ChildNodes.Item(4).InnerText;
                temp.edition = bookNode.ChildNodes.Item(5).InnerText;
                temp.isbn = bookNode.ChildNodes.Item(6).InnerText;
                temp.category = bookNode.ChildNodes.Item(7).InnerText;
                temp.description = bookNode.ChildNodes.Item(8).InnerText;

                // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-7.0
                temp.dueDate = DateTime.ParseExact(bookNode.ChildNodes.Item(9).InnerText, "ddMMyyyy", CultureInfo.InvariantCulture); //converts a string of numbers from database to dateTime;

                //https://www.educative.io/answers/how-to-convert-a-string-to-a-decimal-in-c-sharp
                Decimal.TryParse(bookNode.ChildNodes.Item(10).InnerText, out price);
                temp.price = price;

                temp.reserved = DateTime.ParseExact(bookNode.ChildNodes.Item(11).InnerText, "ddMMyyyy", CultureInfo.InvariantCulture);
                temp.renewed = bool.Parse(bookNode.ChildNodes.Item(12).InnerText);

                books.Add(temp);
            }
            return books;
        }
        /// <summary>
        /// Sorts compiled books by getting rid of duplicates
        /// </summary>
        public List<Book> GetAvailableBooks(List<Book> books)
        {
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
            for (int i = 0; i < uniqueIsbnsList.Count; i++) //deletes duplicates to leave a correct list
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
        /// <summary>
        /// Returns all books in the xml file without sorting them into their duplicates
        /// </summary>
        public List<Book> BookCompiler()
        {
            List<Book> books = new List<Book>();
            Book currentBook;
            doc.Load(bookPath);
            //fix this
            XmlNode rootNode = doc.SelectSingleNode("/library");
            Decimal price;
            foreach (XmlNode currentNode in rootNode.ChildNodes) //adds all the books to a list based on xml node information
            {
                currentBook = new Book();
                currentBook.id = currentNode.ChildNodes.Item(0).InnerText;
                currentBook.title = currentNode.ChildNodes.Item(1).InnerText;
                currentBook.author = currentNode.ChildNodes.Item(2).InnerText;
                currentBook.year = currentNode.ChildNodes.Item(3).InnerText;
                currentBook.publisher = currentNode.ChildNodes.Item(4).InnerText;
                currentBook.edition = currentNode.ChildNodes.Item(5).InnerText;
                currentBook.isbn = currentNode.ChildNodes.Item(6).InnerText;
                currentBook.category = currentNode.ChildNodes.Item(7).InnerText;
                currentBook.description = currentNode.ChildNodes.Item(8).InnerText;
                currentBook.dueDate = DateTime.ParseExact(currentNode.ChildNodes.Item(9).InnerText, "ddMMyyyy", CultureInfo.InvariantCulture);
                Decimal.TryParse(currentNode.ChildNodes.Item(10).InnerText, out price);
                currentBook.price = price;

                currentBook.reserved = DateTime.ParseExact(currentNode.ChildNodes.Item(11).InnerText, "ddMMyyyy", CultureInfo.InvariantCulture);
                currentBook.renewed = bool.Parse(currentNode.ChildNodes.Item(12).InnerText);
                books.Add(currentBook);
            }
            return books;
        }
        /// <summary>
        /// Updates book record
        /// </summary>
        public void UpdateRecord(Book book, bool option)
        {
            doc.Load(bookPath);
            XmlNode insert = doc.SelectSingleNode("/library/book[id='" + book.id + "']");
            String newDate;
            if (option)
            {
                String[] date = book.dueDate.ToShortDateString().Split('/');
                newDate = date[0] + date[1] + date[2];

            }
            else
            {
                newDate = "01010001";
            }
            insert.ChildNodes.Item(9).InnerText = newDate;
            String[] reserved = book.reserved.ToShortDateString().Split('/');
            insert.ChildNodes.Item(11).InnerText = reserved[0] + reserved[1] + reserved[2];
            insert.ChildNodes.Item(12).InnerText = book.renewed.ToString();
            doc.Save(bookPath);
        }

        public void DeleteUserRecord(String userId)
        {
            doc.Load(userPath);
            XmlNode toDelete;
            if (userId[0] == '1')
            {
                toDelete = doc.SelectSingleNode("/users/members/user[id='" + userId+ "']");

            }
            else if (userId[0] == '2')
            {
                toDelete = doc.SelectSingleNode("/users/librarians/user[id='" + userId + "']");
            }
            else
            {
                toDelete = doc.SelectSingleNode("/users/admins/user[id='" + userId + "']");
            }
            toDelete.ParentNode.RemoveChild(toDelete);
            doc.Save(userPath);
        }

        public void AddUserRecord(User user) //creates nodes for the new user
        {
            doc.Load(userPath);
            XmlNode insertLocation;

            XmlNode userNode = doc.CreateElement("user");
            XmlNode idNode = doc.CreateElement("id");
            XmlNode nameNode = doc.CreateElement("name");
            XmlNode phoneNode = doc.CreateElement("phone_number");
            XmlNode emailNode = doc.CreateElement("email");
            XmlNode borrowedBooksNode = doc.CreateElement("borrowed_books");
            XmlNode notificationsNode = doc.CreateElement("notifications");
            XmlNode reservedNode = doc.CreateElement("reserved");
            XmlNode fineNode = doc.CreateElement("fine");

            idNode.InnerText = user.userID;
            nameNode.InnerText = user.name;
            phoneNode.InnerText = user.phoneNumber;
            emailNode.InnerText = user.email;
            borrowedBooksNode.InnerText = "";
            notificationsNode.InnerText = "";
            reservedNode.InnerText = user.reserved;
            fineNode.InnerText = user.fine.ToString();

            userNode.AppendChild(idNode);
            userNode.AppendChild(nameNode);
            userNode.AppendChild(phoneNode);
            userNode.AppendChild(emailNode);
            userNode.AppendChild(borrowedBooksNode);
            userNode.AppendChild(notificationsNode);
            userNode.AppendChild(reservedNode);
            userNode.AppendChild(fineNode);


            if (user.userType.Equals("member")) //find which child it must be inserted into
            {
                insertLocation = doc.SelectSingleNode("/users/members");
            }
            else if (user.userType.Equals("librarian"))
            {
                insertLocation = doc.SelectSingleNode("/users/librarians");
            }
            else
            {
                insertLocation = doc.SelectSingleNode("/users/admins");
            }
            insertLocation.AppendChild(userNode);
            doc.Save(userPath);
        }

        public void UpdateUserRecord(User user) //updates the xml user file based on current global variables
        {
            doc.Load(userPath);
            XmlNode userNode = UserType(user.userID);
            userNode.ChildNodes.Item(0).InnerText = user.userID;
            userNode.ChildNodes.Item(1).InnerText = user.name;
            userNode.ChildNodes.Item(2).InnerText = user.phoneNumber;
            userNode.ChildNodes.Item(3).InnerText = user.email;
            if (userNode.ChildNodes.Item(4).InnerText.Equals("") && user.borrowedBooks.Count == 1)
            {
                userNode.ChildNodes.Item(4).InnerText = user.borrowedBooks[0].id;
            }
            else
            {
                if (user.borrowedBooks.Count > 0)
                {
                    String books = "";
                    for (int i = 0; i < user.borrowedBooks.Count - 1; i++)
                    {
                        books += user.borrowedBooks[i].id + ",";
                    }
                    books += user.borrowedBooks[user.borrowedBooks.Count - 1].id;
                    userNode.ChildNodes.Item(4).InnerText = books;
                }
                else
                {
                    userNode.ChildNodes.Item(4).InnerText = "";
                }
            }
            if (user.notifications.Count == 1) //new notification needs to be added
            {
                userNode.ChildNodes.Item(5).InnerText = user.notifications[0];
            }
            else if (user.notifications.Count == 0)
            {
                userNode.ChildNodes.Item(5).InnerText = "";
            }
            else if (user.notifications.Count > 1)
            {
                String notifs = "";
                for (int i = 0; i < user.notifications.Count - 1; i++)
                {
                    notifs += user.notifications[i] + "}";
                }
                notifs += user.notifications[user.notifications.Count - 1];
                userNode.ChildNodes.Item(5).InnerText = notifs;
            }
            userNode.ChildNodes.Item(6).InnerText = user.reserved;
            userNode.ChildNodes.Item(7).InnerText = user.fine.ToString();
            doc.Save(userPath);
        }
        /// <summary>
        /// finalBook is a book of multiple copies but needs to check if a book has already been reserved
        /// </summary>
        public String ToReserve(Book finalBook)
        {
            List<Book> fullList = BookCompiler();
            foreach (Book bookMain in fullList)
            {
                if (bookMain.isbn.Equals(finalBook.isbn) && bookMain.reserved == DateTime.MinValue)
                {
                    return bookMain.id; //book can be reserved
                }
            }
            return "";
        }
        public void AddBookRecord(Book toAdd)
        {
            doc.Load(bookPath);
            XmlNode insertLocation = doc.SelectSingleNode("/library");

            XmlNode bookNode = doc.CreateElement("book");
            XmlNode idNode = doc.CreateElement("id");
            XmlNode titleNode = doc.CreateElement("title");
            XmlNode authorNode = doc.CreateElement("author");
            XmlNode yearNode = doc.CreateElement("year");
            XmlNode publisherNode = doc.CreateElement("publisher");
            XmlNode editionNode = doc.CreateElement("edition");
            XmlNode isbnNode = doc.CreateElement("isbn");
            XmlNode categoryNode = doc.CreateElement("category");
            XmlNode descriptionNode = doc.CreateElement("description");
            XmlNode dueDateNode = doc.CreateElement("due_date");
            XmlNode priceNode = doc.CreateElement("price");
            XmlNode reservedNode = doc.CreateElement("reserved");
            XmlNode renewedNode = doc.CreateElement("renewed");

            idNode.InnerText = toAdd.id;
            titleNode.InnerText = toAdd.title;
            authorNode.InnerText = toAdd.author;
            yearNode.InnerText = toAdd.year;
            publisherNode.InnerText = toAdd.publisher;
            editionNode.InnerText = toAdd.edition;
            isbnNode.InnerText = toAdd.isbn;
            categoryNode.InnerText = toAdd.category;
            descriptionNode.InnerText = toAdd.description;
            String[] due = toAdd.dueDate.ToShortDateString().Split('/');
            dueDateNode.InnerText = due[0] + due[1] + due[2];
            priceNode.InnerText = toAdd.price.ToString();
            due = toAdd.reserved.ToShortDateString().Split('/');
            reservedNode.InnerText = due[0] + due[1] + due[2];
            renewedNode.InnerText = toAdd.renewed.ToString();

            bookNode.AppendChild(idNode);
            bookNode.AppendChild(titleNode);
            bookNode.AppendChild(authorNode);
            bookNode.AppendChild(yearNode);
            bookNode.AppendChild(publisherNode);
            bookNode.AppendChild(editionNode);
            bookNode.AppendChild(isbnNode);
            bookNode.AppendChild(categoryNode);
            bookNode.AppendChild(descriptionNode);
            bookNode.AppendChild(dueDateNode);
            bookNode.AppendChild(priceNode);
            bookNode.AppendChild(reservedNode);
            bookNode.AppendChild(renewedNode);

            insertLocation.AppendChild(bookNode);
            doc.Save(bookPath);
        }
        public void DeleteBookRecord(String id)
        {
            doc.Load(bookPath);
            XmlNode toDelete = doc.SelectSingleNode("/library/book[id='" + id + "']");
            toDelete.ParentNode.RemoveChild(toDelete);
            doc.Save(bookPath);

        }
        /// <summary>
        /// When someone logs in, checks all books and cancels reservations for books that are past the reservation date
        /// </summary>
        public List<User> CancelReservations()
        {
            Book temp;
            List<User> tempList = new List<User>();
            foreach (User user in GetAllUsers(new User("3")))
            {
                if (!user.reserved.Equals(""))
                {
                    temp = new Book();
                    temp = BookCompiler().Find(book => book.id == user.reserved);
                    if (temp.reserved != DateTime.MinValue && temp.reserved < DateTime.Now) //is currently reserved and past 3 days
                    {
                        temp.reserved = DateTime.MinValue;
                        user.notifications.Add("3 days have passed since" + temp.title + "has come back in stock and now can be borrowed by any member");
                        UpdateRecord(temp, false);
                        user.reserved = "";
                        UpdateUserRecord(user);
                        tempList.Add(user);

                    }
                }
            }
            return tempList;
        }
        public List<User> GetAllUsers(User currentUser)
        {
            doc.Load(userPath);
            XmlNode rootNode = doc.SelectSingleNode("/users");
            XmlNode rootNode1 = rootNode.ChildNodes[0];
            List<User> toAdd = new List<User>();
            User temp;

            foreach (XmlNode node in rootNode1.ChildNodes)
            {
                temp = new User(node.ChildNodes.Item(0).InnerText);
                temp.name = node.ChildNodes.Item(1).InnerText;
                temp.phoneNumber = node.ChildNodes.Item(2).InnerText;
                temp.email = node.ChildNodes.Item(3).InnerText;
                temp.borrowedBooks = GetBorrowedBooks(temp.userID);
                temp.notifications.AddRange(node.ChildNodes.Item(5).InnerText.Split('}'));
                temp.reserved = node.ChildNodes.Item(6).InnerText;
                temp.fine = Decimal.Parse(node.ChildNodes.Item(7).InnerText);
                toAdd.Add(temp);
                
            }
            if (currentUser.userType.Equals("admin")) //only admin and librarian details
            {
                rootNode1 = rootNode.ChildNodes[1];
                foreach (XmlNode node in rootNode1.ChildNodes)
                {

                    temp = new User(node.ChildNodes.Item(0).InnerText);
                    temp.name = node.ChildNodes.Item(1).InnerText;
                    temp.phoneNumber = node.ChildNodes.Item(2).InnerText;
                    temp.email = node.ChildNodes.Item(3).InnerText;
                    temp.borrowedBooks = GetBorrowedBooks(temp.userID);
                    temp.notifications.AddRange(node.ChildNodes.Item(5).InnerText.Split('}'));
                    temp.reserved = node.ChildNodes.Item(6).InnerText;
                    temp.fine = Decimal.Parse(node.ChildNodes.Item(7).InnerText);
                    toAdd.Add(temp);
                }
                rootNode1 = rootNode.ChildNodes[2];
                foreach (XmlNode node in rootNode1.ChildNodes)
                {

                    temp = new User(node.ChildNodes.Item(0).InnerText);
                    temp.name = node.ChildNodes.Item(1).InnerText;
                    temp.phoneNumber = node.ChildNodes.Item(2).InnerText;
                    temp.email = node.ChildNodes.Item(3).InnerText;
                    temp.borrowedBooks = GetBorrowedBooks(temp.userID);
                    temp.notifications.AddRange(node.ChildNodes.Item(5).InnerText.Split('}'));
                    temp.reserved = node.ChildNodes.Item(6).InnerText;
                    temp.fine = Decimal.Parse(node.ChildNodes.Item(7).InnerText);
                    toAdd.Add(temp);
                }   
            }
            return toAdd;
        }
    }
}
