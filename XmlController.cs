using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Library_System
{
    public class XmlController
    {
        String userPath = "Users.xml"; //borrowed books displayed with "book_id,date_borrowed;book_id2"
        String bookPath = "Library.xml";

        public List<String> ReturnMembers()
        { 
            List<String> users = new List<String>();
            XmlDocument doc = new XmlDocument();
            doc.Load(userPath);
            XmlElement idFinder;
            foreach (XmlNode node in doc.SelectNodes("users/members")) //loop through each node in members and get its id
            {
                idFinder = doc.DocumentElement;
                idFinder = 
                users.Add(node.SelectSingleNode("user"))
            }
            doc.SelectSingleNode("//users/members/user")
        }

        public List<String> ReturnLibrarians()
        { 
        
        }

        public List<String> ReturnAdmins()
        { 
            
        }
    }
}
