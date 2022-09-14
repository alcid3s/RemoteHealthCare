using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthCare
{
    public delegate void OnUpdate();

    internal interface IBike
    {
        OnUpdate OnUpdate { get; set; }
        decimal ElapsedTime { get; }
        int DistanceTravelled { get; }
        decimal Speed { get; }
        int HeartRate { get; }
    }
}
