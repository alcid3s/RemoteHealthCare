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

        
    }
}
