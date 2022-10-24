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
        public static bool NetworkEngineRunning = false;
        public static BikeClient BikeClient;
        public static AccountLogin loginScreen;
        static void Main(string[] args)
        {
            loginScreen = new AccountLogin();

            ServerClient serverClient = new ServerClient("127.0.0.1", 1337);
            serverClient.Connect();

            // Making connection with the VR server
            BikeClient = new BikeClient("145.48.6.10", 6666);
            BikeClient.Connect();

            Thread.Sleep(1000);

            new Thread(() => { NetworkEngine(); }).Start();
            

            Application.Run(loginScreen);
            for (; ; );
        }

        /// <summary>
        /// Creates a network engine with all required nodes
        /// </summary>
        /// <param name="bikeClient">The client that will receive all the commands</param>
        private static void NetworkEngine()
        {
            BikeClient.ResetScene();
            BikeClient.GetScene();

            //wait for the getscene response
            while (!BikeClient.IdReceived("GroundPlane") || !BikeClient.IdReceived("LeftHand") || !BikeClient.IdReceived("RightHand") || !BikeClient.IdReceived("Camera"))
                Thread.Sleep(1);
            //head cant be removed for some reason
            //bikeClient.DeleteNode("Head");

            //Remove the standard nodes
            BikeClient.DeleteNode("GroundPlane");
            //bikeClient.DeleteNode("LeftHand");
            //bikeClient.DeleteNode("RightHand");

            BikeClient.SetSkyBox();
            BikeClient.CreateTerrain("terrain");
            BikeClient.CreateBike("bike");
            //bikeClient.CreateBike("bike2");

            BikeClient.AddRoute();

            //Add road texture and trees
            Console.WriteLine("waiting for route");
            while (!BikeClient.RouteExists(0))
                Thread.Sleep(1);
            
            BikeClient.AddRoad(0);
            BikeClient.AddVegetation();

            BikeClient.AddPanel("panel1");


            //wait for the node and route ids
            Console.WriteLine("waiting for ids");
            //while (!bikeClient.IdReceived("bike") || !bikeClient.RouteExists(0) || !bikeClient.IdReceived("panel1"))
            //    Thread.Sleep(1);
            Thread.Sleep(5000);

            while (!BikeClient.IdReceived("panel1"))
                Thread.Sleep(1);

            BikeClient.FollowRoute(0, "Camera");
            NetworkEngineRunning = true;
        }
    }
}