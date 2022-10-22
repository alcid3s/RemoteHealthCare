using Server.Accounts;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MessageStream;

namespace Server
{
    public class Server
    {
        private Socket? ServerSocket;
        private static List<Client> clientList = new List<Client>();

        private int _port;
        private struct Client
        {
            public Socket? Socket { get; }
            public byte Id { get; }
            public Client(Socket? socket, byte id)
            {
                Socket = socket;
                Id = id;
            }
        }

        public struct BikeData
        {
            public byte Identifier { get; set; }
            public decimal ElapsedTime { get; set; }
            public int DistanceTravelled { get; set; }
            public decimal Speed { get; set; }
            public int HeartRate { get; set; }

            public BikeData(byte id, decimal time, int distance, decimal sp, int hr)
            {
                Identifier = id;
                ElapsedTime = time;
                DistanceTravelled = distance;
                Speed = sp;
                HeartRate = hr;
            }
        }

        public Server(int port)
        {
            _port = port;

            // Creating an endpoint and defining the protocol used for communicating.
            ServerSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new(IPAddress.Parse("127.0.0.1"), _port);

            // running server
            ServerSocket.Bind(endPoint);
            ServerSocket.Listen(100);

            Console.WriteLine($"Server setup and running on {endPoint.Address}:{endPoint.Port}");
        }

        public void Run()
        {
            // new thread to listen to incoming connections.
            new Thread(() =>
            {
                while (true)
                {
                    // New incoming connection.
                    Socket? socket = null;
                    if (ServerSocket != null)
                        socket = ServerSocket.Accept();

                    // saving client to list.
                    Client client = new(socket, (byte)(clientList.Count + 1));
                    clientList.Add(client);

                    // Every client gets its own thread.
                    new Thread(() =>
                    {
                        HandleClient(client);
                    }).Start();
                }
            }).Start();
        }

        private void HandleClient(Client client)
        {
            Console.WriteLine($"Client connection from: {client.Socket.RemoteEndPoint}");
            byte[] message = new byte[1024];

            AccountManager? account = null;
            StreamWriter? sr = null;

            bool firstRun = true;

            // While the client is connected.
            while (client.Socket.Connected)
            {
                try
                {
                    int receive = client.Socket.Receive(message);
                    MessageReader reader;
                    try
                    {
                        reader = new MessageReader(message);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    if (!reader.Checksum())
                        continue;

                    byte id = reader.Id;
                    switch (id)
                    {
                        // Client wants to create new account
                        case 0x10:
                            string usernameCreate = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordCreate = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to make new Account, data received: {usernameCreate}, {passwordCreate}");
                            new AccountManager(usernameCreate, passwordCreate, client.Socket, AccountManager.AccountState.CreateClient);
                            break;

                        // Client wants to login
                        case 0x11:
                            string usernameLogin = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordLogin = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to Login, data received: {usernameLogin}, {passwordLogin}");
                            account = new AccountManager(usernameLogin, passwordLogin, client.Socket, AccountManager.AccountState.LoginClient);
                            break;

                        // Client wants to edit account information
                        case 0x12:
                            break;

                        // Client wants to remove account
                        case 0x13:
                            break;

                        // Doctor wants to create account
                        case 0x14:
                            string user = Encoding.UTF8.GetString(reader.ReadPacket());
                            string pass = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to make new Doctor Account, data received: {user}, {pass}");
                            new AccountManager(user, pass, client.Socket, AccountManager.AccountState.CreateDoctor);
                            break;

                        // Doctor wants to login
                        case 0x15:
                            string usernameCreateDoctor = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordCreateDoctor = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to make Log in, data received: {usernameCreateDoctor}, {passwordCreateDoctor}");
                            account = new AccountManager(usernameCreateDoctor, passwordCreateDoctor,
                                client.Socket, AccountManager.AccountState.LoginDoctor);
                            break;

                        // Bike information from client to server
                        case 0x20:
                            if (account != null && account.LoggedIn)
                            {
                                if (firstRun)
                                {
                                    Console.WriteLine("Creating file");
                                    FileStream fs = account.CreateFile();
                                    sr = new StreamWriter(fs);
                                    firstRun = false;
                                }

                                if (sr != null)
                                    account.SaveData(message, sr);
                            }
                            break;

                        // Doctor requests all usernames of registered accounts on the server.
                        case 0x50:
                            Console.WriteLine("Sending list of all clients registered");
                            if (Directory.Exists(AccountManager.PathClient))
                            {
                                string[] dirs = Directory.GetDirectories(AccountManager.PathClient, "*", SearchOption.TopDirectoryOnly);

                                for (int i = 0; i < dirs.Length; i++)
                                {
                                    MessageWriter nameWriter = new MessageWriter(0x51);
                                    string[] name = dirs[i].Split('\\');
                                    dirs[i] = name[name.Length - 1];
                                    nameWriter.WritePacket(Encoding.UTF8.GetBytes(dirs[i]));
                                    client.Socket.Send(nameWriter.GetBytes());
                                    Thread.Sleep(10);
                                }

                                Console.Write("Sending:");
                                foreach (string n in dirs)
                                {
                                    Console.Write($" {n}");
                                }
                                Console.WriteLine();
                                //client.Socket.Send(nameWriter.GetBytes());
                            }
                            else
                            {
                                //TODO: SEND ERROR MESSAGE
                            }
                            break;

                        // Send all patient ids connected with the server.
                        case 0x52:
                            Console.WriteLine("Received 0x52");
                            string accountName = Encoding.UTF8.GetString(reader.ReadPacket());
                            if (Directory.Exists(AccountManager.PathClient))
                            {
                                string path = AccountManager.PathClient + "/" + accountName;
                                if (Directory.Exists(path))
                                {
                                    string[] dirs = Directory.GetFiles(path);

                                    // if credentials is the only thing in the dirs array.
                                    if(dirs.Length == 1)
                                    {
                                        MessageWriter writer = new MessageWriter(0x53);
                                        writer.WritePacket(Encoding.UTF8.GetBytes("No sessions found"));
                                        writer.WriteByte(1);
                                        client.Socket.Send(writer.GetBytes());

                                        // break used for switch case. 
                                        break;
                                    }

                                    foreach (var n in dirs)
                                    {
                                        if (!n.Contains("credentials"))
                                        {
                                            string[] nameOfSession = n.Split('\\');
                                            string[] removeSuffix = nameOfSession[nameOfSession.Length - 1].Split('.');

                                            MessageWriter writer = new MessageWriter(0x53);
                                            writer.WritePacket(Encoding.UTF8.GetBytes(removeSuffix[0]));
                                            Console.WriteLine($"BYTE: {(byte)(dirs.Length - 1)}");
                                            writer.WriteByte((byte)(dirs.Length - 1));
                                            client.Socket.Send(writer.GetBytes());
                                            Thread.Sleep(10);
                                        }

                                    }
                                }
                            }
                            break;
                        case 0x54:
                            Console.WriteLine("Received 0x54");
                            string accountUser = Encoding.UTF8.GetString(reader.ReadPacket());
                            string sessionName = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"user: {accountUser}, session: {sessionName}");
                            break;



                            //Console.WriteLine("Sending data to Doctor");
                            //byte[] packet = new byte[clientList.Count];

                            //for (int i = 0; i < packet.Length; i++)
                            //{
                            //    packet[i] = clientList.ElementAt(i).Id;
                            //}

                            //MessageWriter writer = new MessageWriter(0x53);
                            //writer.WritePacket(packet);

                            //client.Socket.Send(writer.GetBytes());
                            break;
                        case 0x60:
                            Logout(client);
                            firstRun = true;
                            if (sr != null)
                                sr.Close();
                            break;
                    }
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR WITH CLIENT: {e}");
                }
            }

            client.Socket.Close();
        }
        private void Logout(Client client)
        {
            client.Socket.Send(new MessageWriter(0x61).GetBytes());
            Console.WriteLine($"Client: {client.Id} has logged out");
        }
        private BikeData GetBikeData(byte[] message)
        {
            MessageReader reader = new MessageReader(message);
            byte identifier = reader.Id;
            decimal elapsedTime = reader.ReadInt(2) / 4m;
            int distanceTravelled = reader.ReadInt(2);
            decimal speed = reader.ReadInt(2) / 1000m;
            int heartRate = reader.ReadByte();

            return new BikeData(identifier, elapsedTime, distanceTravelled, speed, heartRate);
        }
        private void PrintBikeInformation(BikeData data, Client client)
        {
            Console.WriteLine($"Message from: {client.Id}: id: {data.Identifier}, ep:{data.ElapsedTime}, " +
                $"dt: {data.DistanceTravelled}, " +
                $"sp: {data.Speed}, " +
                $"hr: {data.HeartRate}");
        }
    }
}
