// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Providers.Cryptography
{
    public interface ICryptographyProvider
    {
        ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential);
        ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential);
    }
}
