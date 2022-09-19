using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RemoteHealthCare.Network
{
    internal class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;

        private byte[] _totalBuffer = new byte[0];
        private byte[] _buffer = new byte[1024];

        public string Path { get; } 
        public string Id { get; private set; }
        public Client()
        {
            Path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "/Json";
            Id = string.Empty;
        }

        public async Task Connect(string ip, int port)
        {
            if (ip == null || port < 1000)
                throw new MissingFieldException("IP is null or port is already in use");

            Console.WriteLine($"LOOK FOR THIS {Environment.UserName}");
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                Console.WriteLine($"Connection made with {ip}:{port}");
                _stream = _client.GetStream();
                Send(@"{""id"": ""session/list""}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        public void OnRead(IAsyncResult ar)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Message sent");
            Console.ForegroundColor = ConsoleColor.White;
            
            try
            {
                int rc = _stream.EndRead(ar);
                _totalBuffer = Concat(_totalBuffer, _buffer, rc);
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                return;
            }
            while (_totalBuffer.Length >= 4)
            {
                int packetSize = BitConverter.ToInt32(_totalBuffer, 0);
                if (_totalBuffer.Length >= packetSize + 4)
                {
                    string data = Encoding.UTF8.GetString(_totalBuffer, 4, packetSize);
                    JObject jData = JObject.Parse(data);

                    switch (jData["id"].ToObject<string>())
                    {
                        case "session/list":

                            //The last location of the username in the list, as the username might be in the server multible times and only the most recent one works.
                            int lastLocation = 0;

                            //Go through the list to find your username with the id for tunneling
                            for (int i = 0; jData["data"].ToArray().Length > i; i++)
                            {
                                Console.WriteLine($"session id user: {jData["data"].ElementAt(i)["clientinfo"]["user"]}");
                                if ($"{jData["data"].ElementAt(i)["clientinfo"]["user"]}" == Environment.UserName)
                                {
                                    lastLocation = i;
                                    Console.WriteLine($"New last location = {lastLocation}");
                                }
                            }

                            //Get your id for tunneling
                            var session = jData["data"].ElementAt(lastLocation)["id"];

                            //JSon message to request a tunnel
                            string message = @"{""id"" : ""tunnel/create"", ""data"" : {""session"" : """ + session + "\", \"key\" : \"\"}}";
                            Console.WriteLine($"Sending: {message}");

                            //Send that message
                            Send(message);
                            break;

                        case "tunnel/create":

                            //check if you recieve an error message and print that message
                            if (jData["data"]["status"].ToObject<string>() == "error")
                            {
                                Console.WriteLine("Error while making a tunnel with server, are you running NetwerkEngine?");
                                Console.WriteLine("Server error message:\n" + jData["data"]); 
                                break;
                            }

                            //Get the tunnel id and save it
                            Console.WriteLine($"will try to save ID from:\nServer response Data: {jData["data"]}");
                            Id = jData["data"]["id"].ToObject<string>();

                            //throw an error if the id is empty somehow
                            if (Id.Equals(string.Empty))
                                throw new Exception("Error, couldn't fetch id from tunnel/create");
                            break;

                        default:
                            //No handling implemented so write the full response
                            Console.WriteLine("No handling implemented for the id: " + jData["id"]);
                            Console.WriteLine($"Server response: {jData}");
                            break;
                    }
                    var newBuffer = new byte[_totalBuffer.Length - packetSize - 4];
                    Array.Copy(_totalBuffer, packetSize + 4, newBuffer, 0, newBuffer.Length);
                    _totalBuffer = newBuffer;
                }
                else
                    break;
            }
            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }

        private static byte[] Concat(byte[] b1, byte[] b2, int count)
        {
            byte[] r = new byte[b1.Length + count];
            System.Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
            System.Buffer.BlockCopy(b2, 0, r, b1.Length, count);
            return r;
        }

        public void Send(string message)
        {
            byte[] prefix = BitConverter.GetBytes(message.Length);
            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(prefix, 0, prefix.Length);
            _stream.Write(data, 0, data.Length);
        }
    }
}