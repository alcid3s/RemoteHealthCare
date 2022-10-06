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
            AccountLogin loginScreen = new AccountLogin();

            ServerClient serverClient = new ServerClient("127.0.0.1", 1337);
            serverClient.Connect();

            Application.Run(loginScreen);

            ClientScreen clientScreen = new ClientScreen();

            // Making connection with the VR server
            BikeClient bikeClient = new BikeClient();
            bikeClient.Connect("145.48.6.10", 6666);

            Thread.Sleep(1000);

            NetworkEngine(bikeClient);

            // Kind of bikes available

            IBike bike = new SimulationBike();
            bike.Init();

            bike.OnUpdate += delegate
            {
                if (clientScreen != null)
                {
                    //ClientScreen clientScreen = new ClientScreen();
                    clientScreen.setTxtSpeed(bike.Speed);
                    clientScreen.setTxtDistanceTravelled(bike.DistanceTravelled);
                    clientScreen.setTxtElapsedTime(bike.ElapsedTime);
                    clientScreen.setTxtHeartRate(bike.HeartRate);
                }
                serverClient.Send(0x21, bike.ElapsedTime, bike.DistanceTravelled, bike.Speed, bike.HeartRate);

                if (networkEngineRunning)
                {
                    bikeClient.UpdateSpeed(bike.Speed);
                }
            };
            
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