using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageStream
{

    public class MessageWriter
    {
        private List<byte> _data;
        private bool _closed;

        public MessageWriter(byte id)
        {
            _data = new List<byte>();
            _closed = false;

            WriteByte(255);
            WriteByte(id);
        }

        /// <summary>
        /// Appends a byte to the message
        /// </summary>
        /// <param name="value">The value to append</param>
        public void WriteByte(byte value)
        {
            if (_closed)
                throw new InvalidOperationException();

            _data.Add(value);
        }

        /// <summary>
        /// Appends an integer value to the message
        /// </summary>
        /// <param name="value">The value to append</param>
        /// <param name="byteCount">The amount of bytes to write to</param>
        public void WriteInt(int value, int byteCount)
        {
            if (_closed)
                throw new InvalidOperationException();

            for (int i = 0; i < byteCount; i++)
            {
                WriteByte((byte)((value >> (i * 8)) & 0xFF));
            }
        }

        /// <summary>
        /// Appends a packet to the message
        /// </summary>
        /// <param name="packet">The packet to add</param>
        public void WritePacket(byte[] packet)
        {
            if (_closed)
                throw new InvalidOperationException();

            if (packet.Length > 255)
            {
                throw new ArgumentOutOfRangeException();
            }

            WriteByte((byte)packet.Length);
            foreach (byte value in packet)
            {
                WriteByte(value);
            }
        }

        public void WriteBoolPacket(bool[] packet)
        {
            if (_closed)
                throw new InvalidOperationException();

            if (packet.Length > 256)
            {
                throw new ArgumentOutOfRangeException();
            }

            byte[] bytes = new byte[(packet.Length + 7) / 8];

            for (int i = 0; i < packet.Length; i++)
            {
                bytes[i / 8] |= packet[i] ? (byte)(1 << (7 - i % 8)) : (byte)0;
            }

            WritePacket(bytes);
        }

        /// <summary>
        /// Returns the message to send in bytes
        /// </summary>
        /// <returns>The message to send</returns>
        public byte[] GetBytes()
        {
            if (!_closed)
                Compile();

            return _data.ToArray();
        }

        /// <summary>
        /// Closes all options to change the message and appends a checksum
        /// </summary>
        private void Compile()
        {
            if (_data.Count > 255)
                throw new InternalBufferOverflowException();

            _data[0] = (byte)_data.Count;

            byte checksum = 0;
            foreach (byte value in _data)
            {
                checksum ^= value;
            }

            WriteByte(checksum);

            _closed = true;
        }
    }
}
