// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public interface ICryptographyKeyBroker
    {
        string CryptographyType { get; }

        ValueTask<CryptographicKey> GenerateKeysAsync(
            string comment,
            string passPhrase = "",
            string userName = "",
            string email = "");
    }
}
