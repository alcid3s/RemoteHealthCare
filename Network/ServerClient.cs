using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RemoteHealthCare.Network
{
    internal class ServerClient
    {
        private int _port;
        private IPAddress _address;

        private Socket _socket;

        public ServerClient(string ip, int port)
        {
            _address = IPAddress.Parse(ip);
            _port = port;
        }

        public void Connect()
        {
            IPEndPoint endPoint = new IPEndPoint(_address, _port);
            _socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(endPoint);
                Console.WriteLine($"Connecting to {_socket.RemoteEndPoint}");

                byte[] message = Encoding.ASCII.GetBytes("TESTING");
                int sent = _socket.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception connecting Client to server:\n{e}");
            }
        }


    }
}
