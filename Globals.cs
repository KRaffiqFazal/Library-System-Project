using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Library_System
{
    /// <summary>
    /// Globally accessible variables on all screens
    /// </summary>
    public class Globals
    {
        public User currentUser { get; set; }
        public XmlController xmlC { get; set; }
        public User adminUser { get; set; }

        public Globals()
        {
            adminUser = new User("3");

        }
        /// <summary>
        /// Emails a user with all their notifications https://www.youtube.com/watch?v=lk5dhDzfzsU
        /// </summary>
        public void SendNotifications(User currentUser)
        {
            String fromMail = "bookingslibrary@gmail.com";
            String fromPassword = "cjgygkwvnuipnqrt";

            MailMessage message;
            String body;
            User user = currentUser;
            message = new MailMessage();
            body = "<html><body><div display=\"flex\" justify-content=\"center\"><img src=\"https://i.imgur.com/miQEa5a.png\" alt=\"Library Bookings Co. Logo\" width=\"500\"></div><b>Hi " + user.name + ",</b><br><br>Notifications:<br>";
            if (user.notifications.Count != 0)
            {
                foreach (String notif in user.notifications)
                {
                    body += notif + "<br>";
                }
            }
            else
            {
                body += "None!<br><br>";
            }
            if (!user.reserved.Equals(""))
            {
                List<Book> allBooks = xmlC.BookCompiler();
                body += "Reserved: " + allBooks.Find(book => book.id == user.reserved).title + ".<br><br>";
            }
            else
            {
                body += "You have no reserved books.<br><br>";
            }
            body += "Borrowed Books:<br>";
            if (user.borrowedBooks.Count != 0)
            {
                foreach (Book book in user.borrowedBooks)
                {
                    if (book.dueDate > DateTime.Now)
                    {
                        body += book.title + " is due on " + book.dueDate.ToShortDateString() + ".<br><br>";
                    }
                    else
                    {
                        body += book.title + " is due IMMEDIATELY!";
                    }
                }
            }
            else
            {
                body += "None!<br><br>";
            }
            body += "Fine: " + string.Format("{0:C}", user.fine) + " due.</body></html>";

            message.From = new MailAddress(fromMail);

            message.Subject = "You Have New Notifications!";
            message.To.Add(new MailAddress(user.email));
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            try
            {
                smtpClient.Send(message);
            }
            catch { }
        }
        /// <summary>
        /// Notifies librarians that there are members who have not returned their books yet
        /// </summary>
        public void NotifyUsers()
        {
            List<User> allUsers = xmlC.GetAllUsers(adminUser);
            foreach (User currentUser in allUsers)
            {
                if (currentUser.Overdue())
                {
                    List<User> librarians = xmlC.GetAllUsers(adminUser);
                    foreach (User user in librarians)
                    {
                        if (user.userType.Equals("librarian"))
                        {
                            user.notifications.Add(DateTime.Now.ToShortDateString() + " " + user.userID + " has at least one overdue book.");
                            xmlC.UpdateUserRecord(user);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Each time a user has borrowed a book, both logs need to update
        /// </summary>
        public void BorrowedLog(Book toUpdate, User userUpdate)
        {
            String borrowedPath = @"Borrowed.txt";
            String userPath = @"Users.txt";
            CheckLogsExistence();
            String[] lines = File.ReadAllLines(borrowedPath);
            String lastLineDate = lines.Last().Split(';')[0];
            String[] currentMonth = DateTime.Now.ToShortDateString().Split('/');

            if (lastLineDate.Equals(currentMonth[1] + currentMonth[2]))
            {
                //writes on current line
                using (StreamWriter sw = File.AppendText(borrowedPath))
                {
                    sw.Write(toUpdate.isbn + ";");
                }
            }
            else
            {
                //writes on new line with month followed by the isbn
                using (StreamWriter sw = File.AppendText(borrowedPath))
                {
                    sw.Write(Environment.NewLine + currentMonth[1] + currentMonth[2] + ";" + toUpdate.isbn + ";");
                }
            }

            lines = File.ReadAllLines(userPath);
            lastLineDate = lines.Last().Split(';')[0];
            if (lastLineDate.Equals(currentMonth[1] + currentMonth[2]))
            {
                using (StreamWriter sw = File.AppendText(userPath))
                {
                    sw.Write(userUpdate.userID + ";");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(userPath))
                {
                    sw.Write(Environment.NewLine + currentMonth[1] + currentMonth[2] + ";" + userUpdate.userID + ";");
                }
            }

        }
        /// <summary>
        /// Check if all logs exist
        /// </summary>
        public void CheckLogsExistence()
        {
            String usersPath = @"Users.txt";
            String bookPath = @"Borrowed.txt";
            String logPath = @"Log.txt";
            if (!File.Exists(usersPath))
            {
                var file = File.Create(usersPath);
                file.Close();
                File.SetAttributes(usersPath, FileAttributes.Hidden);
                using (StreamWriter sw = File.AppendText(usersPath))
                {
                    String[] temp = DateTime.Now.ToShortDateString().Split('/');
                    sw.Write(temp[1] + temp[2] + ";");
                }

            }
            if (!File.Exists(bookPath))
            {
                var file = File.Create(bookPath);
                file.Close();
                File.SetAttributes(bookPath, FileAttributes.Hidden);
                using (StreamWriter sw = File.AppendText(bookPath))
                {
                    String[] temp = DateTime.Now.ToShortDateString().Split('/');
                    sw.Write(temp[1] + temp[2] + ";");
                }
            }
            if (!File.Exists(logPath))
            {
                var file = File.Create(logPath);
                file.Close();
                File.SetAttributes(logPath, FileAttributes.Hidden);
            }
        }

        public void UpdateDetailedLog(String message)
        {
            CheckLogsExistence();
            String logPath = @"Log.txt";
            using (StreamWriter sw = File.AppendText(logPath))
            {
                sw.Write(DateTime.Now + " " + message + Environment.NewLine);
            }
        }
    }
}
