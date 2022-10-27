using System;
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

        private void btnCreateAccountCreationClient_Click(object sender, EventArgs e)
        {
            if (txtAccountNameAccountCreationClient.Text.Length < 41 && txtAccountNameAccountCreationClient.Text.Length > 3)
            {
                if (txtPasswordAccountCreationClient.Text.Length > 7 && txtPasswordAccountCreationClient.Text.Length < 32)
                {
                    if (txtPasswordAccountCreationClient == txtPasswordConfirmAccountCreationClient)
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
                        txtErrorMsg.Text = "Passwords are not the same";
                    }
                }
                else
                {
                    txtErrorMsg.Text = "Account name size is wrong";
                }
            }
            else
            {
                txtErrorMsg.Text = "Password size is wrong";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

            Program.loginScreen.Show();
            this.Close();
        }

        private void ClientAccountCreation_Load(object sender, EventArgs e)
        {

        }
    }
}
