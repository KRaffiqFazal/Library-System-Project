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
    /// Interaction logic for MemberPage.xaml
    /// </summary>
    public partial class MemberPage : Window
    {
        Globals globalValues;
        public MemberPage(Globals global)
        {
            InitializeComponent();
            globalValues = global;
            txtblkLogoutMessage.Visibility = Visibility.Hidden;
            txtblkTitle.Text = "Welcome: " + globalValues.currentUser.name;
        }

        private async void picLogout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            globalValues = null;
            txtblkLogoutMessage.Margin = new Thickness(0, 0, 0, 0);
            txtblkLogoutMessage.Visibility= Visibility.Visible;
            await Task.Delay(2000);
            MainWindow window = new MainWindow();
            window.Show();
            await Task.Delay(250);
            Close();
        }
    }
}
