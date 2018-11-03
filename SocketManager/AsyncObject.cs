using System;
using System.Net.Sockets;

namespace SocketManager
{
    public class AsyncObject
    {
        /// <summary>
        /// 버퍼
        /// </summary>
        public Byte[] Buffer { get; set; }

        /// <summary>
        /// 실제 활용되는 Socket의 인스턴스
        /// </summary>
        public Socket WorkingSocket { get; set; }
        public string ID { get; set; }

        /// <summary>
        /// Socket.AsyncObject의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="bufferSize">버퍼 사이즈</param>
        public AsyncObject(int bufferSize)
        {
            this.Buffer = new Byte[bufferSize];
        }
    }
}
