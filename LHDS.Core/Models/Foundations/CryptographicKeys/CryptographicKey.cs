// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Foundations.CryptographicKeys
{
    public class CryptographicKey
    {

        public required string PublicKey { get; set; }
        public required string PrivateKey { get; set; }
        public string? Passphrase { get; set; }
    }
}
