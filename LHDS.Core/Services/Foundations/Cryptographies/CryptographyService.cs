// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
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

        public ValueTask EncryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateInputs(input, output, subscriberCredential);

                await this.cryptographyBroker.EncryptAsync(input, output, subscriberCredential);
            });

        public ValueTask DecryptAsync(Stream input, Stream output, SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateInputs(input, output, subscriberCredential);

                await this.cryptographyBroker.DecryptAsync(input, output, subscriberCredential);
            });
    }
}