using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;

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
            body = "<html><body>Hi " + user.name + ",<br><br>Notifications:<br><br>";
            if (user.notifications.Count != 0)
            {
                foreach (String notif in user.notifications)
                {
                    body += notif + "<br><br>";
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
            body += "Borrowed Books:<br><br>";
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

            message.Subject = "You have new notifications!";
            message.To.Add(new MailAddress(user.email));
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
