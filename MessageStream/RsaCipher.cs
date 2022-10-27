using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MessageStream
{
    //inspired by https://stackoverflow.com/questions/17128038/c-sharp-rsa-encryption-decryption-with-transmission
    internal class RsaCipher
    {
        private static RSAParameters _privateKey;

        public static string PublicKeyString { get; private set; }

        public static void SetCipher()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);

            _privateKey = rsa.ExportParameters(true);
            RSAParameters publicKey = rsa.ExportParameters(false);

            //converting the public key into a string representation
            StringWriter sw = new StringWriter();
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, publicKey);
            PublicKeyString = sw.ToString();
        }

        public static byte[] Encrypt(string publicKeyString, byte[] data)
        {
            StringReader sr = new StringReader(publicKeyString);
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            RSAParameters publicKey = (RSAParameters)xs.Deserialize(sr);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(publicKey);

            return rsa.Encrypt(data, false);
        }

        public static byte[] Decrypt(byte[] data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(_privateKey);

            return rsa.Decrypt(data, false);
        }
    }
}
