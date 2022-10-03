﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avans.TI.BLE;
using Newtonsoft.Json;

using RemoteHealthCare.Bikes;
using RemoteHealthCare.GUI;
using RemoteHealthCare.Network;

namespace RemoteHealthCare
{
    class Program
    {
        private static bool networkEngineRunning = false;
        static void Main(string[] args)
        { 
            //AccountLogin account = new AccountLogin();
            ClientScreen clientScreen = new ClientScreen();
            //Application.Run(clientScreen);

            // Making connection with the VR server
            BikeClient bikeClient = new BikeClient();
            bikeClient.Connect("145.48.6.10", 6666);

            ServerClient serverClient = new ServerClient("127.0.0.1", 1337);
            serverClient.Connect();

            Thread.Sleep(1000);
            Console.WriteLine("After sleep");

            NetworkEngine(bikeClient);

            // Kind of bikes available
            RealBike realBike = new RealBike();
            SimulationBike simBike = new SimulationBike();

            IBike bike = simBike;

            simBike.IsRunning = true;
            //realBike.Init();

            simBike.OnUpdate += delegate
            {
                if (clientScreen != null)
                {
                             //ClientScreen clientScreen = new ClientScreen();
                    clientScreen.setTxtSpeed(simBike.Speed);
                    clientScreen.setTxtDistanceTravelled(simBike.DistanceTravelled);
                    clientScreen.setTxtElapsedTime(simBike.ElapsedTime);
                    clientScreen.setTxtHeartRate(simBike.HeartRate);
                }
                serverClient.Send(0x21, bike.ElapsedTime, bike.DistanceTravelled, bike.Speed, bike.HeartRate);

                if (networkEngineRunning)
                {
                    bikeClient.UpdateSpeed(bike.Speed);
                }
                         //Console.WriteLine(
                         //    $"Time: {simBike.ElapsedTime}\n" +
                         //    $"Speed: {simBike.Speed}\n" +
                         //    $"Distance: {simBike.DistanceTravelled}\n" +
                         //    $"Heart: {simBike.HeartRate}\n");
            };
            simBike.IsRunning = true;
            //ClientScreen clientScreen = new ClientScreen();
            Application.Run(clientScreen);
            for (; ; );
        }

        private static void NetworkEngine(BikeClient bikeClient)
        {
            bikeClient.ResetScene();
            bikeClient.SetSkyBox(16);
            bikeClient.CreateTerrain("terrain");
            bikeClient.CreateTerrain("terrain");
            bikeClient.CreateBike("bike");
            bikeClient.CreateBike("bike2");

            bikeClient.AddRoute();

            bikeClient.AddPanel("panel1");

            while (!bikeClient.IdReceived("panel1"))
                Thread.Sleep(1);

            bikeClient.AddLineToPanel("panel1");

            bikeClient.AddTextToPanel("panel1");
            bikeClient.GetScene();

            //wait for the node and route id
            Console.WriteLine("waiting for ids");
            while (!bikeClient.IdReceived("bike") || !bikeClient.RouteExists(0))
                Thread.Sleep(1);
            Thread.Sleep(5000);

            //bikeClient.DeleteNode("bike2");
            ////client.DeleteNode("node");

            bikeClient.FollowRoute(0, "bike");
            networkEngineRunning = true;
        }
    }
}