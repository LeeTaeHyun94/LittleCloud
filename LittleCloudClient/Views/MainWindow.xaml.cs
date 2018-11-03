using LittleCloudClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LittleCloudClient.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            LoginWindow login = new LoginWindow();
            if (login.ShowDialog() == true)
            {
                InitializeComponent();
            }
            else
            {
                Application.Current.Shutdown();
            }

            (this.Resources["MainViewModel"] as MainViewModel).OnChatStart += (sender, e) =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ChatWindow chatWindow = new ChatWindow();
                    chatWindow.Members = e.Friends;
                    chatWindow.ChatNum = e.ChatNum;
                    chatWindow.Show();
                }));
            };
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            MainWindow newMainWindow = new MainWindow();
            newMainWindow.Show();
            this.Close();
        }

        private void ListViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as ListViewItem).IsSelected = !(sender as ListViewItem).IsSelected;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //do my stuff before closing
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void CloudButton_Click(object sender, RoutedEventArgs e)
        {
            CloudWindow cw = new CloudWindow();
            cw.Show();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
