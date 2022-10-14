using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageStream
{
    internal class MessageEncryption
    {
        //must be a single byte
        private static byte s_xorKey1 = 0x69;
        private static byte s_xorKey2 = 0x42;
        //can be any integer
        private static uint s_startIndex = 4200429899;
        //must be 2^k to stay coprime with 8n+1
        private static uint s_stepLength = 1 << 7;

        public static byte[] Encrypt(byte[] data)
        {
            //Console.WriteLine(BitConverter.ToString(data));
            byte[] initialBytes = data.Select(x => (byte)(x ^ s_xorKey1)).ToArray();
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
                finalBits[i] = initialBits[(s_startIndex + i * (s_stepLength % initialBits.Length)) % initialBits.Length];
            }

            //Console.WriteLine(string.Join("", finalBits.Select(x => x ? 1 : 0)));

            byte[] finalBytes = new byte[finalBits.Length / 8];
            for (int i = 0; i < finalBits.Length - 1; i++)
            {
                finalBytes[i / 8] |= finalBits[i] ? (byte)(1 << (7 - i % 8)) : (byte)0;
            }

            //Console.WriteLine(BitConverter.ToString(finalBytes));
            byte[] result = finalBytes.Select(x => (byte)(x ^ s_xorKey2)).ToArray();
            //Console.WriteLine(BitConverter.ToString(result));

            return result;
        }

        public static byte[] Decrypt(byte[] data)
        {
            //Console.WriteLine(BitConverter.ToString(data));
            byte[] initialBytes = data.Select(x => (byte)(x ^ s_xorKey2)).ToArray();
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
                finalBits[(s_startIndex + i * (s_stepLength % initialBits.Length)) % initialBits.Length] = initialBits[i];
            }

            //Console.WriteLine(string.Join("", finalBits.Select(x => x ? 1 : 0)));

            byte[] finalBytes = new byte[finalBits.Length / 8];
            for (int i = 0; i < finalBits.Length - 1; i++)
            {
                finalBytes[i / 8] |= finalBits[i] ? (byte)(1 << (7 - i % 8)) : (byte)0;
            }

            //Console.WriteLine(BitConverter.ToString(finalBytes));
            byte[] result = finalBytes.Select(x => (byte)(x ^ s_xorKey1)).ToArray();
            //Console.WriteLine(BitConverter.ToString(result));

            return result;
        }
    }
}
