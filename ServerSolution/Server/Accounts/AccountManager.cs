using MessageStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Accounts
{
    public class AccountManager
    {
        public bool LoggedIn { get; set; } = false;
        public bool LoggedInDoctor { get; set; } = false;

        private string _suffix = ".txt";
        private string _path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "/Accounts/Data";
        private string _pathDoctor = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "/Accounts/Doctors";
        private string _username;
        private string _password;
        private Socket _socket;

        private AccountState _state;

        public enum AccountState
        {
            CreateClient,
            CreateDoctor,
            LoginClient,
            EditClient,
            RemoveClient
        }
        public AccountManager(string username, string password, Socket socket, AccountState state)
        {
            _username = username;
            _password = password;
            _socket = socket;
            _state = state;
            GetData();
        }
        private void GetData()
        {
            string path = _path + "/" + _username;
            string pathDoctor = _pathDoctor + "/" + _username;
            if (_state == AccountState.LoginClient)
            {
                if (Directory.Exists(path))
                {
                    var sr = new StreamReader(File.OpenRead(path + "/credentials" + _suffix));
                    string? credentials = sr.ReadLine();
                    if (CheckCredentials(credentials))
                    {
                        LoggedIn = true;
                        MessageWriter writer = new MessageWriter(0x81);
                        writer.WriteByte(0x11);
                        _socket.Send(writer.GetBytes());
                    } if (CheckCredentialsDoctor(credentials))
                    {
                        LoggedInDoctor = true;
                        MessageWriter writer = new MessageWriter(0x81);
                        writer.WriteByte(0x11);
                        _socket.Send(writer.GetBytes());
                    }
                }
                else
                {
                    MessageWriter writer = new MessageWriter(0x80);
                    writer.WriteByte(0x11);
                    _socket.Send(writer.GetBytes());
                }
            }
            else if (_state == AccountState.RemoveClient)
            {
                // TODO 05-10-2022: Remove account
            }
            else if (_state == AccountState.CreateClient)
            {
                Directory.CreateDirectory(path);
                Thread.Sleep(10);
                FileStream fs = File.Create(path + "/credentials" + _suffix);
                var sr = new StreamWriter(fs);
                sr.WriteLine('[' + _username + "," + _password + ',' + "c]");
                sr.Close();
            }
            else if (_state == AccountState.CreateDoctor)
            {
                Directory.CreateDirectory(path);
                Thread.Sleep(10);
                FileStream fs = File.Create(path + "/credentials" + _suffix);
                var sr = new StreamWriter(fs);
                sr.WriteLine('[' + _username + "," + _password + ',' + "d]");
                sr.Close();
            }
        }

        private bool CheckCredentialsDoctor(string? credentials)
        {
            if (credentials != null)
            {
                string[] creds = credentials.Split(',');
                string username = creds[0];
                string password = creds[1];
                string type = creds[2];

                username = username.Replace('[', ' ');
                username = username.Trim();

                type = type.Replace(']', ' ');
                type = type.Trim();
                
                if (_username.Equals(username) && _password.Equals(password) && type.Equals("d"))
                {
                    Console.WriteLine("Login credentials Doctor are correct");
                    return true;
                }
                else
                {
                    Console.WriteLine("Login credentials are faulty");
                    return false;
                }
            }
            else
                return false;
        }

        private bool CheckCredentials(string? credentials)
        {
            if (credentials != null)
            {
                string[] creds = credentials.Split(',');
                string username = creds[0];
                string password = creds[1];
                string type = creds[2];

                username = username.Replace('[', ' ');
                username = username.Trim();

                type = type.Replace(']', ' ');
                type = type.Trim();

                if (_username.Equals(username) && _password.Equals(password) && type.Equals("c"))
                {
                    Console.WriteLine("Login credentials Client are correct");
                    return true;
                }
                else
                {
                    Console.WriteLine("Login credentials are faulty");
                    return false;
                }
            }
            else
                return false;
        }

        public void SaveData(byte[] message, StreamWriter sr)
        {
            MessageReader reader = new MessageReader(message);
            sr.WriteLine('[' + 
                Encoding.UTF8.GetString(
                Enumerable.Range(0, 7).
                Select(_ => reader.ReadByte()).
                ToArray()) + ']');
        }

        public FileStream CreateFile()
        {
            return File.Create(_path + "/" + _username +
                    $"/{DateTime.Now.Day}" +
                    $"-{DateTime.Now.Month}" +
                    $"-{DateTime.Now.Year}" +
                    $"_{DateTime.Now.Hour}" +
                    $"-{DateTime.Now.Minute}" +
                    $"-{DateTime.Now.Second}" +
                    _suffix);
        }
    }
}
