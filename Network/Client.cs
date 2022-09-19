using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RemoteHealthCare.Network
{
    internal class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;

        private readonly Dictionary<string, Command> _commands;

        private byte[] _totalBuffer = new byte[0];
        private byte[] _buffer = new byte[1024];

        private int test = 0;

        public Client()
        {
            _commands = new Dictionary<string, Command>();
        }

        public async Task connect(string ip, int port)
        {
            if (ip == null || port < 1000)
                throw new MissingFieldException("IP is null or port is already in use");

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
            Console.WriteLine("Message sent");
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

                    if (test == 0)
                    {
                        //The last location your username is in the list 
                        int lastLocation = 0;

                        for (int i = 0; jData["data"].ToArray().Length > i; i++)
                        {
                            Console.WriteLine($"session id: {jData["data"].ElementAt(i)["clientinfo"]["user"]}");
                            if ($"{jData["data"].ElementAt(i)["clientinfo"]["user"]}" == Environment.UserName)
                            {
                                lastLocation = i;
                                Console.WriteLine("New last location =" + lastLocation);
                            }
                        }

                        var session = jData["data"].ElementAt(lastLocation)["id"];

                        //JSon message to request a tunnel
                        String message = @"{""id"" : ""tunnel/create"", ""data"" : {""session"" : """ + session + "\", \"key\" : \"\"}}";
                        Console.WriteLine($"Sending: {message}");
                        Send(message);

                        //Dont go through this again
                        test++;
                    }
                    else
                    {
                        Console.WriteLine($"res: {jData["data"]}");
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

        private void Commands()
        {

        }
    }
}