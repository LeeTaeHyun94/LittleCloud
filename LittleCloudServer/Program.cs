using SocketManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketServer server = new SocketServer();
            server.OnConnectedClient += (client) =>
            {
                client.OnReceiveData += (data) =>
                {
                    Console.WriteLine(Encoding.Unicode.GetString(data));
                };
            };

            server.StartServer(15937);

            while (true)
            {
                server.SendToAllMessage(Encoding.Unicode.GetBytes(Console.ReadLine()));
            }
        }
    }
}
