using Avans.TI.BLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RemoteHealthCare.Bikes
{
    enum PacketState
    {
        Standard,
        MessageIdentifier,
        Data,
        Checksum
    }
    internal class RealBike : IBike
    {
        public OnUpdate OnUpdate { get; set; }
        public decimal ElapsedTime => _elapsedTime + (this._elapsedTimeOverflow * 64);

        public int DistanceTravelled => _distanceTravelled + (this._distanceTravelledOverflow * 256);

        public decimal Speed => _speed;

        public int HeartRate => _heartRate;

        public bool IsRunning { get; set; }

        private decimal _elapsedTime;
        private int _distanceTravelled;
        private decimal _speed;
        private int _heartRate;

        private int _elapsedTimeOverflow;
        private int _distanceTravelledOverflow;

        private BLE _bike;

        public async void Init()
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

            if (!string.IsNullOrEmpty(tempBikeCode))
                bikeCode = tempBikeCode;

            // Connecting
            code = await bike.OpenDevice($"Tacx Flux {bikeCode}");

            List<BluetoothLEAttributeDisplay> serviceList = bike.GetServices;
            serviceList.ForEach(service =>
            {
                Console.WriteLine($"service: {service}");
            });

            // Set service
            code = await bike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");

            // Subscribe
            bike.SubscriptionValueChanged += UpdateBikeData;
            code = await bike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");

            // Heart rate
            code = await heart.OpenDevice("Decathlon Dual HR");
            await heart.SetService("HeartRate");

            heart.SubscriptionValueChanged += UpdateHeartrateData;
            await heart.SubscribeToCharacteristic("HeartRateMeasurement");
            IsRunning = true;

            _bike = bike;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void UpdateBikeData(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            int y = 0;
            string[] dataTypes = { "Type", "Elapsed Time", "Distance Travelled", "Speed", "Heart Rate", "Extra Info" };
            PacketState state = PacketState.Standard;

            if (!Checksum(e.Data))
                return;

            bool standardPacket = false;
            // Runs through entire packet, beginning with the first 4 bytes which are standard information.
            for (int i = 0; i < e.Data.Count(); i++)
            {
                // Checking if packet with identifier 1 (0x10) shows up. This packet contains useful data.
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
                            this._speed = ((e.Data.ElementAt(i + 1) * 0x100) + e.Data.ElementAt(i)) / 1000m;
                            i++;
                            break;
                        case "Elapsed Time":
                            decimal newElapsedTime = e.Data.ElementAt(i) / 4m;
                            if(newElapsedTime < this._elapsedTime)
                                this._elapsedTimeOverflow++;
                            this._elapsedTime = newElapsedTime;
                            break;
                        case "Distance Travelled":
                            int newDistanceTravelled = e.Data.ElementAt(i);
                            if (newDistanceTravelled < this._distanceTravelled)
                                this._distanceTravelledOverflow++;
                            this._distanceTravelled = newDistanceTravelled;
                            break;
                        case "Heart Rate":
                            break;
                        default:
                            Console.WriteLine($"{dataType}: {e.Data.ElementAt(i)}");
                            break;
                    }
                    OnUpdate();
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
        private void UpdateHeartrateData(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    _heartRate = e.Data.ElementAt((i));
                }
            }
            OnUpdate();
        }

        /// <summary>
        /// Sets the bike's resistance
        /// </summary>
        /// <param name="resistance">The resistance, between 0 and 1 inclusive</param>
        public async void SetResistance(byte resistance)
        {
            Console.WriteLine("trying to write resistance " + resistance + " as " + resistance);
            byte[] values = { 0xa4, 0x09, 0x4e, 0x05, 0x30, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, resistance, (byte)(0xe6 ^ resistance) };
            while (_bike == null) ;
            await _bike.WriteCharacteristic("6e40fec3-b5a3-f393-e0a9-e50e24dcca9e", values);
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