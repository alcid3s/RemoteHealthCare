using System;
using System.Linq;
using System.Text;

namespace MessageStream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExtendedMessageWriter writer = new ExtendedMessageWriter(0x43);
            writer.WriteString("Hallo Wereld!");
            writer.WriteBikeData(65.25m, 120, 4.53m, 110);

            Console.WriteLine(writer.ToString());

            ExtendedMessageReader reader = new ExtendedMessageReader(writer.GetBytes());


            Console.WriteLine("0x" + BitConverter.ToString(new byte[] { reader.Id }));
            Console.WriteLine(reader);
            Console.WriteLine(reader.ReadString());
            Console.WriteLine(reader);
            Console.WriteLine(reader.ReadBikeData());
            Console.WriteLine(reader);
        }
    }
}
