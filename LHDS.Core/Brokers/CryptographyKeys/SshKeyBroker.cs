// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public class SshKeyBroker : ICryptographyKeyBroker
    {
        public string CryptographyType => "SSH";

        public async ValueTask<CryptographicKey> GenerateKeys(string comment, string? password, string name, string email)
        {
            int keyBits = 2048;

            var keygen = new SshKeyGenerator.SshKeyGenerator(keyBits);

            CryptographicKey returnedKey = new CryptographicKey
            {
                PublicKey = keygen.ToRfcPublicKey(comment),
                PrivateKey = keygen.ToPrivateKey(),
            };

            return await ValueTask.FromResult(returnedKey);
        }
    }
}
