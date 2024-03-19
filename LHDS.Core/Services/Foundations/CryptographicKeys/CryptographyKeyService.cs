// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.CryptographicKeys;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyService : ICryptographyKeyService
    {
        private readonly IEnumerable<ICryptographyKeyBroker> cryptographyKeyBrokers;
        private readonly ILoggingBroker loggingBroker;

        public CryptographyKeyService(IEnumerable<ICryptographyKeyBroker> cryptographyKeyBrokers,
            ILoggingBroker loggingBroker)
        {
            this.cryptographyKeyBrokers = cryptographyKeyBrokers;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<CryptographicKey> GenerateKeysAsync(string cryptographyType, string? publicKeyComment = "") =>
            TryCatch(async () =>
            {
                ValidateInputArguments(cryptographyType);

                var broker = cryptographyKeyBrokers
                    .FirstOrDefault(broker => broker.CryptographyType == cryptographyType);

                ValidateBrokerNotNull(broker);

                return await broker.GenerateKeysAsync(comment: publicKeyComment);
            });
    }
}
