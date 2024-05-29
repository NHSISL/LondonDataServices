// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
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
            subscriberCredential.GpgPublicKey = ConvertToBase64(gpgGeneratedKeys.PublicKey);
            subscriberCredential.GpgPrivateKey = ConvertToBase64(gpgGeneratedKeys.PrivateKey);
            subscriberCredential.GpgPassPhrase = gpgGeneratedKeys.Passphrase;
            subscriberCredential.FtpPublicKey = ConvertToBase64(ftpGeneratedKeys.PublicKey);
            subscriberCredential.FtpPrivateKey = ConvertToBase64(ftpGeneratedKeys.PrivateKey);
            subscriberCredential.FtpPassPhrase = ftpGeneratedKeys.Passphrase;

            return subscriberCredential;
        });

        private string ConvertToBase64(string value)
        {
            byte[] byteValue = Encoding.UTF8.GetBytes(value);

            return Convert.ToBase64String(byteValue);
        }
    }
}
