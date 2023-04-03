using System;
using System.Windows.Controls;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for UserLine.xaml
    /// </summary>
    public partial class UserLine : UserControl
    {
        public UserLine()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Retrieves and sets user details to and from the text block
        /// </summary>
        public String[] UserInfo
        {
            get
            {
                String[] fields = new String[5];
                fields[0] = txtblkId.Text;
                fields[1] = txtblkName.Text;
                fields[2] = txtblkEmail.Text;
                fields[3] = txtblkPhone.Text;
                fields[4] = txtblkFine.Text;
                return fields;
            }
            set
            {
                txtblkId.Text = value[0];
                txtblkName.Text = value[1];
                txtblkEmail.Text = value[2];
                txtblkPhone.Text = value[3];
                txtblkFine.Text = value[4];
            }
        }
    }
}