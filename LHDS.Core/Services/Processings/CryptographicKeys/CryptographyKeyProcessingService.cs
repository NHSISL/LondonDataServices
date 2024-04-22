// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Processings.CryptographicKeys;

namespace LHDS.Core.Services.Foundations.CryptographicKeys
{
    public partial class CryptographyKeyProcessingService : ICryptographyKeyProcessingService
    {
        private readonly ICryptographyKeyService cryptographyKeyService;
        private readonly ILoggingBroker loggingBroker;

        public CryptographyKeyProcessingService(
            ICryptographyKeyService cryptographyKeyService,
            ILoggingBroker loggingBroker)
        {
            this.cryptographyKeyService = cryptographyKeyService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SubscriberCredential> GenerateKeysAsync(SubscriberCredential subscriberCredential) =>
        TryCatch(async () =>
        {
            ValidateSubscriberCredential(subscriberCredential);
            var gpgGeneratedKeys = await cryptographyKeyService.GenerateKeysAsync("GPG");
            var ftpGeneratedKeys = await cryptographyKeyService.GenerateKeysAsync("SSH");
            subscriberCredential.GpgPublicKey = gpgGeneratedKeys.PublicKey;
            subscriberCredential.GpgPrivateKey = gpgGeneratedKeys.PrivateKey;
            subscriberCredential.GpgPassPhrase = gpgGeneratedKeys.Passphrase;
            subscriberCredential.FtpPublicKey = ftpGeneratedKeys.PublicKey;
            subscriberCredential.FtpPrivateKey = ftpGeneratedKeys.PrivateKey;
            subscriberCredential.FtpPassPhrase = ftpGeneratedKeys.Passphrase;

            return subscriberCredential;
        });
    }
}
