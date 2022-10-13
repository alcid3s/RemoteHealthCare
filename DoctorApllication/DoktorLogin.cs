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
    public partial class DoktorLogin : Form
    {
        internal static DoctorScreen doctorScreen;
        internal static bool isloggedIn = false;
        DoctorLoginCreation doctorLoginCreation;


        public DoktorLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExtendedMessageWriter writer = new ExtendedMessageWriter(0x15);
            writer.WriteString(txtAccountName.Text);
            writer.WriteString(txtPassword.Text);
            DoctorClient.Send(writer.GetBytes());

            //wait for a response and return when there is no response
            bool response = DoctorClient.waitForReply();
            if (!response)
                return;
            
            if (DoctorClient.Reply == 0x80)
            {

            }
            else if (DoctorClient.Reply == 0x81)
            {
                if (!isloggedIn)
                {
                    doctorScreen = new DoctorScreen();
                    isloggedIn = true;
                    Hide();
                    doctorScreen.Show();
                }
            }
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

        
    }
}
