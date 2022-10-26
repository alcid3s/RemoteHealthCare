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
        internal class Client
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
            Console.WriteLine($"Client connection from: {client.Socket.RemoteEndPoint} id: {client.Id}");
            byte[] message = new byte[1024];

            AccountManager? account = null;
            StreamWriter? sr = null;

            bool firstRun = true;

            bool firstTime0x54 = true;

            string rsaCode = "";
            EncryptionManager.Manager.GenerateEncryption(client.Id);
            MessageEncryption encryption = EncryptionManager.Manager.GetEncryption(client.Id);

            // While the client is connected.
            while (client.Socket.Connected)
            {
                try
                {
                    message = new byte[1024];
                    int receive = client.Socket.Receive(message);
                    ExtendedMessageReader reader;
                    Console.WriteLine("message received");
                    try
                    {
                        reader = new ExtendedMessageReader(message, client.Id);
                        //Console.WriteLine(BitConverter.ToString(message));
                        //Console.WriteLine(reader);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }
                    if (!reader.Checksum())
                        continue;

                    byte id = reader.Id;
                    Console.WriteLine(reader.Id);
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

                                for (int i = 0; i < clientList.Count; i++)
                                {
                                    if (clientList[i].Id == id)
                                    {
                                        Client tempClient = clientList[i];
                                        tempClient.Name = usernameLogin;
                                        clientList[i] = tempClient;
                                    }
                                }

                                clientList.ForEach(s =>
                                {
                                    Console.WriteLine(s.Name + s.Id);
                                });


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
                            new AccountManager(user, pass, client.Socket, AccountManager.AccountState.CreateDoctor, client.Id);
                            break;

                        // Doctor wants to login
                        case 0x15:
                            string usernameCreateDoctor = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordCreateDoctor = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to make doctor Log in, data received: {usernameCreateDoctor}, {passwordCreateDoctor}");
                            account = new AccountManager(usernameCreateDoctor, passwordCreateDoctor,
                                client.Socket, AccountManager.AccountState.LoginDoctor, client.Id);

                            client.Name = usernameCreateDoctor;
                            Console.WriteLine(client.Name);

                            if (account.LoggedIn)
                            {
                                client.Name = usernameCreateDoctor;

                                for (int i = 0; i < clientList.Count; i++)
                                {
                                    if (clientList[i].Id == client.Id)
                                    {
                                        Client tempClient = clientList[i];
                                        tempClient.Name = usernameCreateDoctor;
                                        tempClient.IsDoctor = true;
                                        clientList[i] = tempClient;
                                    }
                                }
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

                                        MessageWriter writer = new MessageWriter(0x21, connectedClient.Id);
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

                        //Set the resistance from doctor to client
                        case 0x22:
                            Console.WriteLine("Received 0x22");
                            byte id22 = reader.ReadByte();

                            MessageWriter writer22 = new MessageWriter(0x23, id22);
                            writer22.WriteByte(reader.ReadByte());

                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.Id == id22)
                                {
                                    clientTarget.Socket.Send(writer22.GetBytes());
                                }
                            });
                            break;

                        // gets a message from the doctor and sends this to the corresponding client.
                        case 0x30:
                            Console.WriteLine("Received 0x30");
                            byte id30 = reader.ReadByte();
                            string message30Time = Encoding.UTF8.GetString(reader.ReadPacket());
                            string message30 = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Doctor said: {id30}: {message30} at: {message30Time}");

                            MessageWriter writer30 = new MessageWriter(0x31, id30);
                            writer30.WriteByte(id30);
                            writer30.WritePacket(Encoding.UTF8.GetBytes(client.Name));
                            writer30.WritePacket(Encoding.UTF8.GetBytes((message30)));
                            writer30.WritePacket(Encoding.UTF8.GetBytes((message30Time)));
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
                            string message32Time = Encoding.UTF8.GetString(reader.ReadPacket());
                            string message32 = Encoding.UTF8.GetString(reader.ReadPacket());

                            
                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.IsDoctor)
                                {
                                    MessageWriter writer32 = new MessageWriter(0x33, clientTarget.Id);
                                    writer32.WriteByte(client.Id);
                                    writer32.WritePacket(Encoding.UTF8.GetBytes(client.Name));
                                    writer32.WritePacket(Encoding.UTF8.GetBytes((message32)));
                                    writer32.WritePacket(Encoding.UTF8.GetBytes(message32Time));
                                    clientTarget.Socket.Send(writer32.GetBytes());
                                }
                            });

                            break;

                        //send available clients to the doctor
                        case 0x42:
                            Console.WriteLine("Received 0x42");
                            foreach (var connectedClient in clientList)
                            {
                                Console.WriteLine($"has client: {connectedClient.Name} id: {connectedClient.Id}");

                                if (connectedClient.IsDoctor == false && connectedClient.Name != null)
                                {
                                    Console.WriteLine("found client: " + connectedClient.Name);
                                    MessageWriter messageWriter = new MessageWriter(0x43, client.Id);
                                    messageWriter.WriteByte(connectedClient.Id);
                                    messageWriter.WritePacket(Encoding.UTF8.GetBytes(connectedClient.Name));
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
                                    MessageWriter nameWriter = new MessageWriter(0x51, client.Id);
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
                                        MessageWriter writer = new MessageWriter(0x53, client.Id);
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

                                            MessageWriter writer = new MessageWriter(0x53, client.Id);
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

                            MessageWriter eWriter = new MessageWriter(0x55, client.Id);

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
                                            eWriter.WriteInt((short)Math.Round(bikeData.ElapsedTime * 4), 2);
                                            eWriter.WriteInt(bikeData.DistanceTravelled, 2);
                                            eWriter.WriteInt((short)Math.Round(bikeData.Speed * 1000), 2);
                                            eWriter.WriteInt(bikeData.HeartRate, 1);
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

                        //send a stop session message to a given client
                        case 0x71:
                            Console.WriteLine("Received 0x71");
                            byte id71 = reader.ReadByte();

                            MessageWriter writer71 = new MessageWriter(0x71, id71);

                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.Id == id71)
                                {
                                    clientTarget.Socket.Send(writer71.GetBytes());
                                }
                            });
                            break;

                        //send an emergency stop session message to a given client
                        case 0x73:
                            Console.WriteLine("Received 0x73");
                            byte id73 = reader.ReadByte();

                            MessageWriter writer73 = new MessageWriter(073, id73);

                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.Id == id73)
                                {
                                    clientTarget.Socket.Send(writer73.GetBytes());
                                }
                            });
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
                        case 0x70:
                            Console.WriteLine("Received 0x70");
                            byte id70 = reader.ReadByte();

                            MessageWriter writer70 = new MessageWriter(0x70, id70);

                            clientList.ForEach(clientTarget =>
                            {
                                if (clientTarget.Id == id70)
                                {
                                    clientTarget.Socket.Send(writer70.GetBytes());
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
