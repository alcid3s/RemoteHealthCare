using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace RemoteHealthCare
{
    class Program
    {
        // Enum is used for presenting the data in the console
        enum PacketDataState
        {
            Standard,
            MessageIdentifier,
            Data,
            Checksum
        }
        static async Task Main(string[] args)
        {
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
        }

        private static void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            Console.WriteLine($"Packet contains: {e.Data.Count()} bytes");

            int y = 0;
            string[] dataTypes = { "Type", "Elapsed Time", "Distance Travelled", "Speed", "Heart Rate", "Extra Info" };
            PacketDataState state = PacketDataState.Standard;

            // Runs through entire packet, beginning with the first 4 bytes which are standard information.
            for (int i = 0; i < e.Data.Count(); i++)
            {
                // printing the data with the corresponding value.
                if (state == PacketDataState.Data)
                {
                    // Speed is 4 bytes
                    if (dataTypes.ElementAt(y).Equals("Speed"))
                    {
                        int speed = e.Data.ElementAt(i + 1) + e.Data.ElementAt(i);
                        Console.WriteLine($"{dataTypes.ElementAt(y)}: {speed}");
                        i++;
                    }

                    // Otherwise just show the value to the corresponding data type.
                    else
                    {
                        Console.WriteLine($"{dataTypes.ElementAt(y)}: {e.Data.ElementAt(i)}");
                    }
                    y++;
                }

                // Check if the part of the packet checked has changed
                switch (i)
                {
                    case 3:
                        state = PacketDataState.MessageIdentifier;
                        break;
                    case 4:
                        state = PacketDataState.Data;
                        break;
                    case 11:
                        state = PacketDataState.Checksum;
                        break;
                }
            }
        }
    }
}