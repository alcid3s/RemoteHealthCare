using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class MessageReader
    {
        private int _index;
        private byte[] _data;

        public MessageReader(byte[] data)
        {
            _data = data;
            _index = 0;
        }

        /// <summary>
        /// Reads the current byte and moves the indexer
        /// </summary>
        /// <returns>the currently read byte</returns>
        public byte ReadByte()
        {
            return _data[_index++];
        }

        /// <summary>
        /// Reads an integer in little endian using an amount of bytes equal to <paramref name="byteCount"/>
        /// </summary>
        /// <param name="byteCount">The amount of bytes to read</param>
        /// <returns>The integer represented by the read bytes</returns>
        public int ReadInt(int byteCount)
        {
            int value = 0;
            for (int i = 0; i < byteCount; i++)
            {
                value |= ReadByte() << (i * 8);
            }
            return value;
        }

        /// <summary>
        /// Reads a packet of bytes, with a length represented by the first
        /// </summary>
        /// <returns>The raw bytes from the packet</returns>
        public byte[] ReadPacket()
        {
            List<byte> bytes = new List<byte>();
            byte length = ReadByte();
            for (int i = 0; i < length; i++)
            {
                bytes.Add(ReadByte());
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// Validates a message using the checksum
        /// </summary>
        /// <returns>Whether the message is functional or not</returns>
        public bool Checksum()
        {
            byte checksum = 0;
            foreach (byte value in _data)
            {
                checksum ^= value;
            }
            return checksum == 0;
        }
    }
}
