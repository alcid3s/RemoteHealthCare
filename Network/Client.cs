using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthCare.Network
{
    internal class Client
    {
        private IPEndPoint _endPoint;
        private Socket _socket;

        public Client(string ip, int port)
        {
            if (ip == null || port < 1000)
                throw new MissingFieldException("IP is null or port is already in use");

            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.Connect(_endPoint);
                Console.WriteLine($"Connection with {_endPoint.Address}:{_endPoint.Port}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"This is an exception concerning the connection with the server:\n{e.ToString()}");
            }
        }
    }
}
