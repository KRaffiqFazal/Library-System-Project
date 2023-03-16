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
using System.Windows.Shapes;

namespace Library_System //need to add the user control and update each value, kinda like the other one
{
    /// <summary>
    /// Interaction logic for SearchUsers.xaml
    /// </summary>
    public partial class SearchUsers : Window
    {
        public SearchUsers()
        {
            InitializeComponent();
        }

        private void txtbxWatermarked_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtbxSearch_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
