using MessageStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Server.Server;

namespace Server.Accounts
{
    public class AccountManager
    {
        public bool LoggedIn { get; set; } = false;
        public static string PathClient { get; } = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "/Accounts/Data";
        public static string Suffix { get; } = ".txt";
        
        private string _pathDoctor = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "/Accounts/Doctors";
        private string _username;
        private string _password;
        private Socket _socket;
        private byte _id;

        private AccountState _state;

        public enum AccountState
        {
            Client,
            Doctor,

            CreateClient,
            LoginClient,
            EditClient,
            RemoveClient,

            LoginDoctor,
            CreateDoctor,
            RemoveDoctor
        }
        public AccountManager(string username, string password, Socket socket, AccountState state, byte id)
        {
            _username = username;
            _password = password;
            _socket = socket;
            _state = state;
            _id = id;

            GetData();
        }
        private void GetData()
        {
            string pathClient = PathClient + "/" + _username;
            string pathDoctor = _pathDoctor + "/" + _username;

            if (!Directory.Exists(PathClient))
                Directory.CreateDirectory(PathClient);
            else if (!Directory.Exists(_pathDoctor))
                Directory.CreateDirectory(_pathDoctor);

            if (_state == AccountState.LoginClient)
            {
                if (Directory.Exists(pathClient))
                {
                    var sr = new StreamReader(File.OpenRead(pathClient + "/credentials" + Suffix));
                    string? credentials = sr.ReadLine();
                    if (CheckCredentials(credentials, AccountState.Client))
                    {
                        LoggedIn = true;
                        MessageWriter writer = new MessageWriter(0x81, _id);
                        writer.WriteByte(0x11);
                        _socket.Send(writer.GetBytes());
                    }
                    else
                    {
                        MessageWriter writer = new MessageWriter(0x80);
                        writer.WriteByte(0x11);
                        _socket.Send(writer.GetBytes());
                    }
                }
                else
                {
                    MessageWriter writer = new MessageWriter(0x80, _id);
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
                if (!Directory.Exists(pathClient))
                    Directory.CreateDirectory(pathClient);

                Thread.Sleep(10);
                FileStream fs = File.Create(pathClient + "/credentials" + Suffix);
                var sr = new StreamWriter(fs);
                sr.WriteLine('[' + _username + "," + _password + ',' + "c]");
                sr.Close();
            }

            else if (_state == AccountState.LoginDoctor)
            {
                //check if the requested the account exist
                if (Directory.Exists(pathDoctor))
                {
                    var sr = new StreamReader(File.OpenRead(pathDoctor + "/credentials" + Suffix));
                    string? credentials = sr.ReadLine();
                    if (CheckCredentials(credentials, AccountState.Doctor))
                    {
                        //send login is ok back
                        LoggedIn = true;
                        MessageWriter writer = new MessageWriter(0x81, _id);
                        writer.WriteByte(0x15);
                        _socket.Send(writer.GetBytes());
                    }
                    else
                    {
                        //send cannot log in back as password is incorrect
                        MessageWriter writer = new MessageWriter(0x80, _id);
                        writer.WriteByte(0x15);
                        _socket.Send(writer.GetBytes());
                    }
                }
                else
                {
                    //send cannot log in back as username is unused
                    MessageWriter writer = new MessageWriter(0x80, _id);
                    writer.WriteByte(0x15);
                    _socket.Send(writer.GetBytes());
                }
            }

            else if (_state == AccountState.RemoveDoctor)
            {
                // Remove Doctor
            }

            else if (_state == AccountState.CreateDoctor)
            {
                Console.WriteLine("Doctor");
                if (!Directory.Exists(pathDoctor))
                    Directory.CreateDirectory(pathDoctor);

                Thread.Sleep(10);
                FileStream fs = File.Create(pathDoctor + "/credentials" + Suffix);
                var sr = new StreamWriter(fs);
                sr.WriteLine('[' + _username + "," + _password + ',' + "d]");
                sr.Close();

                Console.WriteLine("Sending data back");
                MessageWriter writer = new MessageWriter(0x81, _id);
                writer.WriteByte(0x14);
                _socket.Send(writer.GetBytes());
            }
        }

        private bool CheckCredentials(string? credentials, AccountState doctorOrClient)
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

                if (_username.Equals(username) && _password.Equals(password)
                    && type.Equals("c") && doctorOrClient == AccountState.Client)
                {
                    Console.WriteLine("Login credentials for client are correct");
                    return true;
                }
                else if (_username.Equals(username) && _password.Equals(password)
                    && type.Equals("d") && doctorOrClient == AccountState.Doctor)
                {
                    Console.WriteLine("Login credentials for doctor are correct");
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
            MessageReader reader = new MessageReader(message, _id);
            sr.WriteLine('[' +
                Encoding.UTF8.GetString(
                Enumerable.Range(0, 7).
                Select(_ => reader.ReadByte()).
                ToArray()) + ']');
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

        public FileStream CreateFile()
        {
            return File.Create(PathClient + "/" + _username +
                    $"/{DateTime.Now.Day}" +
                    $"-{DateTime.Now.Month}" +
                    $"-{DateTime.Now.Year}" +
                    $"_{DateTime.Now.Hour}" +
                    $"-{DateTime.Now.Minute}" +
                    $"-{DateTime.Now.Second}" +
                    Suffix);
        }
    }
}
