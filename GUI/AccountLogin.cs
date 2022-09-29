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
    public partial class AccountLogin : Form
    {
        public AccountLogin()
        {
            InitializeComponent();
        }
        AccountTypeSelector accountTypeSelector;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

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
    }
}
