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

        /// <summary>
        /// Connects the server client to the address and port given in the constructor
        /// </summary>
        public void Connect()
        {
            IPEndPoint endPoint = new IPEndPoint(_address, _port);
            _socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(endPoint);
                Console.WriteLine($"Connecting to {_socket.RemoteEndPoint}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception connecting Client to server:\n{e}");
            }
        }

        /// <summary>
        /// Sends a message with the given params as content
        /// </summary>
        /// <param name="id">Id of the bike</param>
        /// <param name="elapsedTime">The elapsed time since the beginning of the exercise</param>
        /// <param name="distanceTravelled">The total travelled distance since the beginning of the exercise</param>
        /// <param name="speed">The current speed of the bike</param>
        /// <param name="heartRate">The current heart rate of the patient</param>
        public void Send(byte id, decimal elapsedTime, int distanceTravelled, decimal speed, int heartRate)
        {
            short elapsedTimeByte = (((short)Math.Round(elapsedTime * 4)));
            short speedByte = (short)(Math.Round(speed * 1000));
            byte[] message = { id, 
                (byte) (elapsedTimeByte & 0xFF), (byte) (elapsedTimeByte >> 8), 
                (byte) (distanceTravelled & 0xFF), (byte) ((distanceTravelled >> 8) & 0xFF), 
                (byte) (speedByte & 0xFF), (byte) (speedByte >> 8), 
                (byte) heartRate};
            int received = _socket.Send(message);

        }
    }
}
