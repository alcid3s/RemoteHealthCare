using RemoteHealthCare.Network;
using System;
using System.Windows.Forms;
using MessageStream;
using System.Reflection;
using System.Threading;
using RemoteHealthCare.Bikes;
using System.Text;
using System.Globalization;

namespace RemoteHealthCare.GUI
{
    public partial class ClientScreen : Form
    {
        public bool LocalNetworkEngineRunning { get; set; } = false;
        private IBike _bike;
        public ClientScreen()
        {
            InitializeComponent();
            txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);
            new Thread(() =>
            {
                StartBike(false);
            }).Start();
            
        }

        AccountLogin accountLogin;

        public void setTxtSpeed(decimal s)
        {
            try
            {
                Invoke(new Action(() => txtSpeed.Text = s.ToString("f2")));
            }
            catch (Exception e)
            {

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

                // Sends a message to the server, server send it to the client.
                ExtendedMessageWriter writer = new ExtendedMessageWriter(0x32);
                writer.WriteString(DateTime.Now.TimeOfDay.ToString().Substring(0, 8));
                writer.WriteString(message);
                ServerClient.Send(writer.GetBytes());

                AddChatMessage(txtChatInput.Text, "You", DateTime.Now.TimeOfDay.ToString().Substring(0, 8));
            }
        }

        /// <summary>
        /// Add a message to lstChatView with a sender and time at 40 characters per line
        /// </summary>
        /// <param name="message">Id of the bike</param>
        /// <param name="sender">TName of the person that send the message</param>
        /// <param name="timeSend">The time the message was send in hh:mm:ss format</param>

        public void AddChatMessage(string sender, string message, string timeSend)
        {
            Invoke(new Action(new Action(() =>
            {
                //put the time above the message, can later also have the sender
                lstChatView.Items.Insert(0, new ListViewItem(timeSend + " - " + sender));

                //get all the words from the input
                string[] words = message.Split(' ');
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
        public void setTxtElapsedTime(decimal s)
        {
            try
            {
                Invoke(new Action(() => txtElapsedTime.Text = s.ToString("F2")));
            }
            catch (Exception e)
            {

            }

        }

        public void setTxtDistanceTravelled(decimal s)
        {
            try
            {
                Invoke(new Action(() => txtDistanceTravelled.Text = s.ToString("F0")));
            }
            catch (Exception e)
            {

            }

        }

        public void setTxtHeartRate(decimal s)
        {
            try
            {
                Invoke(new Action(() => txtHeartRate.Text = s.ToString("F0")));
            }
            catch (Exception e)
            {

            }

        }


        private void ClientScreen_Load(object sender, EventArgs e)
        {
            lstBikes.Items.Add("24517");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (accountLogin == null)
            {
                AccountLogin.isloggedIn = false;

                ServerClient.Send(new MessageWriter(0x60).GetBytes());

                int counter = 0;
                int max = 10;
                ServerClient.Reply = 0x00;
                while (ServerClient.Reply == 0x00)
                {
                    Thread.Sleep(100);
                    counter++;
                    if (counter == 50)
                    {
                        throw new Exception("No reply received");
                    }
                }
                Program.BikeClient.Disconnect();

                if(_bike != null)
                {
                    _bike.Stop();
                    _bike = null;
                }
                accountLogin = new AccountLogin();
                Hide();
                accountLogin.Show();
            }
        }

        private void StartBike(bool realBike)
        {
            if (realBike)
                _bike = new RealBike();
            else
                _bike = new SimulationBike();

            if(!BikeClient.Connected)
                Program.BikeClient.Connect();

            Thread.Sleep(1000);

            _bike.Init();

            short errorcounter = 0;
            _bike.OnUpdate += delegate
            {
                if (AccountLogin.isloggedIn && ServerClient.IsRunning)
                {
                    //ClientScreen clientScreen = new ClientScreen();
                    AccountLogin.clientScreen.setTxtSpeed(_bike.Speed);
                    AccountLogin.clientScreen.setTxtDistanceTravelled(_bike.DistanceTravelled);
                    AccountLogin.clientScreen.setTxtElapsedTime(_bike.ElapsedTime);
                    AccountLogin.clientScreen.setTxtHeartRate(_bike.HeartRate);
                    ServerClient.Send(0x20, _bike.ElapsedTime, _bike.DistanceTravelled, _bike.Speed, _bike.HeartRate);
                }


                if (LocalNetworkEngineRunning)
                {
                    try
                    {
                        errorcounter = 0;
                        Program.BikeClient.UpdateSpeed(_bike.Speed);

                        Program.BikeClient.ClearPanel("panel1");

                        Program.BikeClient.AddTextToPanel("panel1", "                SPEED", 1);
                        Program.BikeClient.AddTextToPanel("panel1", "              " + Math.Round((double)_bike.Speed * 3.6, 1) + " km/u", 2);

                        Program.BikeClient.AddTextToPanel("panel1", "               " + ((int)_bike.ElapsedTime / 3600).ToString("00") + ":" + ((int)_bike.ElapsedTime / 60).ToString("00") + ":" + ((int)_bike.ElapsedTime % 60).ToString("00"), 3);

                        Program.BikeClient.AddTextToPanel("panel1", "             DISTANCE", 5);
                        if (_bike.DistanceTravelled < 1000)
                        {
                            Program.BikeClient.AddTextToPanel("panel1", "                " + _bike.DistanceTravelled + " m", 6);
                        }
                        else
                        {
                            Program.BikeClient.AddTextToPanel("panel1", "                 " + Math.Round((double)_bike.DistanceTravelled / 1000, 2) + " km", 6);
                        }
                        Program.BikeClient.AddTextToPanel("panel1", "            HEARTRATE", 8);
                        Program.BikeClient.AddTextToPanel("panel1", "              " + _bike.HeartRate + " bpm", 9);
                        Program.BikeClient.SwapPanelBuffer("panel1");
                    }
                    catch (Exception e)
                    {
                        errorcounter++;
                    }

                    if(errorcounter > 30)
                    {
                        LocalNetworkEngineRunning = false;
                    }
                }
                    
            };
        }

        private void txtChatInput_TextChanged_1(object sender, EventArgs e)
        {
            if (txtChatInput.Text.Length > 200)
            {
                txtChatInput.Text = txtChatInput.Text.Substring(0, 200);
                txtInfo.Text = "message can have up to 200 characters";
            }
        }
    }
}
