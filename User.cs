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
        public String name { get; set; }
        public String userType { get; set; } //determined by first 2 digits in the user ID
        public String phoneNumber { get; set; }
        public String email { get; set; }
        public List<Book> borrowedBooks { get; set; }
        public List<String> notifications { get; set; }

        public User(String id)
        {
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
        }
    }
}
