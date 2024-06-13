// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.CryptographicKeys;
using LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions;

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

        public ValueTask<CryptographicKey> GenerateKeysAsync(
            string cryptographyType,
            string? comment = "",
            string? passPhrase = "",
            string? userName = "",
            string? email = "") =>
            TryCatch(async () =>
            {
                ValidateInputArguments(cryptographyType);

                var broker = cryptographyKeyBrokers
                    .FirstOrDefault(broker => broker.CryptographyType == cryptographyType);

                if (broker is null)
                {
                    throw new NullBrokerCryptographyKeyException(message: "Broker is null.");
                }

                return await broker.GenerateKeysAsync(
                    comment ?? string.Empty,
                    passPhrase ?? string.Empty,
                    userName ?? string.Empty,
                    email ?? string.Empty);
            });
    }
}
