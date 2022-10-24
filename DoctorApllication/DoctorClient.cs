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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DoctorApllication
{
    class DoctorClient
    {
        private int _port;
        private IPAddress _address;
        private static Socket _socket;
        public static bool IsRunning { get; private set; } = false;
        public static List<string> accounts { get; set; }
        public static Dictionary<byte, List<ClientData>> ClientDataList { get; set; } = new Dictionary<byte, List<ClientData>>();
        public static int Reply { get; private set; }

        public struct ClientData
        {
            public decimal elapsedTime { get; set; }
            public int distance { get; set; }
            public decimal speed { get; set; }
            public int heartRate { get; set; }

            public ClientData(decimal elapsedTime, int distance, decimal speed, int heartRate)
            {
                this.elapsedTime = elapsedTime;
                this.distance = distance;
                this.speed = speed;
                this.heartRate = heartRate;
            }
        }

        public DoctorClient(string ip, int port)
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
                Console.WriteLine($"Connecting to{_socket.Connected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception connecting to Doctor Client to server:\n{ex}");
            }
        }

        /// <summary>
        /// constantly listening to reply from the server and rewrites reply to the reply Id from the server
        /// </summary>
        private void Listen()
        {
            byte tempId = 0x00;
            while (true)
            {
                byte[] message = new byte[1024];
                int receive = _socket.Receive(message);
                Console.WriteLine("received message");
                try
                {
                    ExtendedMessageReader reader = new ExtendedMessageReader(message);
                    byte id = reader.Id;

                    switch (id)
                    {
                        // Doctor receives all registered accounts;
                        case 0x51:
                            Console.WriteLine("Received 0x51");
                            string name = Encoding.UTF8.GetString(reader.ReadPacket());
                            LoadDataScreen.FillIndex(name);
                            break;
                        case 0x53:
                            Console.WriteLine("Received 0x53");
                            string sessionName = Encoding.UTF8.GetString(reader.ReadPacket());
                            int size = reader.ReadByte();
                            LoadDataScreen.FillSessions(sessionName, size);
                            break;
                        case 0x43:
                            DoctorLogin.doctorScreen.addClient(reader.ReadByte(), reader.ReadString());
                            break;
                        case 0x21:
                            Console.WriteLine("bike data: " +reader.ReadByte() + " " + reader.ReadInt(2) + " " + reader.ReadInt(2) + " " + reader.ReadInt(2) + " " + reader.ReadInt(1));

                            break;

                    }

                    // Used only for login now.
                    if(id == 0x80 || id == 0x81)
                    {
                        // Request sent by the doctor client.
                        byte originalRequest = reader.ReadByte();

                        // Check what the server sends to the doctor (mainly used for login)
                        switch (originalRequest)
                        {
                            // Server replies with 0x14 if doctor wants to create account.
                            case 0x14:
                                
                                DoctorLoginCreation.Succes = id;
                                break;

                            // Server replies with 0x15 if the Doctor wants to login. Id will be 0x80 or 0x81.
                            case 0x15:
                                Program.doctor.login(id);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        //public static void sendHistoryRequest(byte id, string s)
        //{
        //    MessageWriter writer = new MessageWriter(id);
        //    switch (id)
        //    {
        //        case 0x52:
        //            //send request for all sessions of specific user 
        //            writer.WritePacket(Encoding.UTF8.GetBytes(s));
        //            break;
        //        case 0x54:
        //            //opvragen details session
        //            writer.WritePacket(Encoding.UTF8.GetBytes(s));
        //            break;
        //    }
        //}

        //public static void Receive(MessageReader reader)
        //{
        //    switch (reader.Id)
        //    {
        //        //Receive information about a client
        //        case 0x21:
        //            byte identifier = reader.ReadByte();            // requires to be different for every client, however this id is the id of the bicycle.
        //            decimal elapsedTime = reader.ReadInt(2) / 4m;
        //            int distance = reader.ReadInt(2);
        //            decimal speed = reader.ReadInt(2) / 1000m;
        //            int heartRate = reader.ReadByte();
        //            ClientDataList.Add(identifier, new List<ClientData>());
        //            ClientDataList[identifier].Add(new ClientData(elapsedTime, distance, speed, heartRate));
        //            break;

        //        case 0x12:
        //            break;

        //        case 0x51:
        //            string account = Encoding.UTF8.GetString(reader.ReadPacket());
        //            accounts.Add(account);
        //            break;

        //        case 0x53:
        //            //server sends all sessions

        //            break;
        //    }
        //}

        public static void Send(byte[] message)
        {
            int received = _socket.Send(message);
        }

        //wait until a new server message has been received
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
