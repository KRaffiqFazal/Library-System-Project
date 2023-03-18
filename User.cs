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
            userID = id;
            fine = 0;
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
        public String CalculateFineString()
        {
            if (CalculateFine() == 0)
            {
                return "No Fine";
            }
           
            return string.Format("{0:C}", CalculateFine());

        }
        public Decimal CalculateFine()
        {
            int weeks;
            foreach (Book borrowed in borrowedBooks)
            {
                if (borrowed.dueDate < DateTime.Now) //give fine
                {
                    weeks = Convert.ToInt32((DateTime.Now - borrowed.dueDate).TotalDays / 7);
                    weeks--; //grace period
                    if (weeks >= 1)
                    {
                        fine += borrowed.price * (Decimal)0.01 * weeks;
                    }
                }
            }
            return fine;
        }
    }
}
