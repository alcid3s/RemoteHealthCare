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
        public static List<string> SessionNameList = new List<string>();
        public static byte Succes { get; set; } = 0x00;

        private static int _sizeOfSessionList = 0;
        public LoadDataScreen()
        {
            InitializeComponent();
        }

        public static void FillIndex(string name)
        {
            Console.WriteLine($"Size of list {ClientNameList.Count}");
            ClientNameList.Add(name);
            Console.WriteLine($"Size of ClientNameList: {ClientNameList.Count}");
        }

        private void Test(object sender, EventArgs e)
        {

        }


        private void lstAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public static void FillSessions(string session, int size)
        {
            if (_sizeOfSessionList == 0)
                _sizeOfSessionList = size;

            SessionNameList.Add(session);
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
                foreach (string name in ClientNameList)
                {
                    lstAccounts.Items.Add(name);
                }
            }
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            //lstSessions.Items.Clear();

            string account = (string)lstAccounts.SelectedItem;

            if (ClientNameList.Contains(account))
            {
                MessageWriter writer = new MessageWriter(0x52);
                writer.WritePacket(Encoding.UTF8.GetBytes(account));
                Console.WriteLine($"Sending: {account} with id: 0x52");
                DoctorClient.Send(writer.GetBytes());

                // if this size is 14, it'll be 14 * 10 + 500 = 640, so we'll wait a total of 6400 miliseconds if the data actually arrives.
                int wait = _sizeOfSessionList * 10 + 500;

                bool success = false;
                for(int i = 0; i < wait; i++)
                {
                    Thread.Sleep(10);
                    if(i >= wait)
                    {
                        txtError.Text = "Data got corrupted during transfer.";
                    }
                    else if(SessionNameList.Count == _sizeOfSessionList)
                    {
                        success = true;
                        break;
                    }
                }

                if (success)
                {
                    _sizeOfSessionList = 0;
                    SessionNameList.ForEach(val =>
                    {
                        lstSessions.Items.Add(val);
                    });
                    SessionNameList.Clear();
                }
            }
            else
            {
                txtError.Text = "The person selected has no data.";
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
