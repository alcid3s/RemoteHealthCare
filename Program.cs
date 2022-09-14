using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;
using RemoteHealthCare.Bikes;

namespace RemoteHealthCare
{
    class Program
    {
        // Enum is used for presenting the data in the console

        static async Task Main(string[] args)
        {
            /*
            int code = 0;
            BLE bike = new BLE();
            BLE heart = new BLE();
            Thread.Sleep(1000);

            // List available devices
            List<String> bikeList = bike.ListDevices();
            Console.WriteLine("Bikes found: ");
            bikeList.ForEach(device =>
            {
                if (device.Contains("Tacx"))
                    Console.WriteLine($"Device: {device}");
            });

            string bikeCode = "24517";
            Console.Write($"Please enter bike code, leave empty if you want to select bike {bikeCode}\n>: ");
            string tempBikeCode = Console.ReadLine();

            if (tempBikeCode.Length != 0)
                bikeCode = tempBikeCode;

            // Connecting
            code = await bike.OpenDevice($"Tacx Flux {bikeCode}");
            // __TODO__ Error check

            List<BluetoothLEAttributeDisplay> serviceList = bike.GetServices;
            serviceList.ForEach(service =>
            {
                Console.WriteLine($"service: {service}");
            });

            Console.Read();

            // Set service
            code = await bike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");

            // Subscribe
            bike.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            code = await bike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");

            // Heart rate
            code = await heart.OpenDevice("Decathlon Dual HR");

            await heart.SetService("HeartRate");

            heart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            await heart.SubscribeToCharacteristic("HeartRateMeasurement");

            Console.Read();
            Console.Read();
            Console.Read();
            */

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