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
        AccountTypeSelector accountTypeSelector;
        public AccountLogin()
        {
            InitializeComponent();
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MessageWriter writer = new MessageWriter(0x11);
            writer.WritePacket(Encoding.UTF8.GetBytes(txtAccountNameLogin.Text));
            writer.WritePacket(Encoding.UTF8.GetBytes(textPasswordLogin.Text));
            ServerClient.Send(writer.GetBytes());

            
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
                Console.WriteLine("Faulty credentials");
            }
        }
    }
}
