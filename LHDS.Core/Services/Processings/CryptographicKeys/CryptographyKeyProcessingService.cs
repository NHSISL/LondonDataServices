// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public class CryptographyKeyProcessingService : ICryptographyKeyProcessingService
    {
        private readonly ICryptographyKeyService cryptographyKeyService;

        public CryptographyKeyProcessingService(ICryptographyKeyService cryptographyKeyService)
        {
            this.cryptographyKeyService = cryptographyKeyService;
        }

        public ValueTask<SubscriberCredential> GenerateKeysAsync(SubscriberCredential subscriberCredential) =>
            throw new NotImplementedException();
    }
}
