// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using static LHDS.Core.Models.Foundations.CryptographicKeys.CryptographicKey;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public interface ICryptographyKeyBroker
    {
        string CryptographyType { get; }
        KeysModel GenerateKeys(string comment, string? password, string userName = "", string email = "");
    }
}
