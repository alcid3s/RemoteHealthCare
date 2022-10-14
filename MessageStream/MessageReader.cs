using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageStream
{
    public class MessageReader
    {
        private int _index;
        private byte[] _data;

        private byte _id;
        public byte Id { get => GetId(); }

        public MessageReader(byte[] data) : this(data, 0) { }

        public MessageReader(byte[] data, byte address)
        {
            MessageEncryption encryption = EncryptionManager.Manager.GetEncryption(address);

            if (data.Length == 0)
                throw new ArgumentException();

            byte length = data[0];
            if (length + 1 > data.Length)
                throw new ArgumentException();

            _data = new byte[] { data[0] }.Concat(encryption.Decrypt(data.Skip(1).Take(length).ToArray())).ToArray();
            _index = 1;

            _id = ReadByte();
        }

        

        /// <summary>
        /// Reads the current byte and moves the indexer
        /// </summary>
        /// <returns>the currently read byte</returns>
        public byte ReadByte()
        {
            if (!Checksum())
                throw new InvalidOperationException();

            return _data[_index++];
        }

        /// <summary>
        /// Reads an integer in little endian using an amount of bytes equal to <paramref name="byteCount"/>
        /// </summary>
        /// <param name="byteCount">The amount of bytes to read</param>
        /// <returns>The integer represented by the read bytes</returns>
        public int ReadInt(int byteCount)
        {
            if (!Checksum())
                throw new InvalidOperationException();

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
            if (!Checksum())
                throw new InvalidOperationException();

            List<byte> bytes = new List<byte>();
            byte length = ReadByte();
            for (int i = 0; i < length; i++)
            {
                bytes.Add(ReadByte());
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// Reads a packet of bytes and turns them to booleans, with an amount of bytes represented by the first byte
        /// </summary>
        /// <returns>The booleans from the packet</returns>
        public bool[] ReadBoolPacket()
        {
            if (!Checksum())
                throw new InvalidOperationException();

            byte[] bytes = ReadPacket();

            bool[] packet = new bool[bytes.Length * 8];

            for (int i = 0; i < packet.Length; i++)
            {
                packet[i] = (bytes[i / 8] >> (7 - i % 8) & 1) == 1;
            }

            return packet;
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

        private byte GetId()
        {
            if (!Checksum())
                throw new InvalidOperationException();
            return _id;
        }

        public override string ToString()
        {
            string[] text = BitConverter.ToString(_data).Split('-');
            if (_index < _data.Length)
                text[_index] = '>' + text[_index] + '<';
            return string.Join(" ", text);
        }
    }
}
