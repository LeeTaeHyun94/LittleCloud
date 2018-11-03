using System;
using System.Net.Sockets;

namespace SocketManager
{
    public class SocketClient
    {
        private Socket m_ClientSocket = null;
        private AsyncCallback m_fnReceiveHandler;
        private AsyncCallback m_fnSendHandler;

        public bool Connected
        {
            get { return this.m_ClientSocket.Connected; }
        }

        public Socket ClientSocket
        {
            get { return this.m_ClientSocket; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("socket", "The parameter socket can not be null.");
                this.m_ClientSocket = value;

                // 4096 바이트의 크기를 갖는 바이트 배열을 가진 AsyncObject 클래스 생성
                AsyncObject ao = new AsyncObject(4096);

                // 작업 중인 소켓을 저장하기 위해 sockClient 할당
                ao.WorkingSocket = m_ClientSocket;

                // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
                m_ClientSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
            }
        }

        public SocketClient()
        {
            m_fnReceiveHandler = new AsyncCallback(handleDataReceive);
            m_fnSendHandler = new AsyncCallback(handleDataSend);
        }

        public SocketClient(Socket socket) : this()
        {
            this.ClientSocket = socket;
        }

        public event Action<byte[]> OnReceiveData = null;

        /// <summary>
        /// Socket서버에 접속을 시도합니다.
        /// </summary>
        /// <param name="hostName">서버의 호스트주소입니다. url이름 혹은 아이피주소를 입력합니다.</param>
        /// <param name="hostPort">포트번호입니다.</param>
        /// <exception cref="System.Net.Sockets.SocketException">hostName, hostPort를 조합했을 때 소켓이 잘못된 경우</exception>
        public void ConnectToServer(string hostName, int hostPort)
        {
            // TCP 통신을 위한 소켓을 생성합니다.
            m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            // 연결 시도
            m_ClientSocket.Connect(hostName, hostPort);

            // 4096 바이트의 크기를 갖는 바이트 배열을 가진 AsyncObject 클래스 생성
            AsyncObject ao = new AsyncObject(4096);

            // 작업 중인 소켓을 저장하기 위해 sockClient 할당
            ao.WorkingSocket = m_ClientSocket;

            // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
            m_ClientSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
        }

        /// <summary>
        /// 서버와의 연결을 끊습니다.
        /// </summary>
        public void StopClient()
        {
            // 가차없이 클라이언트 소켓을 닫습니다.
            m_ClientSocket.Close();
        }

        /// <summary>
        /// 데이터를 전송합니다.
        /// </summary>
        /// <param name="data">전송할 데이터</param>
        /// <exception cref="System.ArgumentNullException">매개변수 data가 null인 경우</exception>
        /// <exception cref="System.Net.Sockets.SocketException">소켓에 액세스하려고 시도하는 동안 오류가 발생한 경우</exception>
        /// <exception cref="System.ObjectDisposedException">서버와 연결이 되어있지 않은경우</exception>
        public void SendData(byte[] data)
        {
            // 추가 정보를 넘기기 위한 변수 선언
            // 크기를 설정하는게 의미가 없습니다.
            // 왜냐하면 바로 밑의 코드에서 문자열을 유니코드 형으로 변환한 바이트 배열을 반환하기 때문에
            // 최소한의 크기르 배열을 초기화합니다.
            AsyncObject ao = new AsyncObject(1);

            // 문자열을 바이트 배열으로 변환
            ao.Buffer = data;

            ao.WorkingSocket = m_ClientSocket;

            // 전송 시작!
            m_ClientSocket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnSendHandler, ao);
        }

        private void handleDataReceive(IAsyncResult ar)
        {

            // 넘겨진 추가 정보를 가져옵니다.
            // AsyncState 속성의 자료형은 Object 형식이기 때문에 형 변환이 필요합니다~!
            AsyncObject ao = (AsyncObject)ar.AsyncState;

            // 받은 바이트 수 저장할 변수 선언
            int recvBytes;

            try
            {
                // 자료를 수신하고, 수신받은 바이트를 가져옵니다.
                recvBytes = ao.WorkingSocket.EndReceive(ar);
            }
            catch
            {
                // 예외가 발생하면 함수 종료!를 일단 하긴했는데.....
                // 여기도 처리를 해야하긴 하는데 단순히 throw했다가는 더이상 서버에서의 데이터를 받지 못한다.
                return;
            }

            // 수신받은 자료의 크기가 1 이상일 때에만 자료 처리
            if (recvBytes > 0)
            {
                // 공백 문자들이 많이 발생할 수 있으므로, 받은 바이트 수 만큼 배열을 선언하고 복사한다.
                Byte[] msgByte = new Byte[recvBytes];
                Array.Copy(ao.Buffer, msgByte, recvBytes);

                if (this.OnReceiveData != null)
                    this.OnReceiveData(msgByte);
            }

            // 자료 처리가 끝났으면~
            // 이제 다시 데이터를 수신받기 위해서 수신 대기를 해야 합니다.
            // Begin~~ 메서드를 이용해 비동기적으로 작업을 대기했다면
            // 반드시 대리자 함수에서 End~~ 메서드를 이용해 비동기 작업이 끝났다고 알려줘야 합니다!
            ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
        }
        private void handleDataSend(IAsyncResult ar)
        {

            // 넘겨진 추가 정보를 가져옵니다.
            AsyncObject ao = (AsyncObject)ar.AsyncState;

            // 보낸 바이트 수를 저장할 변수 선언
            Int32 sentBytes;

            try
            {
                // 자료를 전송하고, 전송한 바이트를 가져옵니다.
                sentBytes = ao.WorkingSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                throw ex;
            }

            if (sentBytes > 0)
            {
                // 여기도 마찬가지로 보낸 바이트 수 만큼 배열 선언 후 복사한다.
                Byte[] msgByte = new Byte[sentBytes];
                Array.Copy(ao.Buffer, msgByte, sentBytes);
            }
        }
    }
}
