using Microsoft.VisualBasic;
﻿using MessageStream;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static DoctorApllication.DoctorClient;
using System.Transactions;

namespace DoctorApllication
{
    public partial class DoctorScreen : Form
    {
        public static List<(byte, string)> ClientList = new List<(byte, string)>();
        private int _index = 0;

        private LoadDataScreen _loadDataScreen;
        public DoctorScreen()
        {
            _loadDataScreen = new LoadDataScreen();
            InitializeComponent();
            this.txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);
            DoctorClient.Send(new MessageWriter(0x50).GetBytes());
        }

        public static void FillClientList(byte id, string name)
        {
            ClientList.Add((id, name));
            Console.WriteLine($"Added {id}, {name}, size now: {ClientList.Count}");
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
        public void btnConnectClient_Click(object sender, EventArgs e)
        {
            DoctorClient.Send(new MessageWriter(0x42).GetBytes());
            for(int i = 0; i < 2; i++)
            {
                Thread.Sleep(10);
                if(_index < ClientList.Count - 1)
                {
                    ClientList.ForEach(client =>
                    {
                        lstClients.Items.Add($"id: {client.Item1}, name: {client.Item2}");
                    });
                    break;
                }
            }
        }

        public void UpdateClientList()
        {

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
            
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Loading LoadDataScreen");
            _loadDataScreen.Show();
        }

        private void txtChatBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChatInput_TextChanged(object sender, EventArgs e)
        {
            if (txtChatInput.Text.Length > 200) 
            {
                txtChatInput.Text = txtChatInput.Text.Substring(0, 200);
                txtInfo.Text = "message can have up to 200 characters";
            }
        }

        //currently only local, space the text out so that it fits on the chatbox
        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && txtChatInput.Text.Length > 0)
            {
                //put the time above the message, can later also have the sender
                lstChatView.Items.Insert(0, new ListViewItem(DateAndTime.Now.TimeOfDay.ToString().Substring(0, 8) + " - You"));

                //get all the words from the input
                string[] words = txtChatInput.Text.Split(" ");
                string line = "";

                int lineNr = 1;
                for (int i = 0; i < words.Length; i++) 
                {
                    //check if the word is bigger than a line
                    if (words[i].Length > 40)
                    {
                        //if the line already has text print it out
                        if (line.Length > 0)
                        {
                            lstChatView.Items.Insert(lineNr, new ListViewItem(line.Substring(1, line.Length)));
                            line = "";
                            lineNr++;
                        }

                        //print out the long word bit by bit
                        string longWord = words[i];

                        while (longWord.Length > 40)
                        {
                            lstChatView.Items.Insert(lineNr, new ListViewItem(longWord.Substring(0, 38) + "-"));
                            lineNr++;
                            longWord = longWord.Substring(38);
                        }
                        lstChatView.Items.Insert(lineNr, new ListViewItem(longWord));
                        lineNr++;
                    }
                    else
                    {
                        //add a word to the line
                        line += " " + words[i];

                        //check if there is a next word
                        if (words.Length > i + 1)
                        {
                            //check if the next word will fit on the line
                            if ((line + " " + words[i + 1]).Length > 41)
                            {
                                //print out the line
                                lstChatView.Items.Insert(lineNr, new ListViewItem(line.Substring(1, line.Length-1)));
                                line = "";
                                lineNr++;
                            }
                        }
                        else
                        {
                            //print out the last line
                            lstChatView.Items.Insert(lineNr, new ListViewItem(line.Substring(1, line.Length-1)));
                            line = "";
                            lineNr++;
                        }
                    }
                }
                //add an empty line for clarity
                lstChatView.Items.Insert(lineNr, new ListViewItem());
                //reset the chat input
                txtChatInput.Text = "";
            }
        }

        private void lstChatBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstChatView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }
    }
}
