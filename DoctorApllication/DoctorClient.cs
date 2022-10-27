using DoctorApplication;
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

        /// <summary>
        /// Connect to the server
        /// </summary>
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
            new EncryptionManager(false);

            List<MessageWriter> writers = MessageWriter.WriteRsa();
            foreach (MessageWriter writer in writers)
            {
                int received;
                received = _socket.Send(writer.GetBytes());
                Thread.Sleep(2000);
            }

            byte tempId = 0x00;
            while (true)
            {
                byte[] message = new byte[1024];
                int receive = _socket.Receive(message);

                MessageReader reader;

                Console.WriteLine("message received");
                try
                {
                    reader = new ExtendedMessageReader(message);

                    if (!reader.Checksum())
                    {
                        Console.WriteLine("checksum failed");
                        continue;
                    }
                        

                    Console.WriteLine(reader);
                    byte id = reader.Id;

                    Console.WriteLine(id);

                    switch (id)
                    {
                        case 0x21:
                            Console.WriteLine("received: 0x21");
                            DoctorScreen.UpdateBikeData(reader.ReadByte(), reader.ReadInt(2) / 4m, reader.ReadInt(2), reader.ReadInt(2) / 1000m, reader.ReadInt(1));
                            //Console.WriteLine("bike data: " +reader.ReadByte() + " " + reader.ReadInt(2) + " " + reader.ReadInt(2) + " " + reader.ReadInt(2) + " " + reader.ReadInt(1));
                            break;

                        case 0x33:
                            Console.WriteLine("received: 0x33");
                            DoctorScreen.ReceiveMessage(reader.ReadByte(), Encoding.UTF8.GetString(reader.ReadPacket()), Encoding.UTF8.GetString(reader.ReadPacket()), Encoding.UTF8.GetString(reader.ReadPacket()));
                            break;


                        
                        case 0x43:
                            Console.WriteLine("received 0x43");
                            byte id43 = reader.ReadByte();
                            string name43 = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"id: {id43}, name: {name43}");
                            DoctorLogin.doctorScreen.AddClient(id43, name43);
                            break;

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
                        case 0x55:
                            Console.WriteLine("Received 0x55");
                            
                            DoctorScreenHistorie.ChangeValues(reader.ReadInt(2), reader.ReadInt(2), reader.ReadInt(2), reader.ReadByte());
                            break;

                        case 0x91:
                            Console.WriteLine("received 0x91");
                            MessageEncryption encryption = new MessageEncryption(
                                reader.ReadByte(),
                                reader.ReadByte(),
                                (uint)reader.ReadInt(4),
                                reader.ReadByte());

                            Console.WriteLine(encryption.XorKey1 + " " + encryption.XorKey2);

                            EncryptionManager.Manager.SetEncryption(encryption);
                            break;


                    }

                    // 
                    if (id == 0x80 || id == 0x81)
                    {
                        Console.WriteLine("Login");
                        // Request sent by the doctor client.
                        byte originalRequest = reader.ReadByte();

                        // Check what the server sends to the doctor (mainly used for login)
                        switch (originalRequest)
                        {
                            // Server replies with 0x14 if doctor wants to create account.
                            case 0x14:

                                DoctorLogin.doctorLoginCreation.AccountCreatedReply(id);
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
                    Console.WriteLine(ex.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// send a message written in a messageWriter
        /// </summary>
        public static void Send(byte[] message)
        {
            int received = _socket.Send(message);
        }

        /// <summary>
        /// wait until a new server message has been received
        /// </summary>
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
