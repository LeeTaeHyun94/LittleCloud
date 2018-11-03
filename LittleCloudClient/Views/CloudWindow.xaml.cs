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
    /// CloudWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CloudWindow : Window
    {
        public CloudWindow()
        {
            InitializeComponent();
        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "모든 파일(*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                button.CommandParameter = openFileDialog.FileName;
            }
        }
    }
}
