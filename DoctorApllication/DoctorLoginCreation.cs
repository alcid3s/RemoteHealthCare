using MessageStream;
using RemoteHealthCare.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorApllication
{
    public partial class DoctorLoginCreation : Form
    {
        public DoctorLoginCreation()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Program.showDoctorLogin();
            Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtAccountName.Text.Length < 41 && txtAccountName.Text.Length > 3)
            {
                if (txtPassword.Text.Length > 7 && txtPassword.Text.Length < 32)
                {
                    if (txtPassword.Text == txtConfirmPassword.Text)
                    {
                        MessageWriter writer = new MessageWriter(0x14);
                        writer.WritePacket(Encoding.UTF8.GetBytes(txtAccountName.Text));
                        writer.WritePacket(Encoding.UTF8.GetBytes(txtPassword.Text));
                        DoctorClient.Send(writer.GetBytes());

                        bool response = DoctorClient.waitForReply();
                        if (!response)
                            return;
                        if (DoctorClient.Reply == 0x80)
                        {
                            txtErrorMessage.Text = "Error with server";
                        }
                        else if (DoctorClient.Reply == 0x81)
                        {
                            //txtErrorMessage.ForeColor = Color.Green;
                            //txtErrorMessage.ForeColorChanged;
                            txtErrorMessage.Text = "Account created";
                        }

                    }
                    else 
                    {
                        txtErrorMessage.Text = "Wrong password confirmation";
                    }
                }
                else
                {
                    txtErrorMessage.Text = "Password too short";
                }
            }
            else
            {
                txtErrorMessage.Text = "Username too short";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtResponse_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
