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
                Thread.Sleep(20);
            }

            byte tempId = 0x00;
            while (true)
            {
                byte[] message = new byte[1024];
                int receive = _socket.Receive(message);
                Console.WriteLine("Received data");
                try
                {
                    ExtendedMessageReader reader = new ExtendedMessageReader(message);
                    Reply = reader.Id;
                    if (!reader.Checksum())
                        continue;

                    switch (Reply)
                    {
                        // Receives a message from the doctor
                        case 0x31:
                            byte id31 = reader.ReadByte();
                            

                            
                            AccountLogin.clientScreen.AddChatMessage(reader.ReadString(), reader.ReadString(), reader.ReadString());
                            break;
                        //reply whether login was allowed or not

                        case 0x81:
                            Program.loginScreen.login(Reply);
                            break;

                        case 0x80:
                            Program.loginScreen.login(Reply);
                            break;

                        //sets encryption client-side
                        case 0x91:
                            MessageEncryption encryption = new MessageEncryption(
                                reader.ReadByte(),
                                reader.ReadByte(),
                                (uint)reader.ReadInt(4),
                                reader.ReadByte());

                            EncryptionManager.Manager.SetEncryption(encryption);
                            break;

                        //start a session
                        case 0xA0:
                            AccountLogin.clientScreen.StartSession();
                            break;

                        //stop a session
                        case 0xA1:
                            AccountLogin.clientScreen.StopSession();
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
