// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        public async ValueTask<SubscriberCredential> GenerateKeysAsync(SubscriberCredential subscriberCredential)
        {
            var gpgGeneratedKeys = await cryptographyKeyService.GenerateKeysAsync("GPG");
            var ftpGeneratedKeys = await cryptographyKeyService.GenerateKeysAsync("SSH");

            subscriberCredential.GpgPublicKey = gpgGeneratedKeys.Base64PublicKey;
            subscriberCredential.GpgPrivateKey = gpgGeneratedKeys.Base64PrivateKey;
            subscriberCredential.GpgPassPhrase = gpgGeneratedKeys.Passphrase;
            subscriberCredential.FtpPublicKey = ftpGeneratedKeys.Base64PublicKey;
            subscriberCredential.FtpPrivateKey = ftpGeneratedKeys.Base64PrivateKey;
            subscriberCredential.FtpPassPhrase = ftpGeneratedKeys.Passphrase;

            return subscriberCredential;
        }
    }
}
