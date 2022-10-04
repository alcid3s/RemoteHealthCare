﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class MessageWriter
    {
        private List<byte> _data;
        private bool _closed;

        public MessageWriter()
        {
            _data = new List<byte>();
            _closed = false;
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
            byte checksum = 0;
            foreach (byte value in _data)
            {
                checksum ^= value;
            }
            _data.Add(checksum);
        } 
    }
}
