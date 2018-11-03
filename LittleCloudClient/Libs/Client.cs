using Libraries;
using LittleCloudClient.Models;
using LittleCloudModels.Models;
using SocketManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LittleCloudClient.Libs
{
    public class Client : SocketClient
    {
        public Member Member { get; set; }

        private static Client _client = null;
        /// <summary>
        /// 클라이언트의 인스턴스를 가져오거나 설정합니다.
        /// </summary>
        public static Client Instance
        {
            get
            {
                return _client == null ? _client = new Client() : _client;
            }
        }

        private Client()
        {
            var setting = XmlSerializer.Deserial<Setting>(new StreamReader("Setting.xml").ReadToEnd());
            ConnectToServer(setting.host, setting.port);
        }

        public void sendData(CommunicationObject data)
        {
            SendData(Encoding.UTF8.GetBytes(data.ToString()));
        }
    }
}
