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

namespace Library_System
{
    /// <summary>
    /// Interaction logic for SearchBooks.xaml
    /// </summary>
    public partial class SearchBooks : Window
    {
        Globals globalValues;
        public SearchBooks(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
        }

        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private void picBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }

        private void txtbxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtbxSearch.Text))
            {
                txtbxSearch.Visibility = Visibility.Collapsed;
                txtbxWatermarkedText.Visibility = Visibility.Visible;
            }
        }

        private void txtbxWatermarkedText_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarkedText.Visibility = Visibility.Collapsed;
            txtbxSearch.Visibility = Visibility.Visible;
            txtbxSearch.Focus();
        }
    }
}
