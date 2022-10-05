using RemoteHealthCare.Network;
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

namespace RemoteHealthCare.GUI
{
    public partial class DoctorAccountCreation : Form
    {
        public DoctorAccountCreation()
        {
            InitializeComponent();
        }

        AccountTypeSelector accountType;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtPasswordAccountCreationDoctor == txtPasswordConfirmAccountCreationPassword)
            {

            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if(accountType == null)
            {
                accountType = new AccountTypeSelector();
                accountType.Show();
            }
            this.Hide();
        }

        private void DoctorAccountCreation_Load(object sender, EventArgs e)
        {
            if (txtAccountNameAccountCreationDoctor.Text.Length < 41 && txtAccountNameAccountCreationDoctor.Text.Length > 3)
            {
                if (txtPasswordAccountCreationDoctor.Text.Length > 7 && txtPasswordAccountCreationDoctor.Text.Length < 32)
                {
                    if (txtPasswordAccountCreationDoctor.Text == txtPasswordConfirmAccountCreationPassword.Text)
                    {
                        MessageWriter writer = new MessageWriter(0x14); 
                        writer.WritePacket(Encoding.ASCII.GetBytes(txtAccountNameAccountCreationDoctor.Text));
                        writer.WritePacket(Encoding.ASCII.GetBytes(txtPasswordAccountCreationDoctor.Text));

                        ServerClient.Send(writer.GetBytes());
                    }


                }
                else
                {
                    txtAccountNameAccountCreationDoctor.Text = "TO LONG OR TO SHORT";
            }
}
            else
                {
                txtPasswordAccountCreationDoctor.Text = "TO LONG OR TO SHORT";
                }
        }
    }
}
