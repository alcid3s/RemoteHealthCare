using MessageStream;
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
    public partial class DoctorScreen : Form
    {
        private LoadDataScreen _loadDataScreen;
        public DoctorScreen()
        {
            _loadDataScreen = new LoadDataScreen();
            InitializeComponent();
            DoctorClient.Send(new MessageWriter(0x50).GetBytes());
        }
        

        public void setTXTSpeed(string s)
        {
            txtSpeed.Text = s;
        }
        public void setTXTDT(string s)
        {
            txtDT.Text = s;
        }
        public void setTXTET(string s)
        {
            txtET.Text = s;
        }
        public void setTXTHR(string s)
        {
            txtHR.Text = s;
        }

        public void addListItems(string s)
        {
            lstClients.Items.Add(s);
        }
        private void btnConnectClient_Click(object sender, EventArgs e)
        {
            //create code that checks if the selected item is actually a bike and use Send() to
            //send a message to the server, with the code for the switch case
            if (lstClients.SelectedItems != null)
            {
                txtInfo.Text = "connecting to ";
                foreach(object s in lstClients.SelectedItems)
                {
                    txtInfo.Text += s.ToString();
                    if (s.ToString().Equals("Simulation Bike"))
                    {
                        txtInfo.Text += " 1"; 
                        //DoctorClient.Send(1);
                    }
                //continue like this for all existing bikes, its only five(better if done with switch case)
                }
                if (lstClients.SelectedItems.ToString() == "Simulation Bike")
                {
                  
                }
            } else if (lstClients.SelectedItems == null)
            {
                txtInfo.Text = "no client selected";
            }
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSpeed_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtET_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtHR_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DoctorScreen_Load(object sender, EventArgs e)
        {
            // create code that scans all possible nearby bluetooth devices
            // and displays them in de nike list view if they are bikes 
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Loading LoadDataScreen");
            _loadDataScreen.Show();
        }
    }
}
