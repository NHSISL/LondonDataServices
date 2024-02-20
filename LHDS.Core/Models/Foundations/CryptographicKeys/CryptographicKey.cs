// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Foundations.CryptographicKeys
{
    public class CryptographicKey
    {
        public string? Base64PublicKey { get; set; }
        public string? Base64PrivateKey { get; set; }
        public string? Passphrase { get; set; }
    }
}
