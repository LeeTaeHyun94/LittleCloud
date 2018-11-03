using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SocketManager
{
    public class SocketServer : SocketServer<SocketClient> { }

    public class SocketServer<T> where T : SocketClient, new()
    {
        public List<T> m_ConnectedClient { get; private set; }

        private Socket m_ServerSocket = null;
        private AsyncCallback m_fnAcceptHandler;
        private delegate void ReceiveMessageHandler(object sender, AsyncObject e);
        
        public event Action<T> OnConnectedClient;
        public event AsyncCallback Receive;
        //public event ReceiveMessageHandler ReceiveMessage;

        public SocketServer()
        {
            m_ConnectedClient = new List<T>();
            m_fnAcceptHandler = new AsyncCallback(handleClientConnectionRequest);
        }

        /// <summary>
        /// 지정된 포트번호를 통해 서버를 실행합니다.
        /// </summary>
        /// <param name="port">포트번호</param>
        /// <exception cref="SocketException">소켓에 액세스하려고 시도하는 동안 오류가 발생한 경우</exception>
        /// <exception cref="System.ObjectDisposedException">System.Net.Sockets.Socket이 닫힌 경우</exception>
        /// <exception cref="System.Security.SecurityException">호출 스택에 있는 상위 호출자에게 요청된 작업에 대한 권한이 없는 경우</exception>
        public void StartServer(int port)
        {
            // TCP 통신을 위한 소켓을 생성합니다.
            m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            // 특정 포트에서 모든 주소로부터 들어오는 연결을 받기 위해 포트를 바인딩합니다.
            m_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));

            // 연결 요청을 받기 시작합니다.
            m_ServerSocket.Listen(5);

            // BeginAccept 메서드를 이용해 들어오는 연결 요청을 비동기적으로 처리합니다.
            // 연결 요청을 처리하는 함수는 handleClientConnectionRequest 입니다.
            m_ServerSocket.BeginAccept(m_fnAcceptHandler, null);
        }

        /// <summary>
        /// 서버를 닫습니다.
        /// </summary>
        public void StopServer()
        {
            // 가차없이 서버 소켓을 닫습니다.
            m_ServerSocket.Close();
        }

        /// <summary>
        /// 접속되어있는 모든 클라이언트에 데이터를 보냅니다.
        /// </summary>
        /// <param name="data">보낼 데이터입니다.</param>
        public void SendToAllMessage(Byte[] data)
        {
            // 전송 시작!
            foreach (SocketClient m_CC in m_ConnectedClient)
            {
                try
                {
                    m_CC.SendData(data);
                }
                catch
                {
                    m_CC.StopClient();
                }
            }
        }

        private void handleClientConnectionRequest(IAsyncResult ar)
        {
            T client;
            try
            {
                // 클라이언트의 연결 요청을 수락합니다.
                client = new T();
                client.ClientSocket = m_ServerSocket.EndAccept(ar);
                if (this.OnConnectedClient != null)
                    this.OnConnectedClient(client);
            }
            catch
            {
                return;
            }

            // 클라이언트 저장
            m_ConnectedClient.Add(client);
        }
    }
}
