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
    public partial class DoctorLogin : Form
    {
        public static DoctorScreen doctorScreen;
        internal static bool isloggedIn = false;

        public static DoctorLoginCreation doctorLoginCreation;

        public DoctorLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtLoginInfo.Text = "";

            ExtendedMessageWriter writer = new ExtendedMessageWriter(0x15);
            writer.WriteString(txtAccountName.Text);
            writer.WriteString(txtPassword.Text);
            DoctorClient.Send(writer.GetBytes());
        }

            private void DoktorLogin_Load(object sender, EventArgs e)
            {

            }

            private void button2_Click(object sender, EventArgs e)
            {
                doctorLoginCreation = new DoctorLoginCreation();
                doctorLoginCreation.Show();
                this.Hide();
            }

        public void login(byte CanLogin)
        {
            
            if (CanLogin == 0x81 && this.InvokeRequired)
            {
                if (!isloggedIn)
                { 
                    this.Invoke(new Action(new Action(() => { 
                        doctorScreen = new DoctorScreen();
                        isloggedIn = true;
                        doctorScreen.Show();
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
