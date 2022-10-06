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
            Create,
            Login,
            Edit,
            Remove
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
                if (_state == AccountState.Login)
                {
                    var sr = new StreamReader(File.OpenRead(path));
                    string? credentials = sr.ReadLine();
                    CheckCredentials(credentials);
                }
                else if (_state == AccountState.Remove)
                {
                    // TODO 05-10-2022: Remove account
                }
                //var sr = new StreamWriter(File.OpenWrite(path));
                //sr.WriteLine("Hello, World!");
                //sr.Close();
            }
            else if (_state == AccountState.Create)
            {
                FileStream fs = File.Create(path);
                var sr = new StreamWriter(fs);
                sr.WriteLine('[' + _username + "," + _password + ']');
                sr.Close();

            }
        }

        private void CheckCredentials(string? credentials)
        {
            if (credentials != null)
            {
                string user = string.Empty;
                string pass = string.Empty;
                bool userBool = false;
                bool passBool = false;
                foreach (char c in credentials)
                {
                    if (c == ',')
                    {
                        userBool = false;
                    }
                    else if (c == ']')
                        passBool = false;

                    // Check if username or password are checked.
                    if (userBool)
                        user += c;
                    else if (passBool)
                        pass += c;

                    // characters that define the end of a string.
                    if (c == '[')
                        userBool = true;
                    else if (c == ',')
                        passBool = true;
                }

                MessageWriter writer;
                Console.WriteLine($"_username: {_username} user: {user} _password: {_password} pass {pass}");
                if (_username.Equals(user) && _password.Equals(pass))
                {
                    writer = new MessageWriter(0x81);
                    Console.WriteLine("Login credentials are correct");
                }
                else
                {
                    writer = new MessageWriter(0x80);
                    Console.WriteLine("Login credentials are fault");
                }
                    

                writer.WriteByte(0x11);
                _socket.Send(writer.GetBytes());
            }
        }
    }
}
