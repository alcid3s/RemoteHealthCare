using System;
using System.Linq;
using System.Text;

namespace MessageStream
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new EncryptionManager(false);
            EncryptionManager.Manager.SetEncryption(MessageEncryption.Generate());
            Console.WriteLine("Generated keys: " + EncryptionManager.Manager.GetEncryption(0));

            ExtendedMessageWriter writer = new ExtendedMessageWriter(0xFE);
            writer.WriteString("Data kan nu encrypted worden doorgestuurd!");

            byte[] data = writer.GetBytes();
            Console.WriteLine("Transmitting: " + BitConverter.ToString(data).Replace('-', ' '));

            ExtendedMessageReader reader = new ExtendedMessageReader(data);
            Console.WriteLine(reader.ReadString());
        }
    }
}
