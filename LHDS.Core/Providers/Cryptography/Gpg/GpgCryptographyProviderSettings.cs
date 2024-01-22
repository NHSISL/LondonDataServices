// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Providers.Cryptography.Gpg
{
    public class GpgCryptographyProviderSettings : IGpgCryptographyProviderSettings
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Passphrase { get; set; }
    }
}
