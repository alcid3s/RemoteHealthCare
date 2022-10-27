using DoctorApplication;
using MessageStream;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DoctorApllication
{
    public partial class LoadDataScreen : Form
    {
        public static List<string> ClientNameList = new List<string>();
        private int _clientNameSize = 0;

        public static List<string> SessionNameList = new List<string>();
        public static byte Succes { get; set; } = 0x00;

        private static int _sizeOfSessionList = 0;
        private string _selectedUser = "";
        public LoadDataScreen()
        {
            InitializeComponent();
        }

        public static void FillIndex(string name)
        {
            ClientNameList.Add(name);
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
            {
                SessionNameList.Clear();
                _sizeOfSessionList = size;
            }
            SessionNameList.Add(session);
        }

        private void LoadDataScreen_Load(object sender, EventArgs e)
        {
            new Thread(LoadAccountData).Start();
            new Thread(LoadDataFromAccount).Start();
        }
        private void LoadAccountData()
        {
            while (true)
            {
                if(_clientNameSize < ClientNameList.Count)
                {
                    Invoke(new Action(new Action(() => {
                        lstAccounts.Items.Add(ClientNameList.ElementAt(_clientNameSize));
                    })));
                    _clientNameSize++;
                }
            }
        }

        private void LoadDataFromAccount()
        {
            while (true)
            {
                if(SessionNameList.Count == _sizeOfSessionList)
                {
                    _sizeOfSessionList = 0;
                    Invoke(new Action(new Action(() => {
                        SessionNameList.ForEach(val =>
                        {
                            lstSessions.Items.Add(val);
                        });
                    })));
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string selectedItem = lstAccounts.SelectedItem.ToString();

            if (ClientNameList.Contains(selectedItem) && !_selectedUser.Equals(selectedItem))
            {
                lstSessions.Items.Clear();
                _selectedUser = selectedItem;
                MessageWriter writer = new MessageWriter(0x52);
                writer.WritePacket(Encoding.UTF8.GetBytes(selectedItem));
                Console.WriteLine($"Sending: {selectedItem} with id: 0x52");
                DoctorClient.Send(writer.GetBytes());
            }

            if(lstSessions.SelectedItem != null)
                selectedItem = lstSessions.SelectedItem.ToString();

            if (SessionNameList.Contains(selectedItem))
            {
                Console.WriteLine($"Selected SESSION: {selectedItem}");
                MessageWriter writer = new MessageWriter(0x54);
                writer.WritePacket(Encoding.UTF8.GetBytes(_selectedUser));
                writer.WritePacket(Encoding.UTF8.GetBytes(selectedItem));
                DoctorClient.Send(writer.GetBytes());

                DoctorScreenHistorie doctorScreen = new DoctorScreenHistorie();
                doctorScreen.clientUsername = _selectedUser;
                doctorScreen.clientSession = selectedItem;
                doctorScreen.Show();
                Close();
            }
        }


        private void lstSessions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DoctorScreen doctorScreen = new DoctorScreen();
            Close();
        }
    }
}
