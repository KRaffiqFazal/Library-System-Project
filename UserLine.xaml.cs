using System;
using System.Collections.Generic;
using System.Linq;
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
