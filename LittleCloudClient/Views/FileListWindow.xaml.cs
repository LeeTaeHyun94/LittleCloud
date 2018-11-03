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
    /// FileListWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FileListWindow : Window
    {
        public FileListWindow()
        {
            InitializeComponent();
        }

        public FileListWindow(ChatViewModel viewmodel)
        {
            this.Resources.Add("ChatViewModel", viewmodel);
            this.InitializeComponent();
        }
    }
}
