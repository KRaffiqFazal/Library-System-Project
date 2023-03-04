using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    public partial class MainWindow : Window
    {
        
        //usage in MainWindow.xaml
        Stopwatch stopwatch;
        int attempts;
        String terminalPassword = "Admin123";
        Globals globalValues;
        
        public MainWindow()
        {
            InitializeComponent();
            globalValues = new Globals();
            Hide2();
        }

        private void Hide1()
        {
            btnProceed.Visibility = Visibility.Hidden;
            btnQuit.Visibility = Visibility.Hidden;
            picLogo.Visibility = Visibility.Hidden;
        }
        private void Hide2() 
        {
            btnAdminQuit.Visibility = Visibility.Hidden;
            btnRestart.Visibility = Visibility.Hidden;
            txtbxWatermarkedText.Visibility = Visibility.Hidden;
            pswdbxTerminalPassword.Visibility = Visibility.Hidden;
            lblUserHelp.Visibility = Visibility.Hidden;
            attempts = 3;
        }
        private void Reveal1()
        {
            btnProceed.Visibility = Visibility.Visible;
            btnQuit.Visibility = Visibility.Visible;
            picLogo.Visibility = Visibility.Visible;
        }
        private void Reveal2() 
        {
            btnAdminQuit.Visibility = Visibility.Visible;
            btnRestart.Visibility = Visibility.Visible;
            txtbxWatermarkedText.Visibility = Visibility.Visible;
            pswdbxTerminalPassword.Visibility = Visibility.Visible;
            lblUserHelp.Visibility = Visibility.Visible;
        }
        private async void btnProceed_Click(object sender, RoutedEventArgs e)
        {

            Window1 win = new Window1(globalValues);
            win.Show();
            await Task.Delay(250);
            Close();

        }

        /// <summary>
        /// https://stackoverflow.com/questions/26682729/event-for-when-a-button-is-held-clicked-for-some-time
        /// </summary>
        private void btnQuit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        private void btnQuit_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds >= 2000) //Admin quit function change to 5000
            {
                Hide1();
                Reveal2();
                
            }
        }

        /// <summary>
        /// https://www.youtube.com/watch?v=YPwnBJod5a8
        /// </summary>
        private void txtbxWatermarkedText_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarkedText.Visibility = Visibility.Collapsed;
            pswdbxTerminalPassword.Visibility = Visibility.Visible;
            pswdbxTerminalPassword.Focus();
        }

        private void pswdbxTerminalPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pswdbxTerminalPassword.Password))
            {
                pswdbxTerminalPassword.Visibility = Visibility.Collapsed;
                txtbxWatermarkedText.Visibility = Visibility.Visible;
            }
        }

        private void Restart()
        {
            Hide2();
            Reveal1();
        }
        /// <summary>
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.keydown?view=windowsdesktop-7.0
        /// </summary>
        private void PasswordEnter()
        {
                if (pswdbxTerminalPassword.Password.Equals(terminalPassword))
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    attempts--;
                    if (attempts <= 0)
                    {
                        Restart();
                    }
                    pswdbxTerminalPassword.Password = null;
                }
        }
        private void btnAdminQuit_Click(object sender, RoutedEventArgs e)
        {
            PasswordEnter();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard.keydown?view=windowsdesktop-7.0
        /// </summary>
        private void pswdbxTerminalPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Enter) 
            {
                PasswordEnter();
            }
        }
    }
}
