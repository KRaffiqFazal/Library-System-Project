using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for ErrorLogger.xaml
    /// </summary>
    public partial class ErrorLogger : Window
    {
        Globals globalValues;
        String path = @"ErrorLog.txt";
        public ErrorLogger(Globals global)
        {
            InitializeComponent();
            globalValues = global;
            Start();

        }

        private void Start()
        {
            txtbxErrorEntry.MaxLength = 512;
            lblErrorLogConfirm.Visibility = Visibility.Hidden;
            txtbxErrorEntry.IsReadOnly = false;
            if (globalValues.currentUser.userType.Equals("admin"))
            {
                btnNext.Visibility = Visibility.Visible;
            }
            else
            {
                btnNext.Visibility = Visibility.Hidden;
            }
            scrlvwrReport.Visibility = Visibility.Hidden;
        }

        private void picLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Home();
        }
        private async void Home()
        {
            MemberPage window = new MemberPage(globalValues);
            window.Show();
            await Task.Delay(250);
            Close();
        }

        private async void picBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (btnNext.Visibility == Visibility.Hidden && globalValues.currentUser.userType.Equals("admin")) //an admin is on the error report
            {
                ErrorLogger window = new ErrorLogger(globalValues);
                window.Show();
                await Task.Delay(250);
                Close();
            }
            else
            {
                Home();
            }
        }

        private void txtbxErrorEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCharsLeft.Content = "Characters Left: " + (512 - txtbxErrorEntry.Text.Length);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxErrorEntry.Text.Equals(""))
            {
                lblErrorLogConfirm.Content = "Please enter an error message.";
                return;
            }
            btnSubmit.IsEnabled = false;
            //(Kanchan_Ray, 2020)

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(globalValues.currentUser.userID + " " + DateTime.Now + " " + txtbxErrorEntry.Text + Environment.NewLine);
            }

            lblErrorLogConfirm.Visibility = Visibility.Visible;
            lblErrorLogConfirm.Content = "<Error Reported>";
            txtbxErrorEntry.IsReadOnly = true;
            txtbxErrorEntry.Foreground = Brushes.Gray;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            lblCharsLeft.Visibility = Visibility.Hidden;
            lblErrorLogConfirm.Visibility = Visibility.Hidden;
            btnNext.Visibility = Visibility.Hidden;
            btnSubmit.Visibility = Visibility.Hidden;
            scrlvwrReport.Visibility = Visibility.Visible;
            txtbxErrorEntry.Visibility = Visibility.Hidden;

            String[] report = File.ReadAllLines(path);
            foreach (String reportItem in report)
            {
                txtblkErrorReport.Text += reportItem + Environment.NewLine;
            }
        }
    }
}
