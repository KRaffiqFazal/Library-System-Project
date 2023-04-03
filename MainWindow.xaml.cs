using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Library_System
{
    public partial class MainWindow : Window
    {
        //usage in MainWindow.xaml
        private Stopwatch stopwatch;

        private int attempts;
        private String terminalPassword = "Admin123";
        private Globals globalValues;

        public MainWindow(Globals globals)
        {
            InitializeComponent();
            if (globals != null) //runs on startup of program, i.e. once a day
            {
                globalValues = new Globals();
                globalValues.xmlC = new XmlController();
                globalValues.NotifyUsers();
            }
            globalValues = new Globals();
            globalValues.xmlC = new XmlController();
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
        /// Opens a screen when a button is held for longer than 2 seconds (heltonbiker, 2015)
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
        /// Textbox reads what it requires to be inputted until it is clicked (GameDevMadeEasy Live, 2018)
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
                pswdbxTerminalPassword.Password = "";
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
        /// Allows user to press enter to proceed (dotnet-bot, 2023b)
        /// </summary>
        private void pswdbxTerminalPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PasswordEnter();
            }
        }
    }
}