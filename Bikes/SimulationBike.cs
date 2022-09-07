using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthCare.Bikes
{
    internal class SimulationBike : IBike
    {
        public OnUpdate OnUpdate { get; set; }

        public decimal ElapsedTime => throw new NotImplementedException();

        public int DistanceTravelled => throw new NotImplementedException();

        public decimal Speed => throw new NotImplementedException();

        public int HeartRate => throw new NotImplementedException();
    }
}
