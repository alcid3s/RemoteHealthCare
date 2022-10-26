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
        }
        // btn forward
        private void button1_Click(object sender, EventArgs e)
        {
            if(_index == ReviewedData.Count - 1)
            {
                MessageWriter writer = new MessageWriter(0x54);
                writer.WritePacket(Encoding.UTF8.GetBytes(clientUsername));
                writer.WritePacket(Encoding.UTF8.GetBytes(clientSession));
                DoctorClient.Send(writer.GetBytes());
                _index = UpdateValues(_index);
            }
            else
            {
                _index++;
                UpdateValues(_index);
            }

        }

        private void btnDataBack_Click(object sender, EventArgs e)
        {
            if(_index >= 1)
            {
                _index--;
                UpdateValues(_index);
            }
            
        }

        private void DoctorScreenHistorie_Load(object sender, EventArgs e)
        {

        }

        public static void ChangeValues(decimal elapsedTime, int distanceTravelled, decimal speed, int heartRate)
        {
            BikeData data = new BikeData(elapsedTime, distanceTravelled, speed, heartRate);
            ReviewedData.Add(data);
        }

        private int UpdateValues(int index)
        {
            Console.WriteLine($"_currentSize = {index}, list size: {ReviewedData.Count}");
            bool success = false;
            for(int i = 0; i < 2; i++)
            {
                Thread.Sleep(10);
                if (index < ReviewedData.Count - 1 && index >= 0)
                {
                    index++;
                    txtET.Text = ReviewedData.ElementAt(_index).ElapsedTime.ToString();
                    txtDT.Text = ReviewedData.ElementAt(_index).DistanceTravelled.ToString();
                    txtSpeed.Text = ReviewedData.ElementAt(_index).Speed.ToString();
                    txtHR.Text = ReviewedData.ElementAt(_index).HeartRate.ToString();
                    success = true;
                }
            }
            if (!success)
            {
                Console.WriteLine("Couldnt update value");
            }
            return index;
        }
    }
}
