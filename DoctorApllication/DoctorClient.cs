using MessageStream;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApllication
{
    class DoctorClient
    {
        private int _port;
        private IPAddress _address;

        private static Socket _socket;

        public static bool IsRunning { get; private set; } = false;

        public DoctorClient(string ip, int port)
        {
            _address = IPAddress.Parse(ip);
            _port = port;
        }

        public static Dictionary<byte, List<ClientData>> clientData { get; set; } = new Dictionary<byte, List<ClientData>>();

        public void connect()
        {
            IPEndPoint endPoint = new IPEndPoint(_address, _port);
            _socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(endPoint);
                IsRunning = true;
                Console.WriteLine($"Connecting to{_socket.Connected}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception connecting to Doctor Client to server:\n{ex}");
            }
        }

        public static void Send(int BikeID)
        {
            switch (BikeID)
            {
                case 1:
                    //send message to connect to simulation bike
                    break;
                case 00438:
                    //message to connect to bike with this id 
                    break;
                    //do the same for alle other possible bikes 

            }

        }

        public static void Receive(MessageReader reader) 
        {
            switch (reader.Id) 
            {
                case 0x21:
                    byte identifier = reader.ReadByte();
                    decimal elapsedTime = reader.ReadInt(2) / 4m;
                    int distance = reader.ReadInt(2);
                    decimal speed = reader.ReadInt(2) / 1000m;
                    int heartRate = reader.ReadByte();
                    clientData.Add(identifier, new List<ClientData>() );
                    clientData[identifier].Add(new ClientData(elapsedTime, distance, speed, heartRate));
                    break;
            }
        }





    }
}
