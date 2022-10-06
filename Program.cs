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

                        bikeClient.ClearPanel("panel1");
                        bikeClient.AddTextToPanel("panel1", "Speed: " + Math.Round(bike.Speed, 2) + " m/s", 1);
                        bikeClient.AddTextToPanel("panel1", "Distance traveled: " + bike.DistanceTravelled + "m", 2);
                        bikeClient.AddTextToPanel("panel1", "Elapsed time: " + (int)bike.ElapsedTime + "s", 3);
                        bikeClient.AddTextToPanel("panel1", "Heartrate: " + bike.HeartRate + " bpm", 4);
                        bikeClient.SwapPanelBuffer("panel1");
                    }
                };

                Application.Run(clientScreen);
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

            Console.WriteLine("waiting for route");
            while (!bikeClient.RouteExists(0)) {
                Thread.Sleep(1);
            }
            bikeClient.AddRoad(0);

            bikeClient.AddPanel("panel1");

            for (int i = 1; i < 5; i++)
            {
                decimal[] postion = new decimal[3];
                postion[0] = (decimal)(i*50);
                postion[2] = (decimal)(i*50);

                //byte[] rotation = new byte[3];
                //rotation[1] = new Random(1).Next;
                bikeClient.CreateModel("testModel" + i, "terrain", postion, 2, new decimal[3], "data/NetworkEngine/models/trees/fantasy/tree" + i + ".obj", "", false);
                
            }

            decimal[] position = new decimal[3];
            position[0] = 60;
            position[1] = (decimal)0.2;
            position[2] = 30;

            bikeClient.CreateModel("testHouse", "terrain", position, 4, new decimal[3], "data/NetworkEngine/models/houses/set1/house1.obj", "", true);
            
            

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