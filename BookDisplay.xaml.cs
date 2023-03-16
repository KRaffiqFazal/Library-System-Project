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
        public String BookInfo
        {
            get { return txtblkBookInfo.Text; }
            set { txtblkBookInfo.Text = value; }
        }
        public String BookIsbn { get; set; }
    }
}
