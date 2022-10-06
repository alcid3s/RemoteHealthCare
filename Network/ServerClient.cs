using MessageStream;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RemoteHealthCare.Network
{
    internal class ServerClient
    {
        private int _port;
        private IPAddress _address;

        private static Socket _socket;

        public static bool IsRunning { get; private set; } = false;

        internal static byte Reply = 0x00;

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

                new Thread(Listen).Start();
                IsRunning = true;
                Console.WriteLine($"Connecting to {_socket.RemoteEndPoint}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception connecting Client to server:\n{e}");
            }
        }

        public void Listen()
        {
            while (true)
            {
                byte[] message = new byte[1024];
                int receive = _socket.Receive(message);
                Console.WriteLine("Received data");
                MessageReader reader = new MessageReader(message);
                Reply = reader.Id;

                Console.WriteLine($"Login: {(reader.Id == 0x81 ? "Successfull" : "No")}");
              
            }
        }

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

        public static void Send(byte[] message)
        {
            MessageReader reader = new MessageReader(message);
            switch (reader.Id)
            {
                case 0x10:
                    Console.WriteLine("Creating account");
                    break;
                case 0x11:
                    Console.WriteLine("Logging in");
                    break;
            }
            int received = _socket.Send(message);
        }
    }
}
