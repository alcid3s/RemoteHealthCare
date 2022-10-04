using Server.Accounts;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Server
    {
        private static Socket? serverSocket;
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
            serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new(IPAddress.Parse("127.0.0.1"), _port);

            // running server
            serverSocket.Bind(endPoint);
            serverSocket.Listen(100);

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
                    Socket socket = serverSocket.Accept();

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
            byte[] message = new byte[8];

            // While the client is connected.
            while (client.Socket.Connected)
            {
                try
                {
                    int receive = client.Socket.Receive(message);

                    byte id = message[1024];

                    switch (id)
                    {
                        // Client wants to create new account
                        case 0x10:
                            message[0] = 0;
                            Console.WriteLine($"Trying to make new Account, data received: {Encoding.UTF8.GetString(message)}");
                            //AccountManager account = new AccountManager(Encoding.UTF8.GetString(message));
                            break;

                        // Client wants to login
                        case 0x11:
                            break;

                        // Client wants to edit account information
                        case 0x12:
                            break;

                        // Client wants to remove account
                        case 0x13:
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
