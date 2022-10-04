using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Accounts
{
    public class AccountManager
    {
        private string _suffix = ".txt";
        private string _path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "/ServerSolution/Server/Accounts/Data";

        private string _username;
        private string _password;
        public AccountManager(string message)
        {
            ParseMessage(message);
            GetData();
        }

        private void ParseMessage(string message)
        {
            bool password = false;

            foreach(char c in message)
            {
                if(c == '/')
                    password = true;

                if (password && c != '/')
                    _password += c;
                else if (c != '/')
                    _username += c;
            }
        }
        private void GetData()
        {
            string path = _path + "/" + _username + _suffix;
            if (File.Exists(path))
            {
                var sr = new StreamWriter(File.OpenWrite(path));
                sr.WriteLine("Hello, World!");
                sr.Close();
            }
            else
            {
                File.Create(path);
            }
        }
    }
}
