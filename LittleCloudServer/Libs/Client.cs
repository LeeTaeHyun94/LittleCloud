using LittleCloudModels.Models;
using SocketManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudServer.Libs
{
    public class Client : SocketClient
    {
        private Member _member;
        /// <summary>
        /// 멤버의 정보를 나타냅니다.
        /// </summary>
        public Member Member
        {
            get { return _member; }
            set { _member = value; }
        }

    }
}
