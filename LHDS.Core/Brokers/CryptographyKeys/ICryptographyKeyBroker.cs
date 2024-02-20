// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Keys;

namespace LHDS.Core.Brokers.Decryptions
{
    public interface ICryptographyKeyBroker
    {
        ValueTask<Key> GenerateKeys(string publicKeyComment);
        ValueTask<Key> GenerateKeys();
    }
}
