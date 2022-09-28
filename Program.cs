using System;
using System.Threading;

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
            BikeClient bikeClient = new BikeClient();
            _ = bikeClient.Connect("145.48.6.10", 6666);

            ServerClient serverClient = new ServerClient("127.0.0.1", 1337);
            serverClient.Connect();

            Thread.Sleep(1000);

            NetworkEngine(bikeClient);

            // Kind of bikes available
            SimulationBike simBike = new SimulationBike();
            RealBike realBike = new RealBike();

            IBike bike = simBike;

            simBike.IsRunning = true;
            //realBike.Init();
            //example on how to use delegates; logs info with every update
            bike.OnUpdate += delegate
            {
                serverClient.Send(bike.ElapsedTime, bike.DistanceTravelled, bike.Speed, bike.HeartRate);

                //Console.WriteLine(
                //    $"Time: {bike.ElapsedTime}\n" +
                //    $"Speed: {bike.Speed}\n" +
                //    $"Distance: {bike.DistanceTravelled}\n" +
                //    $"Heart: {bike.HeartRate}\n");
            };

            //while (true) ;
            for (; ; );

            //activates the simulation bike
            
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

            bikeClient.DeleteNode("bike2");
            //client.DeleteNode("node");

            bikeClient.FollowRoute(0, "bike");
        }
    }
}