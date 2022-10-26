using Server.Accounts;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MessageStream;
using System.Numerics;

namespace Server
{
    public class Server
    {
        private Socket? ServerSocket;
        private static List<Client> clientList = new List<Client>();

        private int _port;

        private StreamReader _streamReader0x54 = null;
        private struct Client
        {
            public string? Name { get; set; }
            public Socket? Socket { get; }
            public byte Id { get; }
            public bool IsDoctor { get; set; }
            public Client(string? name, Socket? socket, byte id, bool isDoctor)
            {
                Name = name;
                Socket = socket;
                Id = id;
                IsDoctor = isDoctor;
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
                    Client client = new(null, socket, (byte)(clientList.Count + 1), false);

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

            string rsaCode = "";
            EncryptionManager.Manager.GenerateEncryption(client.Id);
            MessageEncryption encryption = EncryptionManager.Manager.GetEncryption(client.Id);

            bool firstTime0x54 = false;

            // While the client is connected.
            while (client.Socket.Connected)
            {
                try
                {
                    message = new byte[1024];
                    int receive = client.Socket.Receive(message);
                    ExtendedMessageReader reader;
                    try
                    {
                        reader = new ExtendedMessageReader(message, client.Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
                            new AccountManager(usernameCreate, passwordCreate, client.Socket, AccountManager.AccountState.CreateClient, client.Id);
                            break;

                        // Client wants to login
                        case 0x11:
                            string usernameLogin = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordLogin = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to Login, data received: {usernameLogin}, {passwordLogin}");
                            account = new AccountManager(usernameLogin, passwordLogin, client.Socket, AccountManager.AccountState.LoginClient, client.Id);
                            if (account.LoggedIn) 
                            {
                                client.Name = usernameLogin;
                                clientList.Add(client);
                                Console.WriteLine($"Client: {usernameLogin}, logged in");
                            }
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
                            account = new AccountManager(user, pass, client.Socket, AccountManager.AccountState.CreateDoctor, client.Id);
                            break;

                        // Doctor wants to login
                        case 0x15:
                            string usernameCreateDoctor = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordCreateDoctor = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to make doctor Log in, data received: {usernameCreateDoctor}, {passwordCreateDoctor}");
                            account = new AccountManager(usernameCreateDoctor, passwordCreateDoctor,
                            client.Socket, AccountManager.AccountState.LoginDoctor, client.Id);

                            if (account.LoggedIn)
                            {
                                client.Name = usernameCreateDoctor;
                                client.IsDoctor = true;
                                clientList.Add(client);
                            }
                            break;

                        // Bike information from client to server and then send it to the docters
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

                                foreach (Client connectedClient in clientList)
                                {
                                    if (connectedClient.Name != null && connectedClient.IsDoctor == true) {
                                        //Console.WriteLine("sending data to " + connectedClient.Name);

                                        MessageWriter writer = new MessageWriter(0x21);
                                        writer.WriteByte(client.Id);
                                        writer.WriteInt(reader.ReadInt(2), 2);
                                        writer.WriteInt(reader.ReadInt(2), 2);
                                        writer.WriteInt(reader.ReadInt(2), 2);
                                        writer.WriteInt(reader.ReadInt(1), 1);

                                        connectedClient.Socket.Send(writer.GetBytes());
                                    }
                                }

                            }
                            break;

                            // gets a message from the doctor and sends this to the corresponding client.
                        case 0x30:
                            Console.WriteLine("Received 0x30");
                            byte id30 = reader.ReadByte();
                            string message30Time = reader.ReadString();
                            string message30 = reader.ReadString();
                            Console.WriteLine($"Doctor said: {id30}: {message30} at: {message30Time}");

                            ExtendedMessageWriter writer30 = new ExtendedMessageWriter(0x31);
                            writer30.WriteByte(id30);
                            writer30.WriteString(client.Name);
                            writer30.WriteString(message30);
                            writer30.WriteString(message30Time);
                            clientList.ForEach(clientTarget =>
                            {
                                if(clientTarget.Id == id30)
                                {
                                    clientTarget.Socket.Send(writer30.GetBytes());
                                }
                            });

                            break;

                        // gets a message from a client and sends this to the doctors.
                        case 0x32:
                            Console.WriteLine("Received 0x30");
                            string message32Time = reader.ReadString();
                            string message32 = reader.ReadString();

                            ExtendedMessageWriter writer32 = new ExtendedMessageWriter(0x33);
                            writer32.WriteByte(client.Id);
                            writer32.WriteString(client.Name);
                            writer32.WriteString(message32);
                            writer32.WriteString(message32Time);
                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.IsDoctor)
                                {
                                    clientTarget.Socket.Send(writer32.GetBytes());
                                }
                            });

                            break;

                        case 0x42:
                            Console.WriteLine("Received 0x42");
                            foreach (var connectedClient in clientList)
                            {
                                Console.WriteLine($"has client: {connectedClient.Name}");

                                if (connectedClient.IsDoctor == false)
                                {
                                    ExtendedMessageWriter messageWriter = new ExtendedMessageWriter(0x43);
                                    messageWriter.WriteByte(connectedClient.Id);
                                    messageWriter.WriteString(connectedClient.Name);
                                    client.Socket.Send(messageWriter.GetBytes());
                                }
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

                        // Send all given patient history
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
                                    if (dirs.Length == 1)
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

                            string path54 = AccountManager.PathClient + $"/{accountUser}";
                            Console.WriteLine($"path: {path54}");

                            ExtendedMessageWriter eWriter = new ExtendedMessageWriter(0x55);

                            if (Directory.Exists(path54))
                            {
                                path54 += $"/{sessionName}{AccountManager.Suffix}";
                                if (File.Exists(path54))
                                {

                                    if (!firstTime0x54)
                                    {
                                        firstTime0x54 = true;
                                        _streamReader0x54 = new StreamReader(File.OpenRead(path54));
                                    }
                                    if (_streamReader0x54 != null)
                                    {
                                        string data = _streamReader0x54.ReadLine();
                                        if (data != null)
                                        {
                                            BikeData bikeData = ParseBikeData(data);
                                            eWriter.WriteBikeData(bikeData.ElapsedTime,
                                                bikeData.DistanceTravelled,
                                                bikeData.Speed,
                                                bikeData.HeartRate);
                                            client.Socket.Send(eWriter.GetBytes());
                                        }
                                    }
                                }
                                else
                                    Console.WriteLine($"User has no session: {sessionName}");
                            }
                            else
                                Console.WriteLine($"User: {accountUser} does not exist.");
                            break;
                        case 0x60:
                            Logout(client);
                            firstRun = true;
                            if (sr != null)
                                sr.Close();
                            break;

                        case 0x90:
                            Console.WriteLine(reader);
                            bool continuing = reader.ReadByte() == 1;
                            rsaCode += Encoding.UTF8.GetString(reader.ReadPacket());
                            if (continuing)
                                break;

                            //sending the encoded cipher keys
                            {
                                encryption = EncryptionManager.Manager.GetEncryption(client.Id);
                                Console.WriteLine($"Generated encryption for address {client.Id} with keys {encryption}");

                                MessageWriter writer = new MessageWriter(0x91);
                                writer.WriteByte(encryption.XorKey1);
                                writer.WriteByte(encryption.XorKey2);
                                writer.WriteInt((int)encryption.StartIndex, 4);
                                writer.WriteByte((byte)Math.Round(Math.Log(encryption.StepLength) / Math.Log(2)));

                                client.Socket.Send(writer.GetBytes(rsaCode));
                                Thread.Sleep(10);
                            }

                            break;

                        //send a start session message to a given client
                        case 0xA0:
                            Console.WriteLine("Received 0xA0");
                            byte idA0 = reader.ReadByte();

                            ExtendedMessageWriter writerA0 = new ExtendedMessageWriter(0xA0);

                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.Id == idA0)
                                {
                                    clientTarget.Socket.Send(writerA0.GetBytes());
                                }
                            });
                            break;

                        //send a stop session message to a given client
                        case 0xA1:
                            Console.WriteLine("Received 0xA01");
                            byte idA1 = reader.ReadByte();

                            ExtendedMessageWriter writerA1 = new ExtendedMessageWriter(0xA0);

                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.Id == idA1)
                                {
                                    clientTarget.Socket.Send(writerA1.GetBytes());
                                }
                            });
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
        private BikeData ParseBikeData(string line)
        {
            line = line.Replace('[', ' ');
            line = line.Replace(']', ' ');
            line = line.Trim();

            string[] data = line.Split('-');
            BikeData bikeData = new BikeData();
            foreach (string d in data)
            {
                Console.WriteLine(d);
            }

            bikeData.ElapsedTime = Decimal.Parse(data[0]);
            bikeData.DistanceTravelled = int.Parse(data[1]);
            bikeData.Speed = Decimal.Parse(data[2]);
            bikeData.HeartRate = int.Parse(data[3]);
            return bikeData;
        }
        private void Logout(Client client)
        {
            client.Socket.Send(new MessageWriter(0x61).GetBytes());
            Console.WriteLine($"Client: {client.Id} has logged out");
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
