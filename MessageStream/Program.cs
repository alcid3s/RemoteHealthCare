using System;
using System.Text;

namespace MessageStream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageWriter writer = new MessageWriter(0x10);
            writer.WritePacket(Encoding.UTF8.GetBytes("Pieter Post"));
            writer.WritePacket(Encoding.UTF8.GetBytes("wachtwoord1234"));

            MessageReader reader = new MessageReader(writer.GetBytes());
            Console.WriteLine("0x" + BitConverter.ToString(new byte[] { reader.Id }));
            Console.WriteLine(Encoding.UTF8.GetString(reader.ReadPacket()));
            Console.WriteLine(Encoding.UTF8.GetString(reader.ReadPacket()));
        }
    }
}
