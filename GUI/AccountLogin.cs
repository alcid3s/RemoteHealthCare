﻿using MessageStream;
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
        internal static ClientScreen ClientScreen;
        internal static bool IsLoggedIn = false;
        public AccountLogin()
        {
            InitializeComponent();
        }
        AccountTypeSelector accountTypeSelector;


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MessageWriter writer = new MessageWriter(0x11);
            writer.WritePacket(Encoding.UTF8.GetBytes(txtAccountNameLogin.Text));
            writer.WritePacket(Encoding.UTF8.GetBytes(textPasswordLogin.Text));
            ServerClient.Send(writer.GetBytes());

            int counter = 0;
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
                ServerClient.Reply = 0x00;
            }
            else if (ServerClient.Reply == 0x81)
            {
                if (!IsLoggedIn)
                {
                    ServerClient.Reply = 0x00;
                    IsLoggedIn = true;
                    ClientScreen = new ClientScreen();
                    Hide();
                    ClientScreen.Show();
                }
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if (accountTypeSelector == null)
            {
                accountTypeSelector = new AccountTypeSelector();

                Hide();

                accountTypeSelector.Show();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAccountNameLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void textPasswordLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void AccountLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
