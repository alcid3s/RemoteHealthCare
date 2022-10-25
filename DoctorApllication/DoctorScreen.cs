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
using System.Text.RegularExpressions;
using System.Diagnostics.Metrics;

namespace DoctorApllication
{
    public partial class DoctorScreen : Form
    {

        private byte _selectedUserId = 0;
        private string _selectedUserName = string.Empty;

        private LoadDataScreen _loadDataScreen = new LoadDataScreen();
        private static List<Client> clientList = new List<Client>();
        private static List<DoctorScreen> screens = new List<DoctorScreen>();
        public DoctorScreen()
        {
            InitializeComponent();
            txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);

            Text = "Primary Doctor screen";

            Send(new MessageWriter(0x50).GetBytes());
            RefreshAvailableClients();
        }

        //constructor for a secundary screen, where the refresh and load buttons are disabled
        private DoctorScreen(byte selectedUserId, string selectedUserName)
        {
            InitializeComponent();
            txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);

            lstClients2.Enabled = false;
            btnRefresh.Enabled = false;
            btnLoad.Enabled = false;
            lstClients2.Visible = false;
            btnRefresh.Visible = false;
            btnLoad.Visible = false;

            Text = "Secondary screen, Connected with: " + selectedUserName;

            _selectedUserId = selectedUserId;
            _selectedUserName = selectedUserName;
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

        //clear the client lists and request a new list of clients
        public void RefreshAvailableClients()
        {
            clientList.Clear();
            lstClients2.Items.Clear();
            Send(new MessageWriter(0x42).GetBytes());
        }

        /// <summary>
        /// Add a client to the clientList and show it on lstClient2 from the client request
        /// </summary>
        /// <param name="clientId">Id of the client</param>
        /// <param name="clientName">Name of the client</param>

        public void AddClient(byte clientId, string clientName)
        {
            //txtInfo.Text = clientName; 
            Console.WriteLine("got client: " + clientName + " " + clientId);
            clientList.Add(new Client((byte)(clientId + clientList.Count), clientName, false));
            Invoke(new Action(new Action(() => {

                // Do not change this string, the btnLoad_Click method is dependend on this format.
                lstClients2.Items.Add($"{clientName}, {clientId + clientList.Count -1}");
            })));
        }

        /// <summary>
        /// Update the bikeeData
        /// </summary>
        /// <param name="elapsedTime">Time passed in seconds</param>
        /// <param name="meter">meters cycled in meters</param>
        /// <param name="speed">speed in m/st</param>
        /// <param name="heartRate">in bpm</param>
        public static void UpdateBikeData(byte id, decimal elapsedTime, int meter, decimal speed, int heartRate)
        {
            if (DoctorLogin.doctorScreen._selectedUserId == id)
            {
                DoctorLogin.doctorScreen.Invoke(new Action(new Action(() => {
                    DoctorLogin.doctorScreen.txtSpeed.Text = speed.ToString();
                    DoctorLogin.doctorScreen.txtDT.Text = meter.ToString();
                    DoctorLogin.doctorScreen.txtET.Text = elapsedTime.ToString();
                    DoctorLogin.doctorScreen.txtHR.Text = heartRate.ToString();

                    DoctorLogin.doctorScreen.txtInfo.Text = $"Connected with: {DoctorLogin.doctorScreen._selectedUserName}";
                })));
            }

                screens.ForEach(doctorScreen =>
            {
                // If a user is selected.
                if (doctorScreen._selectedUserId == id)
                {
                    doctorScreen.Invoke(new Action(new Action(() => {
                        doctorScreen.txtSpeed.Text = speed.ToString();
                        doctorScreen.txtDT.Text = meter.ToString();
                        doctorScreen.txtET.Text = elapsedTime.ToString();
                        doctorScreen.txtHR.Text = heartRate.ToString();

                        doctorScreen.txtInfo.Text = $"Connected with: {doctorScreen._selectedUserName}";
                    })));
                }
            });
            
            
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

        private void DoctorScreen_Load(object sender, EventArgs e)
        {

        }

        private void SetConnectedUser(byte clientId, string clientName)
        {
            this._selectedUserId = clientId;
            this._selectedUserName = clientName;



            Name = "Secondary screen, Connected with: " + clientName;
        }

        /// <summary>
        /// Show loadDataScreen when the loadData button is pressed
        /// </summary>
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Loading LoadDataScreen");
            _loadDataScreen = new LoadDataScreen();
            _loadDataScreen.Show();
        }

        /// <summary>
        /// Make sure the input message isnt too long
        /// </summary>

        private void txtChatInput_TextChanged(object sender, EventArgs e)
        {
            if (txtChatInput.Text.Length > 200) 
            {
                txtChatInput.Text = txtChatInput.Text.Substring(0, 200);
                txtInfo.Text = "message can have up to 200 characters";
            }
        }

        /// <summary>
        /// React to an enter press while in txtCHatInput and then send the message to a client and add it to lstChatView
        /// </summary>
        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && txtChatInput.Text.Length > 0)
            {
                // Send text input to the server
                string message = txtChatInput.Text;
                if(message != "" && _selectedUserId != 0)
                {
                    // Sends a message to the server, server send it to the client.
                    ExtendedMessageWriter writer = new ExtendedMessageWriter(0x30);
                    writer.WriteByte(_selectedUserId);
                    writer.WriteString(DateAndTime.Now.TimeOfDay.ToString().Substring(0, 8));
                    writer.WriteString(message);
                    DoctorClient.Send(writer.GetBytes());
                }

                AddChatMessage(txtChatInput.Text, "You", DateAndTime.Now.TimeOfDay.ToString().Substring(0, 8));
            }
        }

        /// <summary>
        /// Add a message to lstChatView with a sender and time at 40 characters per line
        /// </summary>
        /// <param name="message">Id of the bike</param>
        /// <param name="sender">Name of the person that send the message</param>
        /// <param name="timeSend">The time the message was send in hh:mm:ss format</param>

        public void AddChatMessage(string message, string sender, string timeSend)
        {
            Invoke(new Action(new Action(() =>
            {
                //put the time above the message, can later also have the sender
                lstChatView.Items.Insert(0, new ListViewItem(timeSend + " - " + sender));

                //get all the words from the input
                string[] words = message.Split(" ");
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
                                lstChatView.Items.Insert(lineNr, new ListViewItem(line.Substring(1, line.Length - 1)));
                                line = "";
                                lineNr++;
                            }
                        }
                        else
                        {
                            //print out the last line
                            lstChatView.Items.Insert(lineNr, new ListViewItem(line.Substring(1, line.Length - 1)));
                            line = "";
                            lineNr++;
                        }
                    }
                }
                //add an empty line for clarity
                lstChatView.Items.Insert(lineNr, new ListViewItem());
                //reset the chat input
                txtChatInput.Text = "";
            })));
        }

        /// <summary>
        /// Add a received message to the connected client
        /// </summary>
        /// /// <param name="messageId">Id of the client</param>
        /// <param name="message">Id of the bike</param>
        /// <param name="name">Name of the person that send the message</param>
        /// <param name="timeSend">The time the message was send in hh:mm:ss format</param>
        public static void ReceiveMessage(byte messageId, string name, string message, string timeSend)
        {
            if (DoctorLogin.doctorScreen._selectedUserId == messageId)
            {
                DoctorLogin.doctorScreen.Invoke(new Action(new Action(() => {
                    DoctorLogin.doctorScreen.AddChatMessage(message, name, timeSend);
                })));
            }

            screens.ForEach(doctorScreen =>
            {
                if (doctorScreen._selectedUserId == messageId)
                {
                    doctorScreen.Invoke(new Action(new Action(() => {
                        doctorScreen.Invoke(new Action(new Action(() => {
                            doctorScreen.AddChatMessage(message, name, timeSend);
                        })));
                    })));
                }
            });
        }

        /// <summary>
        /// Request a new list of clients from the server
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAvailableClients();
            Console.WriteLine("Refreshing list");
        }

        /// <summary>
        /// Select the clients selected in lstClients2 and then open a window for each and allow bikedata to be send to them
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {

            Console.WriteLine("load button clicked");

            if (lstClients2.SelectedItems.Count > 0)
            {
                foreach (DoctorScreen screen in screens)
                {
                    screen.Close();
                }

                screens.Clear();
                SetConnectedUser(0, string.Empty);

                foreach (string selectedClient in lstClients2.SelectedItems)
                {
                    foreach (Client client in clientList)
                    {
                        if (selectedClient.Split(" ")[1] == client.Id.ToString())
                        {
                            if (_selectedUserName == string.Empty)
                            {
                                SetConnectedUser(client.Id, client.Name);
                                Text = "Primary Doctor screen, Connected with: " + client.Name;
                            }
                            else
                            {
                                DoctorScreen secondaryScreen = new DoctorScreen(client.Id, client.Name);
                                screens.Add(secondaryScreen);
                                secondaryScreen.Show();
                            }
                        }
                    }
                }

                DoctorScreen.UpdateBikeData(_selectedUserId, 0, 0, 0, 0);
            }
        }
    }
}
