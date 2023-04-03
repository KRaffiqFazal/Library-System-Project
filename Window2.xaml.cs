using System.Windows;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        /// <summary>
        /// Runs once when program starts (once each day) and emails users that it needs to wih this information
        /// </summary>
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