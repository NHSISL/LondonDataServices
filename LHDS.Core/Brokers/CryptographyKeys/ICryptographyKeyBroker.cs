// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Brokers.CryptographyKeys
{
    public interface ICryptographyKeyBroker
    {
        ValueTask<CryptographicKey> GenerateKeys(string publicKeyComment);
        ValueTask<CryptographicKey> GenerateKeys();
    }
}
