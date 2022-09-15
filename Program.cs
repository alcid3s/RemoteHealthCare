using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;
using RemoteHealthCare.Bikes;
using RemoteHealthCare.Network;

namespace RemoteHealthCare
{
    class Program
    {
        // Enum is used for presenting the data in the console

        static async Task Main(string[] args)
        {
            // Making connection with the VR server
            Client client = new Client("145.48.6.10", 6666);

            // Kind of bikes available
            SimulationBike simBike = new SimulationBike();
            RealBike realBike = new RealBike();

            IBike bike = realBike;

            realBike.Init();
            //example on how to use delegates; logs info with every update
            bike.OnUpdate += delegate
            {
                Console.WriteLine(
                    $"Time: {bike.ElapsedTime}\n" +
                    $"Speed: {bike.Speed}\n" +
                    $"Distance: {bike.DistanceTravelled}\n" +
                    $"Heart: {bike.HeartRate}\n");
            };

            while (true) ;

            //activates the simulation bike
            // simBike.IsRunning = true;
        }
        

        
    }
}