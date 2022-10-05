using System;
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
using Server;

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
                // If the server is running and password and confirm password are the same the request to create an account will be made.
                if (ServerClient.IsRunning && txtPasswordAccountCreationClient.Text.Equals(txtPasswordConfirmAccountCreationClient.Text))
                {
                    MessageWriter writer = new MessageWriter(0x10);
                    writer.WritePacket(Encoding.ASCII.GetBytes(txtAccountNameAccountCreationClient.Text));
                    writer.WritePacket(Encoding.ASCII.GetBytes(txtPasswordAccountCreationClient.Text));

                    //byte[] id = { 0x10 };
                    //byte[] sizeu = { (byte)txtAccountNameAccountCreationClient.Text.Length };
                    //byte[] username = Encoding.ASCII.GetBytes(txtAccountNameAccountCreationClient.Text);
                    //byte[] sizep = { (byte)txtAccountNameAccountCreationClient.Text.Length };
                    //byte[] password = Encoding.ASCII.GetBytes(txtPasswordAccountCreationClient.Text);

                    //IEnumerable<byte> message = id.Concat(sizeu).Concat(username).Concat(sizep).Concat(password);
                    ServerClient.Send(writer.GetBytes());
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
