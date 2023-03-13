using Library_System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for BookDisplay.xaml
    /// </summary>
    public partial class BookDisplay : UserControl
    {
        public BookDisplay()
        {
            InitializeComponent();
            
        }
        public String BookInfo
        {
            get { return txtblkBookInfo.Text; }
            set { txtblkBookInfo.Text = value; }
        }
        public String BookIsbn { get; set; }
    }
}
