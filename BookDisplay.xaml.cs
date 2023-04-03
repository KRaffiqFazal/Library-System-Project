using System;
using System.Windows.Controls;

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
        //allows book information to be set and allows ISBN to be retrieved to identify the book information
        public String BookInfo
        {
            get { return txtblkBookInfo.Text; }
            set { txtblkBookInfo.Text = value; }
        }

        public String BookIsbn { get; set; }
    }
}