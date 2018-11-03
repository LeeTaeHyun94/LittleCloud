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
    /// LoginWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void id_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (id.Text == "아이디")
            {
                id.Text = "";
                id.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        private void id_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (id.Text == "")
            {
                id.Text = "아이디";
                id.Foreground = new SolidColorBrush(Color.FromRgb(125, 125, 125));
            }

        }

        private void passwd_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (passwd.Password == "비밀번호")
            {
                passwd.Password = "";
                passwd.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        private void passwd_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (passwd.Password == "")
            {
                passwd.Password = "비밀번호";
                passwd.Foreground = new SolidColorBrush(Color.FromRgb(125, 125, 125));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string result = (sender as TextBox).Text;
            if (result == "OK")
            {
                this.DialogResult = true;
                this.Close();
            }
            else if (result == "Fail")
            {
                (sender as TextBox).Text = "F";
                MessageBox.Show("아이디 또는 비밀번호를 확인해주세요.");
            }
            else if(result == "Already Logined")
            {
                (sender as TextBox).Text = "F";
                MessageBox.Show("이미 로그인되어 있는 계정입니다.");
            }
            else if(result == "no Connect")
            {
                (sender as TextBox).Text = "F";
                MessageBox.Show("서버와 연결되지 않았습니다. 다시 접속해주세요.");
            }
        }
    }
}
