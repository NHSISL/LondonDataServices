// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Decryptions;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;

namespace LHDS.Core.Services.Coordinations.Decryptions
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
            TryCatch(async () =>
            {
                ValidateFileNameOnDecrypt(fileName);
                string[] parts = fileName.Split("/");

                if (parts.Length > 0)
                {
                    string extractSubscriberCredentialIdString = parts[0];

                    SubscriberCredential maybeSubscriberCredential = await this.subscriberCredentialOrchestration
                        .RetrieveSubscriberCredentialByIdAsync(new Guid(extractSubscriberCredentialIdString));

                    string decryptItem =
                        await this.decryptionOrchestrationService.DecryptAsync(fileName);

                    return decryptItem;
                }
                else
                {
                    throw new InvalidArgumentDecryptionCoordinationException("Invalid file name format.");
                }
            });
    }
}