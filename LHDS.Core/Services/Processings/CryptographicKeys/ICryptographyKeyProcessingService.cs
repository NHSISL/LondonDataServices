// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Processings.CryptographicKeys
{
    public interface ICryptographyKeyProcessingService
    {
        ValueTask<SubscriberCredential> GenerateKeysAsync(SubscriberCredential subscriberCredential);
    }
}
