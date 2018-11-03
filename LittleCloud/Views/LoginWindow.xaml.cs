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

namespace LittleCloud.Views
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ICommand cmd = LoginButton.Command as ICommand;
            if (cmd != null && cmd.CanExecute(null))
            {
                //PreExecute stuff goes here

                cmd.Execute(null);
                if(cmd.CanExecute(null)) MessageBox.Show("올바른 ID와 Password를 입력해주세요.");
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                //Executed stuff goes here
            }            
        }
    }
}
