using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteHealthCare.GUI
{
    public partial class AccountTypeSelector : Form
    {
        public AccountTypeSelector()
        {
            InitializeComponent();
        }

        ClientAccountCreation client;
        DoctorAccountCreation doctor;
        AccountLogin accountLogin;

        private void btnTypeClient_Click(object sender, EventArgs e)
        {

            if (client == null)
            {
                client = new ClientAccountCreation();
                client.Show();
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(doctor == null)
            {
                doctor = new DoctorAccountCreation();
                doctor.Show();
            }
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (accountLogin == null)
            {
                accountLogin = new AccountLogin();
                accountLogin.Show();
            }
            this.Hide();
        }
    }
}
