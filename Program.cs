using System;
using System.Threading;
using System.Windows.Forms;

using RemoteHealthCare.Bikes;
using RemoteHealthCare.GUI;
using RemoteHealthCare.Network;

namespace RemoteHealthCare
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountLogin loginScreen = new AccountLogin();
            ServerClient serverClient = new ServerClient("127.0.0.1", 1337);
            serverClient.Connect();

            Thread.Sleep(1000);

            Application.Run(loginScreen);
            for (; ; );
        }
    }
}