using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MessageStream
{
    public class ExtendedMessageReader : MessageReader
    {
        public ExtendedMessageReader(byte[] data) : base(data) { }

        /// <summary>
        /// Reads 7 bytes of bike data
        /// </summary>
        /// <returns>All of the bike data in a single tuple</returns>
        public (decimal, int, decimal, int) ReadBikeData()
        {
            return (ReadInt(2) / 4m, ReadInt(2), ReadInt(2) / 1000m, ReadInt(1));
        }

        /// <summary>
        /// Reads a set of bytes as a string
        /// </summary>
        /// <returns>The string that has been read</returns>
        public string ReadString()
        {
            return Encoding.UTF8.GetString(ReadPacket());
        }
    }
}
