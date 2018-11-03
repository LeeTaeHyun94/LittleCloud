using LittleCloudClient.ViewModels;
using LittleCloudModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// ChatWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatWindow : Window
    {

        public ObservableCollection<Member> Members
        {
            get { return (ObservableCollection<Member>)GetValue(MembersProperty); }
            set { SetValue(MembersProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Sample.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MembersProperty =
            DependencyProperty.Register(
                "Members",
                typeof(ObservableCollection<Member>),
                typeof(ChatWindow),
                new FrameworkPropertyMetadata()
                {
                    PropertyChangedCallback = MembersPropertyChangedCallback,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        protected static void MembersPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public int ChatNum
        {
            get
            {
                return (this.DataContext as ChatViewModel).ChatNum;
            }
            set
            {
                (this.DataContext as ChatViewModel).ChatNum = value;
            }
        }

        public ChatWindow()
        {
            InitializeComponent();
        }

        private void SV_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            SV.ScrollToBottom();
        }

        private void fileListButton_Click(object sender, RoutedEventArgs e)
        {
            FileListWindow flw = new FileListWindow(this.DataContext as ChatViewModel);
            flw.Show();
        }

        private void fileSendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "모든 파일(*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                button.CommandParameter = openFileDialog.FileName;
            }
        }

        private void addFriendButton_Click(object sender, RoutedEventArgs e)
        {
            FriendListWindow fw = new FriendListWindow(this.DataContext as ChatViewModel);
            fw.Show();
        }
    }
}
