﻿using System;
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
        public DoctorScreen()
        {
            InitializeComponent();
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
            if (lstClients.SelectedItems != null)
            {
                txtInfo.Text = "connecting...";
                txtInfo.Text = lstClients.SelectedItems.ToString();
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
    }
}
