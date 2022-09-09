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
        enum PacketState
        {
            Standard,
            MessageIdentifier,
            Data,
            Checksum
        }
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

            SimulationBike simBike = new SimulationBike();

            IBike bike = simBike;
            //example on how to use delegates; logs info with every update
            bike.OnUpdate += delegate 
            {
                Console.WriteLine(
                    $"Time: {bike.ElapsedTime}\n" +
                    $"Speed: {bike.Speed}\n" +
                    $"Distance: {bike.DistanceTravelled}\n" +
                    $"Heart: {bike.HeartRate}\n");
            };

            //activates the simulation bike
            simBike.IsRunning = true;
        }

        private static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            Console.WriteLine($"Packet contains: {e.Data.Count()} bytes");

            int y = 0;
            string[] dataTypes = { "Type", "Elapsed Time", "Distance Travelled", "Speed", "Heart Rate", "Extra Info" };
            PacketState state = PacketState.Standard;

            if (!Checksum(e.Data))
                return;

            bool standardPacket = false;
            // Runs through entire packet, beginning with the first 4 bytes which are standard information.
            for (int i = 0; i < e.Data.Count(); i++)
            {
                // Checking if packet with identifier 1 (0x10) is at location i.
                if (state == PacketState.MessageIdentifier)
                    standardPacket = e.Data.ElementAt(i) == 0x10;

                // printing the data with the corresponding value.
                else if (state == PacketState.Data && standardPacket)
                {
                    string dataType = dataTypes.ElementAt(y);
                    switch (dataType)
                    {
                        // Speed is 4 bytes, all other data are 2 bytes
                        case "Speed":
                            decimal speed = ((e.Data.ElementAt(i + 1) * 0x100) + e.Data.ElementAt(i)) / 1000m;
                            Console.WriteLine($"{dataType}: {speed} m/s");
                            i++;
                            break;
                        case "Elapsed Time":
                            Console.WriteLine($"{dataType}: {e.Data.ElementAt(i) / 4m} seconds");
                            break;
                        case "Distance Travelled":
                            Console.WriteLine($"{dataType}: {e.Data.ElementAt(i)} meters");
                            break;
                        case "Heart Rate":
                            if(e.Data.ElementAt(i) != 0xFF)
                            {
                                Console.WriteLine($"{dataType}: {e.Data.ElementAt(i)} bpm");
                            }
                            break;
                        default:
                            Console.WriteLine($"{dataType}: {e.Data.ElementAt(i)}");
                            break;
                    }
                    y++;
                }

                // Check if the part of the packet checked has changed
                switch (i)
                {
                    case 3:
                        state = PacketState.MessageIdentifier;
                        break;
                    case 4:
                        state = PacketState.Data;
                        break;
                    case 11:
                        state = PacketState.Checksum;
                        break;
                }
            }
        }

        private static bool Checksum(byte[] bytes)
        {
            byte checksum = 0;

            foreach (byte b in bytes)
                checksum ^= b;

            return checksum == 0;
        }
    }
}