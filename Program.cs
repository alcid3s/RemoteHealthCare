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
            List<Byte> dataByteList = e.Data.ToList();
            List<string> dataTypeList = new List<string>();

            string[] dataTypeTempList = {"Type", "Elapsed Time", "Distance Travelled",
            "Speed", "Heart Rate", "Extra Info", "Checksum"};
            dataTypeList.AddRange(dataTypeTempList);

            bool newInfo = false;
            int i = 0;
            dataByteList.ForEach(byteType =>
            {
                if (byteType == 16 && !newInfo)
                {
                    newInfo = true;
                    Console.WriteLine($"{dataTypeList.ElementAt(i)}: {byteType}");
                    i++;
                }

                if (newInfo)
                {
                    if (i < dataTypeList.Count)
                    {
                        Console.WriteLine($"{dataTypeList.ElementAt(i)}: {byteType}");
                        i++;
                    }
                }
            });
        }
    }
}