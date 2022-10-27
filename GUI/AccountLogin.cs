using MessageStream;
using RemoteHealthCare.Accounts;
using RemoteHealthCare.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteHealthCare.GUI
{
    public partial class AccountLogin : Form
    {
        public static ClientScreen clientScreen;
        internal static bool isloggedIn = false;
        ClientAccountCreation client;

        public AccountLogin()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            txtLoginInfo.Text = "";
            MessageWriter writer = new MessageWriter(0x11);
            writer.WritePacket(Encoding.UTF8.GetBytes(txtAccountNameLogin.Text));
            writer.WritePacket(Encoding.UTF8.GetBytes(textPasswordLogin.Text));

            ServerClient.Send(writer.GetBytes());

            /*int counter = 0;
            ServerClient.Reply = 0x00;

            ServerClient.Send(writer.GetBytes());

            while (ServerClient.Reply == 0x00)
            {
                Thread.Sleep(100);
                counter++;
                if (counter == 50)
                {
                    throw new Exception("Reply from server takes too long");
                }
            }

            Console.WriteLine($"Checking serverClient.Reply = {ServerClient.Reply}");
            if (ServerClient.Reply == 0x80)
            {
                Console.WriteLine("Error");
            }
            else if (ServerClient.Reply == 0x81)
            {
                if (!IsLoggedIn)
                {
                    ClientScreen = new ClientScreen();
                    IsLoggedIn = true;

                    if (Program.NetworkEngineRunning)
                    {
                        ClientScreen.LocalNetworkEngineRunning = true;
                    }
                    Hide();
                    ClientScreen.Show();
                }*/
            //}

        }


        private void btnCreateAccount_Click(object sender, EventArgs e)
        {

            client = new ClientAccountCreation();
            client.Show();
            
            this.Hide();
        }

        private void AccountLogin_Load(object sender, EventArgs e)
        {

        }

        public void login(byte CanLogin)
        {

            if (CanLogin == 0x81 && this.InvokeRequired)
            {
                if (!isloggedIn)
                {
                    this.Invoke(new Action(new Action(() => {
                        clientScreen = new ClientScreen();
                        isloggedIn = true;
                        clientScreen.Show();
                        Hide();
                    })));

                }

            }
            else if (CanLogin == 0x80 && this.InvokeRequired)
            {
                this.Invoke(new Action(new Action(() => {
                    txtLoginInfo.Text = "Incorrect credentials";
                })));
                Console.WriteLine("Faulty credentials");
            }
        }
    }
}
