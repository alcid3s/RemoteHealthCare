﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAccountNameAccountCreationClient_TextChanged(object sender, EventArgs e)
        {

        }

        private void textPasswordAccountCreationClient_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPasswordConfirmAccountCreationClient_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCreateAccountCreationClient_Click(object sender, EventArgs e)
        {
            if (txtPasswordConfirmAccountCreationClient.Text.Equals(txtPasswordAccountCreationClient.Text))
            {
                //TODO: Send message to server if account exists and uses correct password.
                if (ServerClient.IsRunning)
                {
                    ServerClient.Send(new byte[] { 0x10 }, txtAccountNameAccountCreationClient.Text + Account.Suffix);
                }
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
    }
}
