// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public class SshKeyBroker : ICryptographyKeyBroker
    {
        public async ValueTask<CryptographicKey> GenerateKeys(string? publicKeyComment = "")
        {
            int keyBits = 2048;

            var keygen = new SshKeyGenerator.SshKeyGenerator(keyBits);

            CryptographicKey key = new CryptographicKey
            {
                Base64PublicKey = keygen.ToRfcPublicKey(publicKeyComment),
                Base64PrivateKey = keygen.ToPrivateKey(),
            };

            return await ValueTask.FromResult(key);
        }
    }
}
