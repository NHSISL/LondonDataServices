// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Cryptographies;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.SubscriberCredentials;

namespace LHDS.Core.Services.Foundations.Cryptographies
{
    public partial class CryptographyService : ICryptographyService
    {

        private readonly ICryptographyBroker cryptographyBroker;
        private readonly ILoggingBroker loggingBroker;

        public CryptographyService(
            ICryptographyBroker cryptographyBroker,
            ILoggingBroker loggingBroker)
        {
            this.cryptographyBroker = cryptographyBroker;
            this.loggingBroker = loggingBroker;
        }

        public Task<byte[]> EncryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateInputs(data, subscriberCredential);

                return await this.cryptographyBroker.EncryptAsync(data, subscriberCredential);
            });

        public Task<byte[]> DecryptAsync(byte[] data, SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateInputs(data, subscriberCredential);

                return await this.cryptographyBroker.DecryptAsync(data, subscriberCredential);
            });
    }
}