using MessageStream;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void Encryption()
        {
            new EncryptionManager(false);
            MessageEncryption encryption = new MessageEncryption(0x01, 0x23, 0x12345678, 0x10);
            EncryptionManager.Manager.SetEncryption(encryption);

            string sampleText = "Hello World!";

            byte[] encrypted = encryption.Encrypt(Encoding.UTF8.GetBytes(sampleText));

            string decipheredText = Encoding.UTF8.GetString(encryption.Decrypt(encrypted));

            Assert.AreEqual(sampleText, decipheredText, $"Encryption of {sampleText} got incorrectly decrypted as {decipheredText}.");
        }

        [TestMethod]
        public void MessageReadWrite()
        {
            new EncryptionManager(false);
            MessageEncryption encryption = new MessageEncryption(0x98, 0x76, 0x1A2B3C4D, 0x0F);
            EncryptionManager.Manager.SetEncryption(encryption);

            ExtendedMessageWriter writer = new ExtendedMessageWriter(0xFE);
            string sampleText = "Sample Text Here!";
            writer.WriteString(sampleText);
            int sampleNum = 987654321;
            writer.WriteInt(sampleNum, 4);

            ExtendedMessageReader reader = new ExtendedMessageReader(writer.GetBytes());

            Assert.IsTrue(reader.Checksum() && reader.ReadString() == sampleText && reader.ReadInt(4) == sampleNum, $"Message was not received correctly");
        }
    }
}