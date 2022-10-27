using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageStream
{
    public class ExtendedMessageWriter : MessageWriter
    {
        public ExtendedMessageWriter(byte id) : base(id) { }
        public ExtendedMessageWriter(byte id, byte address) : base(id, address) { }

        /// <summary>
        /// Writes bike data
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds</param>
        /// <param name="distanceTravelled">The travelled distance in meters</param>
        /// <param name="speed">The speed in meters per second</param>
        /// <param name="heartRate">The heart rate in beats per minute</param>
        public void WriteBikeData(decimal elapsedTime, int distanceTravelled, decimal speed, int heartRate)
        {
            WriteInt((short)Math.Round(elapsedTime * 4), 2);
            WriteInt(distanceTravelled, 2);
            WriteInt((short)Math.Round(speed * 1000), 2);
            WriteInt(heartRate, 1);
        }

        /// <summary>
        /// Writes a string
        /// </summary>
        /// <param name="text">The string to write</param>
        public void WriteString(string text)
        {
            WritePacket(Encoding.UTF8.GetBytes(text));
        }
    }
}
