﻿using System;
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

            client.SetSkyBox(16);

            client.CreateTerrain();

            client.CreateBike();

            client.saveScene("test.json", true);

            while (true)
            {
                Thread.Sleep(1000);

                client.resetScene();

                Thread.Sleep(1000);

                client.loadScene("test.json");
            }



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