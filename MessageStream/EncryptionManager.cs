using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessageStream
{
    public class EncryptionManager
    {
        public static EncryptionManager Manager { get; private set; }

        private MessageEncryption[] _encryptions;

        public bool IsServer { get; private set; }

        public EncryptionManager(bool isServer)
        {
            IsServer = isServer;
            _encryptions = new MessageEncryption[isServer ? 256 : 1];
            Manager = this;
        }

        public MessageEncryption GetEncryption(byte address)
        {
            if (!IsServer)
                return _encryptions[0] ?? new MessageEncryption();
            return _encryptions[address] ?? new MessageEncryption();
        }

        public void GenerateEncryption(byte address)
        {
            if (!IsServer)
                throw new InvalidOperationException();

            _encryptions[address] = MessageEncryption.Generate();
        }

        public void SetEncryption(MessageEncryption encryption)
        {
            if (IsServer)
                throw new InvalidOperationException();

            _encryptions[0] = encryption;
        }
    }
}
