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
        internal static byte Succes { get; set; } = 0x00;
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
            txtErrorMessage.ForeColor = Color.Red;
            if (txtAccountName.Text.Length < 41 && txtAccountName.Text.Length > 3)
            {
                if (txtPassword.Text.Length > 7 && txtPassword.Text.Length < 32)
                {
                    if (txtPassword.Text == txtConfirmPassword.Text)
                    {
                        MessageWriter writer = new MessageWriter(0x14);
                        writer.WritePacket(Encoding.UTF8.GetBytes(txtAccountName.Text));
                        writer.WritePacket(Encoding.UTF8.GetBytes(txtPassword.Text));

                        txtErrorMessage.Text = "Waiting for server";

                        DoctorClient.Send(writer.GetBytes());

                        int counter = 0;
                        while (Succes == 0x00)
                        {
                            Thread.Sleep(100);
                            counter++;
                            if(counter > 50)
                            {
                                txtErrorMessage.Text = "Error with server, please try again.";
                                return;
                            }
                        }

                        if(Succes == 0x81)
                        {
                            DoctorLogin login = new DoctorLogin();
                            login.Show();
                            Close();
                        }else if(Succes == 0x80)
                        {
                            Console.WriteLine("UnSuccessfull");
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


        private void DoctorLoginCreation_Load(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
