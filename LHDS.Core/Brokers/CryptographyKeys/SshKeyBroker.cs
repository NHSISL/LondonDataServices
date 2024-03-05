// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public class SshKeyBroker : ICryptographyKeyBroker
    {
        public string CryptographyType => "SSH";

        public CryptographicKey GenerateKeys(string comment, string? password, string name, string email)
        {
            int keyBits = 2048;

            var keygen = new SshKeyGenerator.SshKeyGenerator(keyBits);

            return new CryptographicKey
            {
                PublicKey = keygen.ToRfcPublicKey(comment),
                PrivateKey = keygen.ToPrivateKey(),
            };
        }
    }
}
