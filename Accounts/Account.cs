using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthCare.Accounts
{
    internal class Account
    {
        // These variables are needed in AccountLogin to locate the files of the Accounts.
        public static string Suffix = ".txt";
        public static string Path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "/Accounts/Data";

        private string _username;

        public Account(string username)
        {
            _username = username;
            GetData();
        }

        private void GetData()
        {
            string path = Path + "/" + _username + Suffix;
            if (File.Exists(path))
            {
                FileStream fs = File.OpenWrite(path);
                var sr = new StreamWriter(fs);
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
