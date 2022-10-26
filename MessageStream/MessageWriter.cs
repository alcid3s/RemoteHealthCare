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

        private byte _address = 0;

        public MessageWriter(byte id) : this(id, 0) { }

        public MessageWriter(byte id, byte address)
        {
            _address = address;

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

        /// <summary>
        /// Appends a packet of booleans to the message
        /// This may add extra false entries to complete incomplete bytes
        /// </summary>
        /// <param name="packet">The packet to add</param>
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

        public static List<MessageWriter> WriteRsa()
        {
            RsaCipher.SetCipher();

            List<MessageWriter> messageWriters = new List<MessageWriter>();

            for (int i = 0; i < (RsaCipher.PublicKeyString.Length + 249) / 250; i++)
            {
                MessageWriter writer = new MessageWriter(0x90);
                writer.WriteByte(i < (RsaCipher.PublicKeyString.Length - 1) / 250 ? (byte)1 : (byte)0);
                writer.WritePacket(Encoding.UTF8.GetBytes(RsaCipher.PublicKeyString.Substring(i * 250, Math.Min(250, RsaCipher.PublicKeyString.Length - i * 250))));
                messageWriters.Add(writer);
            }

            return messageWriters;
        }

        /// <summary>
        /// Returns the message to send in bytes
        /// </summary>
        /// <returns>The message to send</returns>
        public byte[] GetBytes()
        {
            return GetBytes(null);
        }

        /// <summary>
        /// Returns the message to send in bytes
        /// </summary>
        /// <param name="rsaKey">the key for RSA encryption</param>
        /// <returns>The message to send</returns>
        public byte[] GetBytes(string rsaKey)
        {
            if (!_closed)
                Compile(rsaKey);

            return _data.ToArray();
        }

        /// <summary>
        /// Closes all options to change the message and appends a checksum
        /// </summary>
        private void Compile(string key)
        {
            if (_data.Count > 255)
                throw new InternalBufferOverflowException();

            byte checksum = 0xFF;
            foreach (byte value in _data)
            {
                checksum ^= value;
            }

            WriteByte(checksum);

            //The length can't be encrypted because decryption is length-dependent
            if (key != null)
                _data = _data.Take(2).Concat(RsaCipher.Encrypt(key, _data.Skip(2).ToArray())).ToList();
            else if (_data[1] != 0x91)
                _data = _data.Take(2).Concat(EncryptionManager.Manager.GetEncryption(_address).Encrypt(_data.Skip(2).ToArray())).ToList();

            _data[0] = (byte)(_data.Count - 1);

            _closed = true;
        }

        public override string ToString()
        {
            return BitConverter.ToString(GetBytes()).Replace('-', ' ');
        }
    }
}
