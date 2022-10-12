using System;
using System.Linq;
using System.Text;

namespace MessageStream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageWriter writer = new MessageWriter(0x43);
            writer.WriteBoolPacket(new bool[] { true, false, false, true, true, true, false, false, true });

            MessageReader reader = new MessageReader(writer.GetBytes());
            Console.WriteLine("0x" + BitConverter.ToString(new byte[] { reader.Id }));
            Console.WriteLine(string.Join("", reader.ReadBoolPacket().Select(x => x ? 1 : 0)));
        }
    }
}
