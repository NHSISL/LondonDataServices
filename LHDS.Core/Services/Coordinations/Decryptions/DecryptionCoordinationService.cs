// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Decryptions;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;

namespace LHDS.Core.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationService : IDecryptionCoordinationService
    {
        private readonly IDecryptionOrchestrationService decryptionOrchestrationService;
        private readonly ISubscriberCredentialOrchestration subscriberCredentialOrchestration;
        private readonly IIngressOrchestrationService ingressOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public DecryptionCoordinationService(
            IDecryptionOrchestrationService decryptionOrchestrationService,
            ISubscriberCredentialOrchestration subscriberCredentialOrchestration,
            IIngressOrchestrationService ingressOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.decryptionOrchestrationService = decryptionOrchestrationService;
            this.subscriberCredentialOrchestration = subscriberCredentialOrchestration;
            this.ingressOrchestrationService = ingressOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<string> DecryptAsync(string encryptedFileName) =>
            TryCatch(async () =>
            {
                (string decryptedFileName, Guid ingestionTrackingId) = await DecryptFileAsync(encryptedFileName);
                await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

                return decryptedFileName;
            });

        public ValueTask RetryDecryptOnAllAsync() =>
            TryCatch(async () =>
            {
                string? encryptedFileName;

                while (!string.IsNullOrEmpty(encryptedFileName =
                    await this.decryptionOrchestrationService.GetNextItemToBeDecrypted()))
                {
                    try
                    {
                        await DecryptFileAsync(encryptedFileName);
                    }
                    catch (Exception exception)
                    {
                        this.loggingBroker.LogError(exception);
                    }
                }
            });

        private async ValueTask<(string DecryptedFileName, Guid IngestionTrackingId)> DecryptFileAsync(string encryptedFileName)
        {
            ValidateFileNameOnDecrypt(encryptedFileName);
            string[] parts = encryptedFileName.Split("/");

            if (parts.Length > 0)
            {
                string extractSubscriberCredentialIdString = parts[2];
                Guid subscriberCredentialId;

                if (!Guid.TryParse(extractSubscriberCredentialIdString, out subscriberCredentialId))
                {
                    throw new InvalidArgumentDecryptionCoordinationException(
                        $"Failed to parse {extractSubscriberCredentialIdString} to Guid. " +
                        $"Encrypted File Name is {encryptedFileName}.");
                }

                SubscriberCredential maybeSubscriberCredential = await this.subscriberCredentialOrchestration
                    .RetrieveSubscriberCredentialByIdAsync(
                        subscriberCredentialId: new Guid(extractSubscriberCredentialIdString),
                        externalUse: false);

                return await this.decryptionOrchestrationService.DecryptAsync(
                        encryptedFileName,
                        maybeSubscriberCredential);
            }
            else
            {
                throw new InvalidArgumentDecryptionCoordinationException("Invalid file name format.");
            }
        }
    }
}