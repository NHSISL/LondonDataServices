// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Brokers.Cryptographies
{
    public interface ICryptographyBroker
    {
        ValueTask<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential);
        ValueTask<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential);
    }
}
