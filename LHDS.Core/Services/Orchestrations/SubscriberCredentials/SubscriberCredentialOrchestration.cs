// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Processings.SecureDatas;
using LHDS.Core.Services.Processings.SubscriberAgreements;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    internal class SubscriberCredentialOrchestration : ISubscriberCredentialOrchestration
    {
        private readonly ISubscriberAgreementProcessingService subscriberAgreementProcessingService;
        private readonly ISecureDataProcessingService secureDataProcessingService;

        public SubscriberCredentialOrchestration(
            ISubscriberAgreementProcessingService subscriberAgreementProcessingService,
            ISecureDataProcessingService secureDataProcessingService)
        {
            this.subscriberAgreementProcessingService = subscriberAgreementProcessingService;
            this.secureDataProcessingService = secureDataProcessingService;
        }

        public ValueTask<SubscriberCredential> ModifyOrAddSubscriberCredentialAsync(
            SubscriberCredential subscriberCredential) =>
                throw new NotImplementedException();

        public IQueryable<SubscriberCredential> RetrieveAllSubscriberCredentials() =>
            throw new NotImplementedException();

        public ValueTask<List<Guid>> RetrieveAllActiveSubscriberCredentialIds() =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RetrieveSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            throw new NotImplementedException();
    }
}
