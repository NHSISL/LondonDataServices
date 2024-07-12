// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Brokers.Cryptographies
{
    public interface ICryptographyBroker
    {
        ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential);
        ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential);
    }
}
