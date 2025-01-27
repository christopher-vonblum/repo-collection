using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace tcptest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            TcpListener l = new TcpListener(IPAddress.Any, 1234);
            l.Start();
            new Thread(ClientThread).Start();
            
            var socket = l.AcceptSocket();
        }

        private static void ClientThread()
        {
            TcpClient
        }
    }
}