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

        public User()
        {
            if (userID[0] == '1' && userID[1] == '0') //change this to find parent of xml file
            {
                userType = "member";
                //set borrowedBooks by looking at xml file elements
            }
            else if (userID[0] == '2' && userID[1] == '1')
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
