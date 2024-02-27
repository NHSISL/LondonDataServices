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
                    LastPollStartDate = subscriberCredential.LastPollStartDate
                };

                await this.subscriberAgreementProcessingService.ModifyOrAddSubscriberAgreementAsync(
                    subscriberAgreement);

                return await this.secureDataProcessingService.AddOrModifySecureDataAsync(subscriberCredential);
            });

        public IQueryable<SubscriberCredential> RetrieveAllSubscriberCredentials() =>
            throw new NotImplementedException();

        public ValueTask<List<Guid>> RetrieveAllActiveSubscriberCredentialIds() =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential>
            RetrieveSubscriberCredentialBySupplierSharingAgreementGuidAsync(Guid SupplierSharingAgreementGuid) =>
                throw new NotImplementedException();
    }
}
