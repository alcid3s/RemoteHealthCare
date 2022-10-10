using System;
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

            // Making connection with the VR server
            BikeClient bikeClient = new BikeClient();
            bikeClient.Connect("145.48.6.10", 6666);

            Thread.Sleep(1000);

            //NetworkEngine(bikeClient);

            // Kind of bikes available

            IBike bike = new SimulationBike();
            bike.Init();
            bike.OnUpdate += delegate
            {
                if (AccountLogin.IsLoggedIn)
                {
                    //ClientScreen clientScreen = new ClientScreen();
                    AccountLogin.ClientScreen.setTxtSpeed(bike.Speed);
                    AccountLogin.ClientScreen.setTxtDistanceTravelled(bike.DistanceTravelled);
                    AccountLogin.ClientScreen.setTxtElapsedTime(bike.ElapsedTime);
                    AccountLogin.ClientScreen.setTxtHeartRate(bike.HeartRate);
                    serverClient.Send(0x20, bike.ElapsedTime, bike.DistanceTravelled, bike.Speed, bike.HeartRate);
                }

                if (networkEngineRunning)
                {
                    bikeClient.ClearPanel("panel1");

                    bikeClient.AddTextToPanel("panel1", "                SPEED", 1);
                    bikeClient.AddTextToPanel("panel1", "              " + Math.Round((double)bike.Speed * 3.6, 1) + " km/u", 2);

                    bikeClient.AddTextToPanel("panel1", "               " + ((int)bike.ElapsedTime / 3600).ToString("00") + ":" + ((int)bike.ElapsedTime / 60).ToString("00") + ":" + ((int)bike.ElapsedTime % 60).ToString("00"), 3);

                    bikeClient.AddTextToPanel("panel1", "             DISTANCE", 5);
                    if (bike.DistanceTravelled < 1000)
                    {
                        bikeClient.AddTextToPanel("panel1", "                " + bike.DistanceTravelled + " m", 6);
                    }
                    else
                    {
                        bikeClient.AddTextToPanel("panel1", "                 " + Math.Round((double)bike.DistanceTravelled / 1000, 2) + " km", 6);
                    }
                    bikeClient.AddTextToPanel("panel1", "            HEARTRATE", 8);
                    bikeClient.AddTextToPanel("panel1", "              " + bike.HeartRate + " bpm", 9);
                    bikeClient.SwapPanelBuffer("panel1");
                }

            };

            Application.Run(loginScreen);
            for (; ; );
        }

        /// <summary>
        /// Creates a network engine with all required nodes
        /// </summary>
        /// <param name="bikeClient">The client that will receive all the commands</param>
        private static void NetworkEngine(BikeClient bikeClient)
        {
            bikeClient.ResetScene();
            bikeClient.GetScene();

            //wait for the getscene response
            while (!bikeClient.IdReceived("GroundPlane") || !bikeClient.IdReceived("LeftHand") || !bikeClient.IdReceived("RightHand") || !bikeClient.IdReceived("Camera"))
                Thread.Sleep(1);

            //head cant be removed for some reason
            //bikeClient.DeleteNode("Head");

            //Remove the standard nodes
            bikeClient.DeleteNode("GroundPlane");
            //bikeClient.DeleteNode("LeftHand");
            //bikeClient.DeleteNode("RightHand");

            bikeClient.SetSkyBox(16);
            bikeClient.CreateTerrain("terrain");
            bikeClient.CreateBike("bike");
            //bikeClient.CreateBike("bike2");

            bikeClient.AddRoute();

            //Add road texture and trees
            Console.WriteLine("waiting for route");
            while (!bikeClient.RouteExists(0))
            {
                Thread.Sleep(1);
            }
            bikeClient.AddRoad(0);
            bikeClient.AddVegetation();

            bikeClient.AddPanel("panel1");


            //wait for the node and route ids
            Console.WriteLine("waiting for ids");
            //while (!bikeClient.IdReceived("bike") || !bikeClient.RouteExists(0) || !bikeClient.IdReceived("panel1"))
            //    Thread.Sleep(1);
            Thread.Sleep(5000);



            while (!bikeClient.IdReceived("panel1"))
                Thread.Sleep(1);

            bikeClient.FollowRoute(0, "Camera");
            networkEngineRunning = true;
        }
    }
}