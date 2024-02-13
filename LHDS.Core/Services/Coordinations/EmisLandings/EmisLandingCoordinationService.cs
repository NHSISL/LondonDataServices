// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
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

        public ValueTask<List<string>> ProcessAsync() =>
            throw new NotImplementedException();

        public async ValueTask<string> ProcessAsync(string fileName) =>
            throw new NotImplementedException();
    }
}