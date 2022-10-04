using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Accounts
{
    public class AccountManager
    {
        public static string Suffix = ".txt";
        private string _path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "/ServerSolution/Server/Accounts/Data";

        private string _username;
        public AccountManager()
        {

        }
    }
}
