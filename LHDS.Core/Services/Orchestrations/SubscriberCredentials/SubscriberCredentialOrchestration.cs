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
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestration : ISubscriberCredentialOrchestration
    {
        private readonly ISubscriberAgreementProcessingService subscriberAgreementProcessingService;
        private readonly ISecureDataProcessingService secureDataProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public SubscriberCredentialOrchestration(
            ISubscriberAgreementProcessingService subscriberAgreementProcessingService,
            ISecureDataProcessingService secureDataProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.subscriberAgreementProcessingService = subscriberAgreementProcessingService;
            this.secureDataProcessingService = secureDataProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredential(subscriberCredential);

                SubscriberAgreement subscriberAgreement = new SubscriberAgreement
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

                SubscriberAgreement storageSubscriberAgreement =
                    await this.subscriberAgreementProcessingService.ModifyOrAddSubscriberAgreementAsync(
                        subscriberAgreement);

                ValidateSubscriberAgreementIsNotNull(storageSubscriberAgreement);

                SubscriberCredential updatedSubcriberCredential = new SubscriberCredential
                {
                    Id = storageSubscriberAgreement.Id,
                    SupplierSharingAgreementShortName = storageSubscriberAgreement.SupplierSharingAgreementShortName,
                    SupplierSharingAgreementGuid = storageSubscriberAgreement.SupplierSharingAgreementGuid,
                    FtpUserName = storageSubscriberAgreement.FtpUserName,
                    FtpPassword = subscriberCredential.FtpPassword,
                    FtpPublicKey = storageSubscriberAgreement.FtpPublicKey,
                    FtpPassPhrase = subscriberCredential.FtpPassPhrase,
                    FtpPrivateKey = subscriberCredential.FtpPrivateKey,
                    GpgPublicKey = storageSubscriberAgreement.GpgPublicKey,
                    GpgPassPhrase = subscriberCredential.GpgPassPhrase,
                    GpgPrivateKey = subscriberCredential.GpgPrivateKey,
                    IsActive = storageSubscriberAgreement.IsActive,
                    LastPollEndDate = storageSubscriberAgreement.LastPollEndDate,
                    LastPollStartDate = storageSubscriberAgreement.LastPollStartDate,
                    CreatedBy = storageSubscriberAgreement.CreatedBy,
                    UpdatedBy = storageSubscriberAgreement.UpdatedBy,
                    UpdatedDate = storageSubscriberAgreement.UpdatedDate,
                    CreatedDate = storageSubscriberAgreement.CreatedDate,
                };

                return await this.secureDataProcessingService.AddOrModifySecureDataAsync(updatedSubcriberCredential);
            });

        public IQueryable<SubscriberCredential> RetrieveAllSubscriberCredentials() =>
            TryCatch(() =>
            {
                IQueryable<SubscriberAgreement> retrievedSubscriberAgreements = 
                    this.subscriberAgreementProcessingService.RetrieveAllSubscriberAgreements();

                List<SubscriberCredential> subscriberCredentials = new List<SubscriberCredential>();

                foreach (SubscriberAgreement subscriberAgreement in retrievedSubscriberAgreements)
                {
                    SubscriberCredential subscriberCredential = new SubscriberCredential
                    {
                        Id = subscriberAgreement.Id,
                        SupplierSharingAgreementShortName = subscriberAgreement.SupplierSharingAgreementShortName,
                        SupplierSharingAgreementGuid = subscriberAgreement.SupplierSharingAgreementGuid,
                        FtpUserName = subscriberAgreement.FtpUserName,
                        FtpPassword = string.Empty,
                        FtpPublicKey = subscriberAgreement.FtpPublicKey,
                        FtpPassPhrase = string.Empty,
                        FtpPrivateKey = string.Empty,
                        GpgPublicKey = subscriberAgreement.GpgPublicKey,
                        GpgPassPhrase = string.Empty,
                        GpgPrivateKey = string.Empty,
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

        public ValueTask<List<Guid>> RetrieveAllActiveSubscriberCredentialIds() =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(
            Guid subscriberCredentialId, 
            bool externalUse = true) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            throw new NotImplementedException();
    }
}
