using RemoteHealthCare.Network;
using System;
using System.Windows.Forms;
using MessageStream;
using System.Reflection;
using System.Threading;
using RemoteHealthCare.Bikes;

namespace RemoteHealthCare.GUI
{
    public partial class ClientScreen : Form
    {
        public bool LocalNetworkEngineRunning { get; set; } = false;
        private IBike _bike;
        public ClientScreen()
        {
            InitializeComponent();
            new Thread(() =>
            {
                StartBike(false);
            }).Start();
            
        }

        AccountLogin accountLogin;
        private void txtSpeed_TextChanged(object sender, EventArgs e)
        {

        }

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

        private void txtElapsedTime_TextChanged(object sender, EventArgs e)
        {

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

        private void txtDistanceTravelled_TextChanged(object sender, EventArgs e)
        {

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

        private void txtHeartRate_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void ClientScreen_Load(object sender, EventArgs e)
        {
            lstBikes.Items.Add("24517");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (accountLogin == null)
            {
                AccountLogin.IsLoggedIn = false;

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

            _bike.Init();
            

            short errorcounter = 0;
            _bike.OnUpdate += delegate
            {
                if (AccountLogin.IsLoggedIn && ServerClient.IsRunning)
                {
                    //ClientScreen clientScreen = new ClientScreen();
                    AccountLogin.ClientScreen.setTxtSpeed(_bike.Speed);
                    AccountLogin.ClientScreen.setTxtDistanceTravelled(_bike.DistanceTravelled);
                    AccountLogin.ClientScreen.setTxtElapsedTime(_bike.ElapsedTime);
                    AccountLogin.ClientScreen.setTxtHeartRate(_bike.HeartRate);
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
    }
}
