using System;
using System.Collections.Generic;
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


            List<MessageWriter> writerList = MessageWriter.WriteRsa();
            Queue<byte[]> data = new Queue<byte[]>(writerList.Select(x => x.GetBytes()));

            Console.WriteLine("Transmitting: " + string.Join(";\n", data.Select(x => BitConverter.ToString(x).Replace('-', ' '))));

            string key = "";
            MessageReader reader;
            while (true)
            {
                bool continuing = true;
                reader = new MessageReader(data.Dequeue());
                continuing = reader.ReadByte() == 1;
                key += Encoding.UTF8.GetString(reader.ReadPacket());
                if (!continuing)
                    break;
            }

            MessageWriter writer = new MessageWriter(0x91);
            writer.WriteByte(EncryptionManager.Manager.GetEncryption(0).XorKey1);
            writer.WriteByte(EncryptionManager.Manager.GetEncryption(0).XorKey2);
            writer.WriteInt((int)EncryptionManager.Manager.GetEncryption(0).StartIndex, 4);
            writer.WriteByte((byte)Math.Round(Math.Log(EncryptionManager.Manager.GetEncryption(0).StepLength) / Math.Log(2)));
            byte[] newData = writer.GetBytes(key);

            Console.WriteLine("Transmitting: " + BitConverter.ToString(newData).Replace('-', ' '));

            reader = new MessageReader(newData);

            MessageEncryption encryption = new MessageEncryption(reader.ReadByte(), reader.ReadByte(), (uint)reader.ReadInt(4), reader.ReadByte());
            Console.WriteLine("Decoded keys: " + encryption);
        }
    }
}
