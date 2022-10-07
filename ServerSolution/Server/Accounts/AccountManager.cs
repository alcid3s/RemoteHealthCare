using MessageStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Server.Accounts
{
    public class AccountManager
    {
        private string _suffix = ".txt";
        private string _path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "/Accounts/Data";

        private string _username;
        private string _password;
        private Socket _socket;

        private AccountState _state;

        public enum AccountState
        {
            CreateClient,
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
            string path = _path + "/" + _username + _suffix;
            if (File.Exists(path))
            {
                if (_state == AccountState.LoginClient)
                {
                    var sr = new StreamReader(File.OpenRead(path));
                    string? credentials = sr.ReadLine();
                    CheckCredentials(credentials);
                }
                else if (_state == AccountState.RemoveClient)
                {
                    // TODO 05-10-2022: Remove account
                }
            }
            else if (_state == AccountState.CreateClient)
            {
                FileStream fs = File.Create(path);
                var sr = new StreamWriter(fs);
                sr.WriteLine('[' + _username + "," + _password + ',' + "c]");
                sr.Close();

            }
        }

        private void CheckCredentials(string? credentials)
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

                MessageWriter writer;

                if (_username.Equals(username) && _password.Equals(password) && type.Equals("c"))
                {
                    writer = new MessageWriter(0x81);
                    Console.WriteLine("Login credentials are correct");
                }
                else
                {
                    writer = new MessageWriter(0x80);
                    Console.WriteLine("Login credentials are faulty");
                }

                writer.WriteByte(0x11);
                _socket.Send(writer.GetBytes());
            }
            else
            {
                Console.WriteLine("credentials is null");
            }
        }
    }
}
