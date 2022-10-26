﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageStream;
using RemoteHealthCare.Accounts;
using RemoteHealthCare.Network;

namespace RemoteHealthCare.GUI
{
    public partial class ClientAccountCreation : Form
    {
        public ClientAccountCreation()
        {
            InitializeComponent();
        }

        AccountTypeSelector accountTypeSelector;

        private void btnCreateAccountCreationClient_Click(object sender, EventArgs e)
        {
            if (txtAccountNameAccountCreationClient.Text.Length < 41 && txtAccountNameAccountCreationClient.Text.Length > 3)
            {
                if (txtPasswordAccountCreationClient.Text.Length > 7 && txtPasswordAccountCreationClient.Text.Length < 32)
                {
                    MessageWriter writer = new MessageWriter(0x10);
                    writer.WritePacket(Encoding.UTF8.GetBytes(txtAccountNameAccountCreationClient.Text));
                    writer.WritePacket(Encoding.UTF8.GetBytes(txtPasswordAccountCreationClient.Text));
                    ServerClient.Send(writer.GetBytes());

                    // await successfull reply

                    AccountLogin login = new AccountLogin();
                    Close();
                    login.Show();
                }
                else
                {
                    txtPasswordAccountCreationClient.Text = "TO LONG OR TO SHORT";
                }
            }
            else
            {
                txtAccountNameAccountCreationClient.Text = "TO LONG OR TO SHORT";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if(accountTypeSelector == null)
            {
                accountTypeSelector = new AccountTypeSelector();
                accountTypeSelector.Show();
            }

            this.Hide();
        }

        private void ClientAccountCreation_Load(object sender, EventArgs e)
        {

        }
    }
}
