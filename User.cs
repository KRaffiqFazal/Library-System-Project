using System;
using System.Collections.Generic;

namespace Library_System
{
    public class User
    {
        public String userID { get; set; }
        public String name { get; set; }
        public String userType { get; set; } //determined by first 2 digits in the user ID
        public String phoneNumber { get; set; }
        public String email { get; set; }

        public List<Book> borrowedBooks { get; set; }
        public List<String> notifications { get; set; }
        public String reserved { get; set; }
        public Decimal fine { get; set; }

        public User(String id)
        {
            fine = 0;
            userID = id;
            if (userID[0] == '1')
            {
                userType = "member";
            }
            else if (userID[0] == '2')
            {
                userType = "librarian";
            }
            else if (userID[0] == '3')
            {
                userType = "admin";
            }
            notifications = new List<String>();
            borrowedBooks = new List<Book>();
        }
        /// <summary>
        /// Fine that is based off borrowed books that have not been returned
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public Decimal CalculateFine(Book book)
        {
            Decimal tempFine = fine; //does not change actual fine value
            if (book.dueDate < DateTime.Now) //give fine
            {
                int weeks = Convert.ToInt32((DateTime.Now - book.dueDate).TotalDays / 7);
                weeks--; //grace period
                if (weeks >= 1)
                {
                    tempFine += book.price * (Decimal)0.01 * weeks;
                }
            }
            return tempFine;
        }
        /// <summary>
        /// Returns true if the user has an overdue book
        /// </summary>
        /// <returns>true, false</returns>
        public bool Overdue()
        {
            foreach (Book book in borrowedBooks)
            {
                if (book.dueDate < DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }
    }
}