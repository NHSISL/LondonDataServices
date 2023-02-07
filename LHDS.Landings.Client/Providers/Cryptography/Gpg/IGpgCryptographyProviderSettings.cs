// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Landings.Client.Providers.Cryptography.Gpg
{
    public interface IGpgCryptographyProviderSettings
    {
        string PrivateKey { get; }
        string PublicKey { get; }
        string Passphrase { get; }
    }
}
