using RemoteHealthCare.Network;
using System;
using System.Windows.Forms;
using MessageStream;
using System.Reflection;
using System.Threading;
using RemoteHealthCare.Bikes;
using Avans.TI.BLE;
using System.Collections.Generic;
using System.Linq;

namespace RemoteHealthCare.GUI
{
    public partial class ClientScreen : Form
    {
        public OnUpdate OnUpdate { get; set; }
        public int HeartRate => _heartRate;

        public bool IsRunning { get; set; }

        private decimal _elapsedTime;
        private int _distanceTravelled;
        private decimal _speed;
        private int _heartRate;

        private int _elapsedTimeOverflow;
        private int _distanceTravelledOverflow;
        public bool LocalNetworkEngineRunning { get; set; } = false;
        private IBike _bike;
        public ClientScreen()
        {
            InitializeComponent();
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
            int code = 0;
            BLE bike = new BLE();
            BLE heart = new BLE();
            Thread.Sleep(1000);

            List<string> deviceList = bike.ListDevices();
            List<string> bikeList = new List<string>();

            deviceList.ForEach(device =>
            {
                if (device.Contains("Tacx"))
                    bikeList.Add(device);
            });

            bikeList.ForEach(device =>
            {
                lstBikes.Items.Add(device);
            });

            lstBikes.Items.Add("SimBike");
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

        private void lstBikes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Connect button
        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedBike = lstBikes.SelectedItem.ToString();
            if(selectedBike != null)
            {
                if (selectedBike.Equals("SimBike"))
                {
                    new Thread(() =>
                    {
                        StartBike(false);
                    }).Start();
                }
                else
                {
                    int code = 0;
                    BLE bike = new BLE();
                    BLE heart = new BLE();

                    code = await bike.OpenDevice($"Tacx Flux {selectedBike}");

                    List<BluetoothLEAttributeDisplay> serviceList = bike.GetServices;

                    // Set service
                    code = await bike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");

                    // Subscribe
                    bike.SubscriptionValueChanged += UpdateBikeData;
                    code = await bike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");

                    // Heart rate
                    code = await heart.OpenDevice("Decathlon Dual HR");
                    await heart.SetService("HeartRate");

                    heart.SubscriptionValueChanged += UpdateHeartrateData;
                    await heart.SubscribeToCharacteristic("HeartRateMeasurement");
                    IsRunning = true;
                }
            }
        }

        private void UpdateBikeData(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            int y = 0;
            string[] dataTypes = { "Type", "Elapsed Time", "Distance Travelled", "Speed", "Heart Rate", "Extra Info" };
            PacketState state = PacketState.Standard;

            if (!Checksum(e.Data))
                return;

            bool standardPacket = false;
            // Runs through entire packet, beginning with the first 4 bytes which are standard information.
            for (int i = 0; i < e.Data.Count(); i++)
            {
                // Checking if packet with identifier 1 (0x10) shows up. This packet contains useful data.
                if (state == PacketState.MessageIdentifier)
                    standardPacket = e.Data.ElementAt(i) == 0x10;

                // printing the data with the corresponding value.
                else if (state == PacketState.Data && standardPacket)
                {
                    string dataType = dataTypes.ElementAt(y);
                    switch (dataType)
                    {
                        // Speed is 4 bytes, all other data are 2 bytes
                        case "Speed":
                            this._speed = ((e.Data.ElementAt(i + 1) * 0x100) + e.Data.ElementAt(i)) / 1000m;
                            i++;
                            break;
                        case "Elapsed Time":
                            decimal newElapsedTime = e.Data.ElementAt(i) / 4m;
                            if (newElapsedTime < this._elapsedTime)
                                this._elapsedTimeOverflow++;
                            this._elapsedTime = newElapsedTime;
                            break;
                        case "Distance Travelled":
                            int newDistanceTravelled = e.Data.ElementAt(i);
                            if (newDistanceTravelled < this._distanceTravelled)
                                this._distanceTravelledOverflow++;
                            this._distanceTravelled = newDistanceTravelled;
                            break;
                        case "Heart Rate":
                            break;
                        default:
                            Console.WriteLine($"{dataType}: {e.Data.ElementAt(i)}");
                            break;
                    }
                    OnUpdate();
                    y++;
                }

                // Check if the part of the packet checked has changed
                switch (i)
                {
                    case 3:
                        state = PacketState.MessageIdentifier;
                        break;
                    case 4:
                        state = PacketState.Data;
                        break;
                    case 11:
                        state = PacketState.Checksum;
                        break;
                }
            }
        }
        private void UpdateHeartrateData(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    _heartRate = e.Data.ElementAt((i));
                }
            }
            OnUpdate();
        }

        private static bool Checksum(byte[] bytes)
        {
            byte checksum = 0;

            foreach (byte b in bytes)
                checksum ^= b;

            return checksum == 0;
        }
    }
}
