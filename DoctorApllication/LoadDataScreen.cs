using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorApllication
{
    public partial class LoadDataScreen : Form
    {
        public LoadDataScreen()
        {
            InitializeComponent();
        }

        List<string> accounts;

        private void lstAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadDataScreen_Load(object sender, EventArgs e)
        {
            DoctorClient.Send(0x50);
            int count = 0;
            while(DoctorClient.accounts.Count == 0)
            {
                Thread.Sleep(100);
                count++;
                if (count > 50)
                {
                    txtError.Text = "Error loading accounts";
                }
            } if (DoctorClient.accounts.Count > 0)
            {
                accounts = DoctorClient.accounts;
                lstAccounts.Items.Add(accounts);
            }
            
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
            List<string> sessions = new List<string>();
            
                foreach(object s in lstAccounts.SelectedItems)
                {
                    accounts.Add(s.ToString());
                }
                foreach(object s in lstSessions.SelectedItems)
                {
                    sessions.Add(s.ToString());
                }
        
                if(sessions.Count == 1 && accounts.Count == 0)
                {

                } else if (accounts.Count == 1 && sessions.Count == 0){

                }
                else
                {
                    txtError.Text = "please select one and only one";
                }
          
        }

        private void lstSessions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
