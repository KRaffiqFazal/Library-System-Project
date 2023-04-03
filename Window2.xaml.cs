using System.Windows;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            Globals globalValues = new Globals();
            MainWindow win = new MainWindow(globalValues);
            win.Show();
            Close();
        }
    }
}