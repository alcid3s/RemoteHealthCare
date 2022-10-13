using MessageStream;
using RemoteHealthCare.GUI;
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

        public static List<string> accounts { get; set; }

        public DoctorClient(string ip, int port)
        {
            _address = IPAddress.Parse(ip);
            _port = port;
        }

        public static Dictionary<byte, List<ClientData>> clientData { get; set; } = new Dictionary<byte, List<ClientData>>();
        public static int Reply { get; internal set; }

        public void connect()
        {
            IPEndPoint endPoint = new IPEndPoint(_address, _port);
            _socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(endPoint);
                new Thread(Listen).Start();
                IsRunning = true;
                Console.WriteLine($"Connecting to{_socket.Connected}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception connecting to Doctor Client to server:\n{ex}");
            }
        }
        /// <summary>
        /// constantly listening to reply from the server and rewrites reply to the reply Id from the server
        /// </summary>
        public void Listen()
        {
            byte tempId = 0x00;
            while (true)
            {
                byte[] message = new byte[1024];
                int receive = _socket.Receive(message);
                try
                {
                    MessageReader reader = new MessageReader(message);
                    Reply = reader.Id;

                } 
                catch (Exception ex)
                {
                    continue;
                }

            }
        }

        public static void Send(byte id)
        {
            MessageWriter writer = new MessageWriter(id);
            switch (id)
            {
                case 0x50:  
                    //send message to connect to simulation bike
                    break;
                case 0x20:
                    //message to connect to bike with this id 
                    break;
                    //do the same for alle other possible bikes 

            }

        }
        public static void sendHistoryRequest(byte id, string s)
        {
            MessageWriter writer = new MessageWriter(id);
            switch (id)
            {
                case 0x52:
                    //send request for all sessions of specific user 
                    writer.WritePacket(Encoding.UTF8.GetBytes(s));
                    break;
                case 0x54:
                    //opvragen details session
                    writer.WritePacket(Encoding.UTF8.GetBytes(s));
                    break;
            }
        }

        public static void Receive(MessageReader reader) 
        {
            switch (reader.Id) 
            {
                //Receive information about a client
                case 0x21:
                    byte identifier = reader.ReadByte();
                    decimal elapsedTime = reader.ReadInt(2) / 4m;
                    int distance = reader.ReadInt(2);
                    decimal speed = reader.ReadInt(2) / 1000m;
                    int heartRate = reader.ReadByte();
                    clientData.Add(identifier, new List<ClientData>() );
                    clientData[identifier].Add(new ClientData(elapsedTime, distance, speed, heartRate));
                    break;

                case 0x12: 
                    break;

                case 0x51:
                    string account = Encoding.UTF8.GetString(reader.ReadPacket());
                    accounts.Add(account);
                        break;

                case 0x53:
                    //server sends all sessions
                    break;
            }
        }

        internal static void Send(byte[] message)
        {
            MessageReader reader = new MessageReader(message);
            switch (reader.Id)
            {
                
                case 0x14:
                    //creating account
                    break;
                
                case 0x15:
                    //loging in to account
                    break;
            }
            int received = _socket.Send(message);
        }

        public static bool waitForReply()
        {
            int counter = 0;
            DoctorClient.Reply = 0x00;
            while (DoctorClient.Reply == 0x00)
            {
                Thread.Sleep(100);
                counter++;
                if (counter == 50)
                {
                    //response takes too long
                    return false;
                }
            }

            //Got a response
            return true;
        }
    }
}
