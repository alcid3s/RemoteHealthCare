using MessageStream;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
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
        public static List<string> ClientNameList = new List<string>();
        public static byte Succes { get; set; } = 0x00;
        public LoadDataScreen()
        {
            InitializeComponent();
            DoctorClient.Send(new MessageWriter(0x50).GetBytes());
        }

        public static void FillIndex(MessageReader reader)
        {
            for(int i = 0; i < 4; i++)
            {
                ClientNameList.Add(Encoding.UTF8.GetString(reader.ReadPacket()));
            }
            
        }

        private void Test(object sender, EventArgs e)
        {

        }


        private void lstAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadDataScreen_Load(object sender, EventArgs e)
        {
            int count = 0;
            while (ClientNameList.Count == 0)
            {
                Thread.Sleep(100);
                count++;
                if (count > 50)
                {
                    txtError.Text = "Error loading accounts";
                }
            }
            if (ClientNameList.Count > 0)
            {
                foreach(string name in ClientNameList)
                {
                    lstAccounts.Items.Add(name);
                }
                
            }

        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            List<string> accounts = new List<string>();
            List<string> sessions = new List<string>();

            foreach (object s in lstAccounts.SelectedItems)
            {
                accounts.Add(s.ToString());
            }
            foreach (object s in lstSessions.SelectedItems)
            {
                sessions.Add(s.ToString());
            }

            if (sessions.Count == 1 && accounts.Count == 0)
            {

            }
            else if (accounts.Count == 1 && sessions.Count == 0)
            {

            }
            else
            {
                txtError.Text = "please select one and only one";
            }

        }

        private void lstSessions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
