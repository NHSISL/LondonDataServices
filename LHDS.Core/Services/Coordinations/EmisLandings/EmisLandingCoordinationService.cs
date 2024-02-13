// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class EmisLandingCoordinationService : IEmisLandingCoordinationService
    {
        private readonly IEmisLandingOrchestrationService emisLandingOrchestrationService;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;
        private readonly ILoggingBroker loggingBroker;

        public EmisLandingCoordinationService(
            IEmisLandingOrchestrationService emisLandingOrchestrationService,
            ISubscriberCredentialOrchestration subscriberCredentialOrchestration,
            ILoggingBroker loggingBroker)
        {
            this.emisLandingOrchestrationService = emisLandingOrchestrationService;
            this.subscriberCredentialOrchestration = subscriberCredentialOrchestration;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<List<string>> ProcessAsync()
        {
            List<Guid> subscriberAgreementIds = await this.subscriberCredentialOrchestration
                .RetrieveAllActiveSubscriberCredentialIds();

            List<string> processedPaths = new List<string>();

            foreach (Guid subscriberAgreementId in subscriberAgreementIds)
            {
                SubscriberCredential subscriberCredential = await this.subscriberCredentialOrchestration
                    .RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId);

                List<string> processedItems = await this.emisLandingOrchestrationService
                    .ProcessAsync(subscriberCredential);

                processedPaths.AddRange(processedItems);
            }

            return processedPaths;
        }

        public async ValueTask<string> ProcessAsync(string fileName) =>
            throw new NotImplementedException();
    }
}