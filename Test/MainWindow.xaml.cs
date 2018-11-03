using LittleCloudServer.Libs;
using LittleCloudServer.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //ObservableCollection<LittleCloudModels.Models.Member> list = new ObservableCollection<LittleCloudModels.Models.Member>();
            //list.Add(new LittleCloudModels.Models.Member()
            //{
            //    IsLogin = true,
            //    Passwd = "asdf",
            //    UserID = "asdfasdf"
            //});
            //list.Add(new LittleCloudModels.Models.Member()
            //{
            //    IsLogin = true,
            //    Passwd = "asdf",
            //    UserID = "asdfasdf"
            //});

            //var test = Libraries.XmlSerializer.Serialize<ObservableCollection<LittleCloudModels.Models.Member>>(list);

            //var test2 = Libraries.XmlSerializer.Deserial<ObservableCollection<LittleCloudModels.Models.Member>>(test);

            //var test = LittleCloudServer.Models.DAO.getFriends();

            //ObservableCollection<LittleCloudModels.Models.Member> list = new ObservableCollection<LittleCloudModels.Models.Member>();
            //list.Add(new LittleCloudModels.Models.Member()
            //{
            //    IsLogin = true,
            //    Passwd = "asdf",
            //    UserID = "1"
            //});
            //list.Add(new LittleCloudModels.Models.Member()
            //{
            //    IsLogin = true,
            //    Passwd = "asdf",
            //    UserID = "2"
            //});

            //DAO.StartChat(list);

            InitializeComponent();

            Client client = new Client();

            //client.ConnectToServer("localhost", 64090);



            client.SendData(new byte[123111011]);

        }
    }
}
