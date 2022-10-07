using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApllication
{
    internal class ClientData
    {
        public ClientData(decimal elapsedTime, int distance, decimal speed, int heartRate)
        {
            this.elapsedTime = elapsedTime;
            this.distance = distance;
            this.speed = speed;
            this.heartRate = heartRate;
        }

        public decimal elapsedTime { get; set; }
        public int distance { get; set; }
        public decimal speed { get; set; }  
        public int heartRate { get; set; }

    }
}
