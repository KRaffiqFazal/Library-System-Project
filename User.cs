using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System
{
    public class User
    {
        public String userID { get; set; }
        public String userType { get; set; } //determined by first 2 digits in the user ID
        public List<Book> borrowedBooks { get; set; }

        public User(String id)
        {
            userID = id;
            if (userID[0] == '1')
            {
                userType = "member";
            }
            else if (userID[1] == '2')
            {
                userType = "librarian";
            }
            else
            {
                userType = "admin";
            }
        }
    }
}
