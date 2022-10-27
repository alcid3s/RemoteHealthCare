using DoctorApllication;
using MessageStream;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorApplication
{
    public partial class DoctorScreenHistorie : Form
    {
        public string clientUsername { private get; set; } = "";
        public string clientSession { private get; set; } = "";

        public static List<BikeData> ReviewedData = new List<BikeData>();
        private int _currentSize = 0;
        private int _index = 0;
        public struct BikeData
        {
            public decimal ElapsedTime { get; set; }
            public int DistanceTravelled { get; set; }
            public decimal Speed { get; set; }
            public int HeartRate { get; set; }

            public BikeData(decimal time, int distance, decimal speed, int hr)
            {
                ElapsedTime = time;
                DistanceTravelled = distance;
                Speed = speed;
                HeartRate = hr;
            }
        }

        public DoctorScreenHistorie()
        {
            InitializeComponent();
            new Thread(UpdateValues).Start();
        }
        // btn forward
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Index: {_index}, list size: {ReviewedData.Count}");
            if (_index == ReviewedData.Count)
            {
                MessageWriter writer = new MessageWriter(0x54);
                writer.WritePacket(Encoding.UTF8.GetBytes(clientUsername));
                writer.WritePacket(Encoding.UTF8.GetBytes(clientSession));
                DoctorClient.Send(writer.GetBytes());
                Console.WriteLine("Send message to get more data");
            }
            else if (_index < ReviewedData.Count)
            {
                _index++;
                SetValues();
            }

        }

        private void btnDataBack_Click(object sender, EventArgs e)
        {
            if (_index >= 1 && _index <= ReviewedData.Count)
            {
                _index--;
                SetValues();
            }

        }

        private void DoctorScreenHistorie_Load(object sender, EventArgs e)
        {

        }

        public static void ChangeValues(decimal elapsedTime, int distanceTravelled, decimal speed, int heartRate)
        {
            BikeData data = new BikeData(elapsedTime / 4m, distanceTravelled, speed / 1000m, heartRate);
            ReviewedData.Add(data);
        }

        private void UpdateValues()
        {
            while (true)
            {
                // If a new request was answered
                if (_currentSize < ReviewedData.Count)
                {
                    _currentSize++;
                    _index++;
                    Invoke(new Action(new Action(() =>
                    {
                        SetValues();
                    })));
                    Console.WriteLine($"Values changed, index: {_index}, size: {ReviewedData.Count}");
                }
            }
        }

        private void SetValues()
        {
            if(_index > 0)
            {
                txtET.Text = ReviewedData.ElementAt(_index - 1).ElapsedTime.ToString();
                txtDT.Text = ReviewedData.ElementAt(_index - 1).DistanceTravelled.ToString();
                txtSpeed.Text = ReviewedData.ElementAt(_index - 1).Speed.ToString();
                txtHR.Text = ReviewedData.ElementAt(_index - 1).HeartRate.ToString();
            }
        }
    }
}
