// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public class CryptographyKeyService : ICryptographyKeyService
    {
        private readonly IEnumerable<ICryptographyKeyBroker> cryptographyKeyBrokers;

        public CryptographyKeyService(IEnumerable<ICryptographyKeyBroker> cryptographyKeyBrokers)
        {
            this.cryptographyKeyBrokers = cryptographyKeyBrokers;
        }

        public ValueTask<CryptographicKey> GenerateKeysAsync(string CryptographyType, string? publicKeyComment = "") =>
            throw new NotImplementedException();
    }
}
