// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Keys;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public class SSHKeys : ICryptographyKeyBroker
    {
        public ValueTask<Key> GenerateKeys(string publicKeyComment)
        {
            int keyBits = 2048;

            var keygen = new SshKeyGenerator.SshKeyGenerator(keyBits);

            Key key = new Key
            {
                Base64PublicKey = keygen.ToRfcPublicKey(publicKeyComment),
                Base64PrivateKey = keygen.ToPrivateKey(),
            };

            return ValueTask.FromResult(key);
        }

        public ValueTask<Key> GenerateKeys() =>
            this.GenerateKeys($"Auto Generated Comment {DateTimeOffset.UtcNow} ");
    }
}
