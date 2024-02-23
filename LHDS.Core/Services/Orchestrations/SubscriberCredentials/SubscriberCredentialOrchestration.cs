// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    internal class SubscriberCredentialOrchestration : ISubscriberCredentialOrchestration
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

        public async ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential)
        {
            return await this.secureDataProcessingService.AddOrModifySecureDataAsync(subscriberCredential);
        }

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
