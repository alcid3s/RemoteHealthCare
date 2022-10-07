using Server.Accounts;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MessageStream;

namespace Server
{
    internal class Server
    {
        private Socket? ServerSocket;
        private static List<Client> clientList = new List<Client>();

        private int _port;
        struct Client
        {
            public Socket Socket { get; }
            public int Id { get; }
            public Client(Socket socket, int id)
            {
                Socket = socket;
                Id = id;
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
                    Socket socket = ServerSocket.Accept();

                    // saving client to list.
                    Client client = new(socket, clientList.Count + 1);
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
                            AccountManager account = new AccountManager(usernameCreate, passwordCreate, client.Socket, AccountManager.AccountState.CreateClient);
                            break;

                        // Client wants to login
                        case 0x11:
                            string usernameLogin = Encoding.UTF8.GetString(reader.ReadPacket());
                            string passwordLogin = Encoding.UTF8.GetString(reader.ReadPacket());
                            Console.WriteLine($"Trying to Login, data received: {usernameLogin}, {passwordLogin}");
                            AccountManager accountLogin = new AccountManager(usernameLogin, passwordLogin, client.Socket, AccountManager.AccountState.LoginClient);
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
                            break;

                        // SimBike information
                        case 0x21:
                            PrintBikeInformation(message, client, id);
                            break;
                    }
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR WITH CLIENT: {e}");
                }
            }
        }

        private void PrintBikeInformation(byte[] message, Client client, byte id)
        {
            decimal elapsedTime = ((message[2] << 8) + message[1]) / 4m;
            int distanceTravelled = (message[4] << 8) + message[3];
            decimal speed = ((message[6] << 8) + message[5]) / 1000m;
            int heartRate = message[7];
            Console.WriteLine($"Message from: {client.Id}: id: {id}, ep:{elapsedTime}, " +
                $"dt: {distanceTravelled}, " +
                $"sp: {speed}, " +
                $"hr: {heartRate}");
        }
    }
}
