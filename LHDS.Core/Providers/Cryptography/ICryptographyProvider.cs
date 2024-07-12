// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Providers.Cryptography
{
    public interface ICryptographyProvider
    {
        ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential);
        ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential);
    }
}
