// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.Landings.Client.Brokers.Decryptions
{
    public interface IDecryptionBroker
    {
        ValueTask<byte[]> DecryptAsync(byte[] data);
    }
}
