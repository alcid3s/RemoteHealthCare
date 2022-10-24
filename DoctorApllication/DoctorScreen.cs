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
using System.Net.Sockets;
using System.Transactions;

namespace DoctorApllication
{
    public partial class DoctorScreen : Form
    {
        public static List<(byte, string)> ClientList = new List<(byte, string)>();
        private int _index = 0;

        private byte _selectedUser = 0;

        private LoadDataScreen _loadDataScreen;
        private static List<Client> clientList = new List<Client>();
        public DoctorScreen()
        {
         
             InitializeComponent();
            _loadDataScreen = new LoadDataScreen();

            this.txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);
            DoctorClient.Send(new MessageWriter(0x50).GetBytes());
            RefreshAvailableClients();
        }

        private struct Client
        {
            public string Name { get; set; }
            public byte Id { get; }
            public bool Selected { get; set; }
            public Client(byte id, string name, bool selected)
            {
                Id = id;
                Name = name;
                Selected = selected;
            }
        }

        public void RefreshAvailableClients()
        {
            clientList.Clear();
            lstClients2.Items.Clear();
            DoctorClient.Send(new MessageWriter(0x42).GetBytes());
            
        }

        public void AddClient(byte clientId, string clientName)
        {
            //txtInfo.Text = clientName; 
            Console.WriteLine("got client: " + clientName + " " + clientId);
            clientList.Add(new Client(clientId, clientName, false));
            this.Invoke(new Action(new Action(() => {
                lstClients2.Items.Add(clientName);
            })));
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
        public void btnConnectClient_Click(object sender, EventArgs e)
        {
            
            
        }
        private void btnConnectClient_Click(object sender, EventArgs e)
        {
            //create code that checks if the selected item is actually a bike and use Send() to
            //send a message to the server, with the code for the switch case
            //            if (lstClients.SelectedItems != null)
            //            {
            //                foreach (var x in lstClients.SelectedItems)
            //                {
            ///*                    byte ids = (byte)x.ToString().Split(" ")[x.ToString().Split(" ").Length-1];
            //                    foreach (Client client in clientList)
            //                    {
            //                        if ()
            //                        {

            //                        }
            //                    }*/
            //                }








            /*                txtInfo.Text = "connecting to ";
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

                            }*/
            //} else if (lstClients.SelectedItems == null)
            //{
            //    txtInfo.Text = "no client selected";
           
       
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
                // Send text input to the server
                string message = txtChatInput.Text;
                if(message != "" && _selectedUser != 0)
                {
                    // Sends a message to the server, server send it to the client.
                    MessageWriter writer = new MessageWriter(0x30);
                    writer.WriteByte(_selectedUser);
                    writer.WritePacket(Encoding.UTF8.GetBytes(message));
                    DoctorClient.Send(writer.GetBytes());
                }


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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAvailableClients();
            Console.WriteLine("Refreshing list");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Loading");
            string selectedUser = lstClients.SelectedItem.ToString();
            selectedUser = selectedUser.Replace('i', ' ');
            selectedUser = selectedUser.Replace('d', ' ');
            selectedUser = selectedUser.Replace(':', ' ');
            selectedUser = selectedUser.Trim();
            string[] data = selectedUser.Split(',');
            byte id = byte.Parse(data[0]);
            _selectedUser = id;
        }

        private void txtInfo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
