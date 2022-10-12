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
        private bool _networkEngineRunning = false;
        private IBike _bike;
        private BikeClient _bikeClient;
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
                    if (counter == 10)
                    {
                        throw new Exception("No reply received");
                    }
                }
                BikeClient.Disconnect();

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

            // Making connection with the VR server
            _bikeClient = new BikeClient();
            _bikeClient.Connect("145.48.6.10", 6666);

            NetworkEngine();

            _bike.Init();
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

                if (_networkEngineRunning && _bike.IsRunning)
                {
                    _bikeClient.ClearPanel("panel1");

                    _bikeClient.AddTextToPanel("panel1", "                SPEED", 1);
                    _bikeClient.AddTextToPanel("panel1", "              " + Math.Round((double)_bike.Speed * 3.6, 1) + " km/u", 2);

                    _bikeClient.AddTextToPanel("panel1", "               " + ((int)_bike.ElapsedTime / 3600).ToString("00") + ":" + ((int)_bike.ElapsedTime / 60).ToString("00") + ":" + ((int)_bike.ElapsedTime % 60).ToString("00"), 3);

                    _bikeClient.AddTextToPanel("panel1", "             DISTANCE", 5);
                    if (_bike.DistanceTravelled < 1000)
                    {
                        _bikeClient.AddTextToPanel("panel1", "                " + _bike.DistanceTravelled + " m", 6);
                    }
                    else
                    {
                        _bikeClient.AddTextToPanel("panel1", "                 " + Math.Round((double)_bike.DistanceTravelled / 1000, 2) + " km", 6);
                    }
                    _bikeClient.AddTextToPanel("panel1", "            HEARTRATE", 8);
                    _bikeClient.AddTextToPanel("panel1", "              " + _bike.HeartRate + " bpm", 9);
                    _bikeClient.SwapPanelBuffer("panel1");
                }
            };
        }

        /// <summary>
        /// Creates a network engine with all required nodes
        /// </summary>
        /// <param name="bikeClient">The client that will receive all the commands</param>
        private void NetworkEngine()
        {
            Console.WriteLine("Resetting scene");
            _bikeClient.ResetScene();
            _bikeClient.GetScene();

            //wait for the getscene response
            while (!_bikeClient.IdReceived("GroundPlane") || !_bikeClient.IdReceived("LeftHand") || !_bikeClient.IdReceived("RightHand") || !_bikeClient.IdReceived("Camera"))
                Thread.Sleep(1);


            //head cant be removed for some reason
            //bikeClient.DeleteNode("Head");

            //Remove the standard nodes
            _bikeClient.DeleteNode("GroundPlane");
            //bikeClient.DeleteNode("LeftHand");
            //bikeClient.DeleteNode("RightHand");

            _bikeClient.SetSkyBox(16);
            _bikeClient.CreateTerrain("terrain");
            _bikeClient.CreateBike("bike");
            //bikeClient.CreateBike("bike2");

            _bikeClient.AddRoute();

            //Add road texture and trees
            Console.WriteLine("waiting for route");
            while (!_bikeClient.RouteExists(0))
            {
                Thread.Sleep(1);
            }
            _bikeClient.AddRoad(0);
            _bikeClient.AddVegetation();

            _bikeClient.AddPanel("panel1");


            //wait for the node and route ids
            Console.WriteLine("waiting for ids");
            //while (!bikeClient.IdReceived("bike") || !bikeClient.RouteExists(0) || !bikeClient.IdReceived("panel1"))
            //    Thread.Sleep(1);
            Thread.Sleep(5000);



            while (!_bikeClient.IdReceived("panel1"))
                Thread.Sleep(1);

            _bikeClient.FollowRoute(0, "Camera");
            _networkEngineRunning = true;
        }
    }
}
