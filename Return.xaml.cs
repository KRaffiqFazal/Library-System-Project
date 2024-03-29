﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Borrow.xaml
    /// </summary>
    public partial class Return : Window
    {
        private Globals globalValues;
        private bool txtChangedRun;

        public Return(Globals globals)
        {
            InitializeComponent();
            globalValues = globals;
            txtChangedRun = true;
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

        private void txtbxToBorrow_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxToReturn.Text))
            {
                txtbxToReturn.Visibility = Visibility.Collapsed;
                txtbxWatermarked.Visibility = Visibility.Visible;
            }
        }

        private void txtbxWatermarked_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbxWatermarked.Visibility = Visibility.Collapsed;
            txtbxToReturn.Visibility = Visibility.Visible;
            txtbxToReturn.Focus();
        }

        private async void btnReturn_Click(object sender, RoutedEventArgs e) //fix tomorrow
        {
            if (txtbxToReturn.Text.Equals(""))
            {
                lblError.Foreground = Brushes.Red;
                lblError.Content = "Error: No input detected.";
                await Task.Delay(3000);
                lblError.Content = "";
            }
            else if (globalValues.currentUser.borrowedBooks.FindIndex(book => book.id == txtbxToReturn.Text) != -1) //if the entered id exists in user's borrowed books it can be returned
            {
                Book toReturn = globalValues.currentUser.borrowedBooks.Find(book => book.id == txtbxToReturn.Text);
                if (toReturn.dueDate != DateTime.MinValue) //can be returned
                {
                    txtChangedRun = false;
                    if (globalValues.currentUser.CalculateFine(toReturn) > globalValues.currentUser.fine) //if the fine increases, inform user and add the fine
                    {
                        globalValues.currentUser.fine = globalValues.currentUser.CalculateFine(toReturn);
                        lblError.Foreground = Brushes.Black;
                        lblError.Content = "Book returned, fine due, please see a librarian.";
                        globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " has a fine of " + string.Format("{0:C}", globalValues.currentUser.fine));
                    }
                    else
                    {
                        lblError.Foreground = Brushes.Black;
                        lblError.Content = "Book returned, hope it was enjoyable!";
                    }
                    toReturn.dueDate = DateTime.MinValue; //back in stock
                    toReturn.renewed = false;
                    if (toReturn.reserved == DateTime.MaxValue) //is currently reserved by someone
                    {
                        //informs user who reserved book to obtain it within 3 days
                        toReturn.reserved = DateTime.Now.AddDays(3);
                        List<User> allUsers = globalValues.xmlC.GetAllUsers(globalValues.adminUser);
                        User update = allUsers.Find(user => user.reserved == toReturn.id);
                        update.notifications.Add(toReturn.title + " is back in stock, please see a librarian before " + toReturn.reserved.ToShortDateString() + ", to claim it.");
                        globalValues.xmlC.UpdateUserRecord(update);
                        globalValues.UpdateDetailedLog(update.userID + " reserved " + toReturn.id + " and it is reserved for them for 3 days");
                        globalValues.SendNotifications(update);
                    }
                    //clears field and informs the user that the book is reserved
                    txtbxToReturn.Text = "";
                    globalValues.currentUser.borrowedBooks.Remove(toReturn);
                    globalValues.xmlC.UpdateRecord(toReturn, false);
                    globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                    globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " returned " + toReturn.id);
                    await Task.Delay(3000);
                    txtChangedRun = true;
                    lblError.Content = "";
                }
                else
                {
                    txtChangedRun = false;
                    lblError.Foreground = Brushes.Red;
                    lblError.Content = "Error: This book has not been borrowed.";
                    await Task.Delay(3000);
                    txtChangedRun = true;
                    lblError.Content = "";
                }
            }
            else
            {
                lblError.Content = "Error: ID not found, please enter a valid book ID.";
            }
        }

        private async void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            if (txtbxToReturn.Text.Equals(""))
            {
                lblError.Content = "Error: No input detected.";
            }
            else if (globalValues.currentUser.borrowedBooks.FindIndex(book => book.id == txtbxToReturn.Text) != -1) //a borrowed book that the user possesses
            {
                Book toRenew = globalValues.currentUser.borrowedBooks.Find(book => book.id == txtbxToReturn.Text);
                if (globalValues.currentUser.CalculateFine(toRenew) > globalValues.currentUser.fine) //if they are different, a fine needs to take place so the book must be returned
                {
                    lblError.Content = "Please return this book as a fine is due.";
                }
                else if (!toRenew.renewed)
                {
                    globalValues.currentUser.borrowedBooks.Remove(toRenew); //remove old version
                    toRenew.dueDate = DateTime.Now.AddDays(30); //ensures renewed books do not get renewed 30 more days from new renewal date
                    toRenew.renewed = true;
                    globalValues.currentUser.borrowedBooks.Add(toRenew);
                    globalValues.xmlC.UpdateRecord(toRenew, true);
                    globalValues.xmlC.UpdateUserRecord(globalValues.currentUser);
                    globalValues.UpdateDetailedLog(globalValues.currentUser.userID + " has renewed " + toRenew.id);
                    lblError.Foreground = Brushes.Red;
                    txtChangedRun = false;
                    lblError.Content = "Book renewed successfully!";
                    await Task.Delay(3000);
                    txtChangedRun = true;
                    lblError.Content = "";
                }
                else
                {
                    lblError.Foreground = Brushes.Red;
                    txtChangedRun = false;
                    lblError.Content = "This book has already been renewed once and cannot be renewed again";
                    await Task.Delay(3000);
                    txtChangedRun = true;
                    lblError.Content = "";
                }
            }
            else
            {
                lblError.Content = "Error: ID not found, please enter a valid book ID.";
            }
        }
        /// <summary>
        /// Displays book title when id is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbxToReturn_TextChanged(object sender, TextChangedEventArgs e)
        {
            String id = txtbxToReturn.Text;
            if (globalValues.currentUser.borrowedBooks.FindIndex(book => book.id == id) != -1) //exists
            {
                lblError.Foreground = Brushes.Black;
                lblError.Content = globalValues.currentUser.borrowedBooks.Find(book => book.id == id).title;
            }
            else if (txtChangedRun)
            {
                lblError.Content = "";
            }
        }
    }
}