using System;
using System.Collections.Generic;
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

        private readonly Dictionary<string, Command> _commands;
		
		private byte[] _totalBuffer = new byte[0];
		private byte[] _buffer = new byte[1024];

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

                    if (_commands.ContainsKey(jData["id"].ToObject<string>()))
                    {
                        Console.WriteLine("Received Command " + jData);
                        _commands[jData["id"].ToObject<string>()].OnCommandReceived(jData);
                    }
                    else
                    {
                        Console.WriteLine($"Could not find command for {jData["id"]}");
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
            byte[] packetLength = BitConverter.GetBytes(message.Length);
            byte[] packet = Encoding.ASCII.GetBytes(message);
            _stream.Write(packetLength, 0, packetLength.Length);
            _stream.Write(packet, 0, packet.Length);


            //IEnumerable<byte> packet = prefix.Concat(data);
            //_stream.Write(packet.ToArray(), 0, packet.Count());
        }
    }
}