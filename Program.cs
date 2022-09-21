using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RemoteHealthCare.Bikes;
using RemoteHealthCare.Network;

namespace RemoteHealthCare
{
    class Program
    {
        // Enum is used for presenting the data in the console

        static void Main(string[] args)
        {
            // Making connection with the VR server
            Client client = new Client();
            _ = client.Connect("145.48.6.10", 6666);

            Thread.Sleep(1000);

            client.SetSkyBox(12.6);

            JObject terrain = JObject.Parse(File.ReadAllText(client.Path + "/terrain.json"));
            terrain["data"]["dest"] = client.Id;

            var heights = terrain["data"]["data"]["data"]["heights"] as JArray;

            for (var i = 0; i < 256; i++)
                 for (var j = 0; j < 256; j++)
                     heights.Add(0);
     
            Console.WriteLine("message: " + terrain);

            client.Send("" + terrain);



            JObject bike = JObject.Parse(File.ReadAllText(client.Path + "/bike.json"));
            bike["data"]["dest"] = client.Id;

            Console.WriteLine("message: " + bike);

            client.Send(bike.ToString());

            //// Kind of bikes available
            //SimulationBike simBike = new SimulationBike();
            //RealBike realBike = new RealBike();

            //IBike bike = realBike;

            //realBike.Init();
            ////example on how to use delegates; logs info with every update
            //bike.OnUpdate += delegate
            //{
            //    Console.WriteLine(
            //        $"Time: {bike.ElapsedTime}\n" +
            //        $"Speed: {bike.Speed}\n" +
            //        $"Distance: {bike.DistanceTravelled}\n" +
            //        $"Heart: {bike.HeartRate}\n");
            //};

            while (true) ;

            //activates the simulation bike
            // simBike.IsRunning = true;
        }
    }
}