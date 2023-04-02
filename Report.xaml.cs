using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        Globals globalValues;
        String borrowedPath = @"Borrowed.txt"; //records each time a book is borrowed
        String usersPath = @"Users.txt"; //records each time a user borrows a book
        String fullLog = @"Log.txt";
        public Report(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            PopulateFields();
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private void picBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }
        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }
        /// <summary>
        /// loops through data in both text files and translates content into meaningful information
        /// </summary>
        private void PopulateFields()
        {
            globalValues.CheckLogsExistence();
            //each line is a month, there will be a detailed account and a new month's breakdown
            String[] lines = File.ReadAllLines(borrowedPath);
            List<String> borrowedList = new List<String>();
            borrowedList.AddRange(lines);
            lines = File.ReadAllLines(usersPath);
            List<String> userList = new List<String>();
            userList.AddRange(lines);

            //top 3 and bottom 3 borrowers (most books read)
            List<IdCounter> top3 = new List<IdCounter>();
            IdCounter temp = new IdCounter();
            List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.adminUser);
            String[] tempSequence = userList.Last().Remove(userList.Last().Length - 1, 1).Split(';');
            String[] sequence = new String[tempSequence.Length - 1];
            for (int j = 1; j < tempSequence.Length; j++)
            {
                sequence[j - 1] = tempSequence[j];
            }
            int i = 0;
            foreach (User user in allUsers)
            {
                i = 0;
                temp = new IdCounter();
                foreach (String userId in sequence)
                {
                    if (userId.Equals(user.userID))
                    {
                        i++;
                    }
                }
                temp.id = user.userID;
                temp.val = i;
                top3.Add(temp);
            }
            top3 = top3.OrderByDescending(x => x.val).ToList();
            String first = "None";
            String second = "None";
            String third = "None";
            String last = "None";
            String secondLast = "None";
            String thirdLast = "None";

            if (top3.Count == 1)
            {
                first = top3[0].id + " ~ " + top3[0].val;
                last = top3[0].id + " ~ " + top3[0].val;
            }
            else if (top3.Count == 2)
            {
                first = top3[0].id + " ~ " + top3[0].val;
                second = top3[1].id + " ~ " + top3[1].val;

                last = top3[top3.Count - 1].id + " ~ " + top3[top3.Count - 1].val;
                secondLast = top3[top3.Count - 2].id + " ~ " + top3[top3.Count - 2].val;
            }
            else
            {
                first = top3[0].id + " ~ " + top3[0].val;
                second = top3[1].id + " ~ " + top3[1].val;
                third = top3[2].id + " ~ " + top3[2].val;

                last = top3[top3.Count - 1].id + " ~ " + top3[top3.Count - 1].val;
                secondLast = top3[top3.Count - 2].id + " ~ " + top3[top3.Count - 2].val;
                thirdLast = top3[top3.Count - 3].id + " ~ " + top3[top3.Count - 3].val;
            }
            String[] month = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            String currentMonth = month[DateTime.Now.Month - 1];
            txtblkRecentBreakdown.Text = "Results for: " + currentMonth + "\n" + "Most Books Read by: \n1) " + first + "\n2) " + second + "\n3) " + third + "\n\nLeast Books Read by:\n1) " + last + "\n2) " + secondLast + "\n3) " + thirdLast + "\n\n";

            //top 3 and bottom 3 most popular books
            top3 = new List<IdCounter>();
            temp = new IdCounter();
            List<Book> allBooks = globalValues.xmlC.GetAvailableBooks(globalValues.xmlC.BookCompiler());
            String basicSequence = borrowedList.Last();
            basicSequence = basicSequence.Remove(basicSequence.Length - 1, 1);
            basicSequence = basicSequence.Substring(7, basicSequence.Length - 7);
            tempSequence = basicSequence.Split(';');
            sequence = tempSequence;
            i = 0;
            foreach (Book book in allBooks)
            {
                i = 0;
                temp = new IdCounter();
                foreach (String borrowedBookIsbn in sequence)
                {
                    if (borrowedBookIsbn.Equals(book.isbn))
                    {
                        i++;
                    }
                }
                temp.id = book.isbn;
                temp.val = i;
                top3.Add(temp);
            }
            top3.OrderBy(x => x.val).ToList();
            first = "None";
            second = "None";
            third = "None";

            last = "None";
            secondLast = "None";
            thirdLast = "None";

            if (top3.Count == 1)
            {
                first = top3[0].id + " ~ " + top3[0].val;
                last = top3[0].id + " ~ " + top3[0].val;
            }
            else if (top3.Count == 2)
            {
                first = top3[0].id + " ~ " + top3[0].val;
                second = top3[1].id + " ~ " + top3[1].val;

                last = top3[top3.Count - 1].id + " ~ " + top3[top3.Count - 1].val;
                secondLast = top3[top3.Count - 2].id + " ~ " + top3[top3.Count - 2].val;
            }
            else
            {
                first = top3[0].id + " ~ " + top3[0].val;
                second = top3[1].id + " ~ " + top3[1].val;
                third = top3[2].id + " ~ " + top3[2].val;

                last = top3[top3.Count - 1].id + " ~ " + top3[top3.Count - 1].val;
                secondLast = top3[top3.Count - 2].id + " ~ " + top3[top3.Count - 2].val;
                thirdLast = top3[top3.Count - 3].id + " ~ " + top3[top3.Count - 3].val;
            }
            txtblkRecentBreakdown.Text += "Most Popular Books: \n1) " + first + "\n2) " + second + "\n3) " + third + "\n\nLeast Popular Books:\n1) " + last + "\n2) " + secondLast + "\n3) " + thirdLast + "\n\n";

            //top 3 most fined
            allUsers.OrderByDescending(x => x.fine).ToList();

            first = "None";
            second = "None";
            third = "None";

            if (top3.Count == 1)
            {
                first = allUsers[0].userID + " ~ " + string.Format("{0:C}", allUsers[0].fine);
            }
            else if (top3.Count == 2)
            {
                first = allUsers[0].userID + " ~ " + string.Format("{0:C}", allUsers[0].fine);
                second = allUsers[1].userID + " ~ " + string.Format("{0:C}", allUsers[1].fine);
            }
            else
            {
                first = allUsers[0].userID + " ~ " + string.Format("{0:C}", allUsers[0].fine);
                second = allUsers[1].userID + " ~ " + string.Format("{0:C}", allUsers[1].fine);
                third = allUsers[2].userID + " ~ " + string.Format("{0:C}", allUsers[2].fine);
            }
            txtblkRecentBreakdown.Text += "Largest fined users (current): \n1) " + first + "\n2) " + second + "\n3) " + third;

            //all books displayed
            txtblkFullBreakdown.Text += "Book Inventory:\n";
            String reserved = "";
            String renewed = "";
            String due = "";
            foreach (Book book in globalValues.xmlC.BookCompiler())
            {
                if (!book.renewed)
                {
                    renewed = "NO";
                }
                else
                {
                    renewed = "YES";
                }
                if (book.reserved == DateTime.MinValue)
                {
                    reserved = "NO";
                }
                else
                {
                    reserved = "YES";
                }
                if (book.dueDate == DateTime.MinValue)
                {
                    due = "IN STOCK";
                }
                else
                {
                    due = "OUT OF STOCK";
                }
                txtblkFullBreakdown.Text += $"ID: {book.id} | ISBN: {book.isbn} | RESERVED: {reserved} | RENEWED: {renewed} | STOCK: {due}\n";
            }
            //populate the log in the detailed field
            txtblkFullBreakdown.Text += "\nLog:\n";
            lines = File.ReadAllLines(fullLog);
            foreach (String line in lines) 
            {
                txtblkFullBreakdown.Text += line + "\n";
            }
        }
    }
}
