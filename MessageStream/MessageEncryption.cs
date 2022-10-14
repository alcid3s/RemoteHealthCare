using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageStream
{
    public class MessageEncryption
    {
        //must be a single byte
        public byte XorKey1 { get; private set; }
        public byte XorKey2 { get; private set; }
        //can be any integer
        public uint StartIndex { get; private set; }
        //must be 2^k to stay coprime with 8n+1
        public uint StepLength { get; private set; }

        public MessageEncryption() : this(0x00, 0x00, 0, 0) { }

        public MessageEncryption(byte xorKey1, byte xorKey2, uint startIndex, byte stepLength)
        {
            XorKey1 = xorKey1;
            XorKey2 = xorKey2;
            StartIndex = startIndex;
            StepLength = (uint)(1 << stepLength);
        }

        public byte[] Encrypt(byte[] data)
        {
            //Console.WriteLine(BitConverter.ToString(data));
            byte[] initialBytes = data.Select(x => (byte)(x ^ XorKey1)).ToArray();
            //Console.WriteLine(BitConverter.ToString(initialBytes));

            bool[] initialBits = new bool[initialBytes.Length * 8 + 1];
            bool parityBit = false;
            for (int i = 0; i < initialBits.Length - 1; i++)
            {
                initialBits[i] = (initialBytes[i / 8] >> (7 - i % 8) & 1) == 1;
                parityBit ^= initialBits[i];
            }
            initialBits[initialBits.Length - 1] = parityBit;

            //Console.WriteLine(string.Join("", initialBits.Select(x => x ? 1 : 0)));

            bool[] finalBits = new bool[initialBits.Length];
            for (int i = 0; i < initialBits.Length; i++)
            {
                finalBits[i] = initialBits[(StartIndex + i * (StepLength % initialBits.Length)) % initialBits.Length];
            }

            //Console.WriteLine(string.Join("", finalBits.Select(x => x ? 1 : 0)));

            byte[] finalBytes = new byte[finalBits.Length / 8];
            for (int i = 0; i < finalBits.Length - 1; i++)
            {
                finalBytes[i / 8] |= finalBits[i] ? (byte)(1 << (7 - i % 8)) : (byte)0;
            }

            //Console.WriteLine(BitConverter.ToString(finalBytes));
            byte[] result = finalBytes.Select(x => (byte)(x ^ XorKey2)).ToArray();
            //Console.WriteLine(BitConverter.ToString(result));

            return result;
        }

        public byte[] Decrypt(byte[] data)
        {
            //Console.WriteLine(BitConverter.ToString(data));
            byte[] initialBytes = data.Select(x => (byte)(x ^ XorKey2)).ToArray();
            //Console.WriteLine(BitConverter.ToString(initialBytes));

            bool[] initialBits = new bool[initialBytes.Length * 8 + 1];
            bool parityBit = false;
            for (int i = 0; i < initialBits.Length - 1; i++)
            {
                initialBits[i] = (initialBytes[i / 8] >> (7 - i % 8) & 1) == 1;
                parityBit ^= initialBits[i];
            }
            initialBits[initialBits.Length - 1] = parityBit;

            //Console.WriteLine(string.Join("", initialBits.Select(x => x ? 1 : 0)));

            bool[] finalBits = new bool[initialBits.Length];
            for (int i = 0; i < initialBits.Length; i++)
            {
                finalBits[(StartIndex + i * (StepLength % initialBits.Length)) % initialBits.Length] = initialBits[i];
            }

            //Console.WriteLine(string.Join("", finalBits.Select(x => x ? 1 : 0)));

            byte[] finalBytes = new byte[finalBits.Length / 8];
            for (int i = 0; i < finalBits.Length - 1; i++)
            {
                finalBytes[i / 8] |= finalBits[i] ? (byte)(1 << (7 - i % 8)) : (byte)0;
            }

            //Console.WriteLine(BitConverter.ToString(finalBytes));
            byte[] result = finalBytes.Select(x => (byte)(x ^ XorKey1)).ToArray();
            //Console.WriteLine(BitConverter.ToString(result));

            return result;
        }

        internal static MessageEncryption Generate()
        {
            Random random = new Random();
            return new MessageEncryption((byte)random.Next(256), (byte)random.Next(256), (uint)random.Next(), (byte)random.Next(32));
        }

        public override string ToString()
        {
            return BitConverter.ToString(new byte[] { XorKey1, XorKey2 }) + "-" + StartIndex.ToString("X8") + "-" + ((int)Math.Round(Math.Log(StepLength) / Math.Log(2))).ToString("X2");
        }
    }
}
