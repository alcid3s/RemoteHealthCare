using System;
using System.Linq;
using System.Text;

namespace MessageStream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExtendedMessageWriter writer = new ExtendedMessageWriter(0xFE);
            writer.WriteString("Data kan nu encrypted worden doorgestuurd!");

            byte[] data = writer.GetBytes();
            Console.WriteLine("Transmitting: " + BitConverter.ToString(data).Replace('-', ' '));

            ExtendedMessageReader reader = new ExtendedMessageReader(data);
            Console.WriteLine(reader.ReadString());
        }
    }
}
