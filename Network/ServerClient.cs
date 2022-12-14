using MessageStream;
using RemoteHealthCare.GUI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteHealthCare.Network
{
    internal class ServerClient
    {
        private int _port;
        private IPAddress _address;

        private static Socket _socket;

        public static bool IsRunning { get; private set; } = false;

        internal static byte Reply { get; set; } = 0x00;

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
                Console.WriteLine("Received data");
                MessageReader reader;
                try
                {
                    reader = new MessageReader(message);
                    Reply = reader.Id;

                    if (!reader.Checksum())
                        continue;

                    Console.WriteLine(reader);

                    byte id = reader.Id;

                    switch (id)
                    {
                        //set the resistance
                        case 0x23:
                            Console.WriteLine("received 0x23");
                            AccountLogin.clientScreen.SetResistance(reader.ReadByte());
                            
                            break;
                        // Receives a message from the doctor
                        case 0x31:
                            Console.WriteLine("received 0x31");
                            byte id31 = reader.ReadByte();
                            AccountLogin.clientScreen.AddChatMessage(Encoding.UTF8.GetString(reader.ReadPacket()), Encoding.UTF8.GetString(reader.ReadPacket()), Encoding.UTF8.GetString(reader.ReadPacket()));
                            break;

                        //start a session
                        case 0x70:
                            Console.WriteLine("starting session received");
                            AccountLogin.clientScreen.SetTxtInfo("starting session");
                            AccountLogin.clientScreen.StartSession();
                            break;

                        //stop a session
                        case 0x71:
                            Console.WriteLine("stopping session received");
                            AccountLogin.clientScreen.SetTxtInfo("stopping session");
                            AccountLogin.clientScreen.StopSession();
                            break;

                        //emergency stop
                        case 0x73:
                            Console.WriteLine("emergency stop received");
                            AccountLogin.clientScreen.SetTxtInfo("Emergency stop");
                            AccountLogin.clientScreen.Emergency();
                            Program.BikeClient.ResetScene();
                            break;

                        //reply wether login was allowed or not
                        case 0x80:
                            Console.WriteLine("received 0x80");
                            switch (reader.ReadByte())
                            {
                                case 0x11:
                                    Program.loginScreen.login(id);
                                    break;

                                case 0x10:
                                    AccountLogin.Creation.AccountCreatedReply(id);
                                    break;
                            }
                            break;

                        case 0x81:
                            Console.WriteLine("received 0x81");
                            switch (reader.ReadByte())
                            {
                                case 0x11:
                                    Program.loginScreen.login(id);
                                    break;

                                case 0x10:
                                    AccountLogin.Creation.AccountCreatedReply(id);
                                    break;
                            }
                            
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
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
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
        public static void Send(byte id, decimal elapsedTime, int distanceTravelled, decimal speed, int heartRate)
        {
            MessageWriter writer = new MessageWriter(id);
            writer.WriteInt((int) Math.Round(elapsedTime * 4), 2);
            writer.WriteInt(distanceTravelled, 2);
            writer.WriteInt((int)Math.Round(speed * 1000), 2);
            writer.WriteInt(heartRate, 1);
            int received = _socket.Send(writer.GetBytes());
        }

        public static void Send(byte[] message)
        {
            
            Console.WriteLine("sending message");
            
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
