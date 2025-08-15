// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Processings.CryptographicKeys;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestration : ISubscriberCredentialOrchestration
    {
        private readonly ISubscriberAgreementProcessingService subscriberAgreementProcessingService;
        private readonly ISecureDataProcessingService secureDataProcessingService;
        private readonly ICryptographyKeyProcessingService cryptographyKeyProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public SubscriberCredentialOrchestration(
            ISubscriberAgreementProcessingService subscriberAgreementProcessingService,
            ISecureDataProcessingService secureDataProcessingService,
            ICryptographyKeyProcessingService cryptographyKeyProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.subscriberAgreementProcessingService = subscriberAgreementProcessingService;
            this.secureDataProcessingService = secureDataProcessingService;
            this.cryptographyKeyProcessingService = cryptographyKeyProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        /// <summary>
        /// Method to update or add a subscriber credential
        /// </summary>
        /// <param name="subscriberCredential">The subscriber credentials</param>
        /// <param name="regenerateKeys">If TRUE, the service will regenerate the cryptography keys</param>
        /// <param name="externalUse">If TRUE, the cryptography private keys will be omitted from the result</param>
        /// <returns></returns>
        public ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential,
            bool regenerateKeys = false,
            bool externalUse = true) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredential(subscriberCredential);
                SubscriberAgreement subscriberAgreement = MapToSubsciberAgreement(subscriberCredential);

                SubscriberAgreement storageSubscriberAgreement =
                    await this.subscriberAgreementProcessingService.ModifyOrAddSubscriberAgreementAsync(
                        subscriberAgreement);

                ValidateSubscriberAgreementIsNotNull(storageSubscriberAgreement);

                if (!regenerateKeys)
                {
                    return MapToSubsciberCredentialForInternalExternalUse(
                        subscriberCredential,
                        storageSubscriberAgreement,
                        externalUse);
                }

                var updatedSubscriberCredentials = MapToSubsciberCredentialForInternalExternalUse(
                    subscriberCredential,
                    storageSubscriberAgreement,
                    externalUse: false);

                SubscriberCredential subcriberCredentialsWithSecureData = await this.cryptographyKeyProcessingService
                    .GenerateKeysAsync(updatedSubscriberCredentials);

                ValidateSubscriberCredentialIsNotNull(subcriberCredentialsWithSecureData);

                SubscriberAgreement subscriberAgreementWithKeys = MapToSubsciberAgreement(
                    subcriberCredentialsWithSecureData);

                subscriberAgreementWithKeys.UpdatedDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                await this.subscriberAgreementProcessingService.ModifyOrAddSubscriberAgreementAsync(
                    subscriberAgreementWithKeys);

                var updatedSubscriberCredentialsWithSecureData = await this.secureDataProcessingService
                    .AddOrModifySecureDataAsync(subcriberCredentialsWithSecureData);

                return MapToSubsciberCredentialForInternalExternalUse(
                    subscriberCredential: updatedSubscriberCredentialsWithSecureData,
                    externalUse);
            });

        public ValueTask<IQueryable<SubscriberCredential>> RetrieveAllSubscriberCredentialsAsync() =>
            TryCatch(async () =>
            {
                IQueryable<SubscriberAgreement> retrievedSubscriberAgreements =
                    await this.subscriberAgreementProcessingService.RetrieveAllSubscriberAgreementsAsync();

                List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

                foreach (SubscriberAgreement subscriberAgreement in retrievedSubscriberAgreements)
                {
                    SubscriberCredential subscriberCredential = new SubscriberCredential
                    {
                        Id = subscriberAgreement.Id,
                        SupplierSharingAgreementShortName = subscriberAgreement.SupplierSharingAgreementShortName,
                        SupplierSharingAgreementGuid = subscriberAgreement.SupplierSharingAgreementGuid,
                        FtpUserName = subscriberAgreement.FtpUserName,
                        FtpPassword = null,
                        FtpPublicKey = subscriberAgreement.FtpPublicKey,
                        FtpPassPhrase = null,
                        FtpPrivateKey = null,
                        GpgPublicKey = subscriberAgreement.GpgPublicKey,
                        GpgPassPhrase = null,
                        GpgPrivateKey = null,
                        IsActive = subscriberAgreement.IsActive,
                        LastPollEndDate = subscriberAgreement.LastPollEndDate,
                        LastPollStartDate = subscriberAgreement.LastPollStartDate,
                        CreatedBy = subscriberAgreement.CreatedBy,
                        UpdatedBy = subscriberAgreement.UpdatedBy,
                        UpdatedDate = subscriberAgreement.UpdatedDate,
                        CreatedDate = subscriberAgreement.CreatedDate,
                    };

                    subscriberCredentials.Add(subscriberCredential);
                }

                return subscriberCredentials.AsQueryable();
            });

        public ValueTask<List<Guid>> RetrieveAllActiveSubscriberCredentialIdsAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                IQueryable<SubscriberAgreement> retrievedSubscriberAgreements =
                    await this.subscriberAgreementProcessingService.RetrieveAllSubscriberAgreementsAsync();

                List<Guid> retrievedActiveIds = retrievedSubscriberAgreements
                    .Where(SubscriberAgreement => SubscriberAgreement.IsActive == true
                        && SubscriberAgreement.SupplierId == supplierId)

                    .Select(SubscriberAgreement => SubscriberAgreement.Id).ToList();

                return await ValueTask.FromResult(retrievedActiveIds);
            });

        /// <summary>
        /// Method to retrieve a subscriber credential by its id
        /// </summary>
        /// <param name="subscriberCredentialId">The subscriber credential id</param>
        /// <param name="externalUse">If TRUE, the cryptography private keys will be omitted from the result</param>
        /// <returns></returns>
        public ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId,
            bool externalUse = true) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredentialId(subscriberCredentialId);

                SubscriberAgreement retrievedSubscriberAgreement =
                    await this.subscriberAgreementProcessingService.RetrieveSubscriberAgreementByIdAsync(
                        subscriberCredentialId);

                ValidateSubscriberAgreementIsNotNull(retrievedSubscriberAgreement);

                SubscriberCredential mappedSubscriberCredential = MapToSubsciberCredentialForInternalExternalUse(
                    subscriberAgreement: retrievedSubscriberAgreement,
                    externalUse);

                if (externalUse)
                {
                    return mappedSubscriberCredential;
                }

                SubscriberCredential retrievedSubscriberCredential =
                    await this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(
                        mappedSubscriberCredential);

                return retrievedSubscriberCredential;
            });

        public ValueTask RemoveSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredentialId(subscriberCredentialId);
                await this.secureDataProcessingService.RemoveSecureDataByIdAsync(subscriberCredentialId);

                await this.subscriberAgreementProcessingService.RemoveSubscriberAgreementByIdAsync(
                    subscriberCredentialId);
            });

        private static SubscriberCredential MapToSubsciberCredentialForInternalExternalUse(
            SubscriberAgreement subscriberAgreement,
            bool externalUse)
        {
            return MapToSubsciberCredentialForInternalExternalUse(
                subscriberCredential: new SubscriberCredential(),
                subscriberAgreement,
                externalUse);
        }

        private static SubscriberCredential MapToSubsciberCredentialForInternalExternalUse(
            SubscriberCredential subscriberCredential,
            SubscriberAgreement subscriberAgreement,
            bool externalUse)
        {
            var mappedSubscriberCredential = new SubscriberCredential
            {
                Id = subscriberAgreement.Id,
                SupplierSharingAgreementShortName = subscriberAgreement.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberAgreement.SupplierSharingAgreementGuid,
                FtpUserName = subscriberAgreement.FtpUserName,
                FtpPassword = externalUse ? null : subscriberCredential.FtpPassword,
                FtpPublicKey = subscriberAgreement.FtpPublicKey,
                FtpPassPhrase = externalUse ? null : subscriberCredential.FtpPassPhrase,
                FtpPrivateKey = externalUse ? null : subscriberCredential.FtpPrivateKey,
                GpgPublicKey = subscriberAgreement.GpgPublicKey,
                GpgPassPhrase = externalUse ? null : subscriberCredential.GpgPassPhrase,
                GpgPrivateKey = externalUse ? null : subscriberCredential.GpgPrivateKey,
                IsActive = subscriberAgreement.IsActive,
                LastPollEndDate = subscriberAgreement.LastPollEndDate,
                LastPollStartDate = subscriberAgreement.LastPollStartDate,
                CreatedBy = subscriberAgreement.CreatedBy,
                UpdatedBy = subscriberAgreement.UpdatedBy,
                UpdatedDate = subscriberAgreement.UpdatedDate,
                CreatedDate = subscriberAgreement.CreatedDate,
            };

            return MapToSubsciberCredentialForInternalExternalUse(mappedSubscriberCredential, externalUse);
        }

        private static SubscriberCredential MapToSubsciberCredentialForInternalExternalUse(
            SubscriberCredential subscriberCredential,
            bool externalUse)
        {
            return new SubscriberCredential
            {
                Id = subscriberCredential.Id,
                SupplierSharingAgreementShortName = subscriberCredential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberCredential.SupplierSharingAgreementGuid,
                FtpUserName = subscriberCredential.FtpUserName,
                FtpPassword = externalUse ? null : subscriberCredential.FtpPassword,
                FtpPublicKey = subscriberCredential.FtpPublicKey,
                FtpPassPhrase = externalUse ? null : subscriberCredential.FtpPassPhrase,
                FtpPrivateKey = externalUse ? null : subscriberCredential.FtpPrivateKey,
                GpgPublicKey = subscriberCredential.GpgPublicKey,
                GpgPassPhrase = externalUse ? null : subscriberCredential.GpgPassPhrase,
                GpgPrivateKey = externalUse ? null : subscriberCredential.GpgPrivateKey,
                IsActive = subscriberCredential.IsActive,
                LastPollEndDate = subscriberCredential.LastPollEndDate,
                LastPollStartDate = subscriberCredential.LastPollStartDate,
                CreatedBy = subscriberCredential.CreatedBy,
                UpdatedBy = subscriberCredential.UpdatedBy,
                UpdatedDate = subscriberCredential.UpdatedDate,
                CreatedDate = subscriberCredential.CreatedDate,
            };
        }

        private static SubscriberAgreement MapToSubsciberAgreement(
            SubscriberCredential subscriberCredential)
        {
            return new SubscriberAgreement
            {
                Id = subscriberCredential.Id,
                SupplierSharingAgreementShortName = subscriberCredential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberCredential.SupplierSharingAgreementGuid,
                FtpUserName = subscriberCredential.FtpUserName,
                FtpPublicKey = subscriberCredential.FtpPublicKey,
                GpgPublicKey = subscriberCredential.GpgPublicKey,
                IsActive = subscriberCredential.IsActive,
                LastPollEndDate = subscriberCredential.LastPollEndDate,
                LastPollStartDate = subscriberCredential.LastPollStartDate,
                CreatedBy = subscriberCredential.CreatedBy,
                UpdatedBy = subscriberCredential.UpdatedBy,
                UpdatedDate = subscriberCredential.UpdatedDate,
                CreatedDate = subscriberCredential.CreatedDate,
            };
        }
    }
}
