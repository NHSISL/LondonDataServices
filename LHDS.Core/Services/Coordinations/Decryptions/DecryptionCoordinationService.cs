// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionCoordinationService : IDecryptionCoordinationService
    {
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;
        private readonly ILoggingBroker loggingBroker;

        public DecryptionCoordinationService(
            IDecryptionOrchestrationService decryptionOrchestrationService,
            ISubscriberCredentialOrchestration subscriberCredentialOrchestration,
            ILoggingBroker loggingBroker)
        {
            this.decryptionOrchestrationService = decryptionOrchestrationService;
            this.subscriberCredentialOrchestration = subscriberCredentialOrchestration;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<string> DecryptAsync(string fileName) =>
            throw new System.NotImplementedException();
    }
}