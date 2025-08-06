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
using Xeptions;

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

        public ValueTask ProcessDecryptedItemsForBatchCompleteAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateOnProcessDecryptedItemsForBatchComplete(supplierId);
                await this.ingressOrchestrationService.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);
            });

        public ValueTask<string> DecryptAsync(string encryptedFileName) =>
            TryCatch(async () =>
            {
                try
                {
                    (string decryptedFileName, Guid ingestionTrackingId) = await DecryptFileAsync(encryptedFileName);
                    await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);

                    return decryptedFileName;
                }
                catch (Exception exception)
                {
                    var rollBackException =
                        new RollbackDecryptionCoordinationException(
                            message: $"Failed to decrypt file. Rollback encrypted file: {encryptedFileName}",
                            innerException: exception as Xeption);

                    await this.loggingBroker.LogErrorAsync(rollBackException);
                    await this.ingressOrchestrationService.RollbackIngestionTrackingItemAsync(encryptedFileName);
                    throw;
                }
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
                        (string decryptedFileName, Guid ingestionTrackingId) = await DecryptFileAsync(encryptedFileName);
                        await this.ingressOrchestrationService.CheckForBatchCompleteAsync(ingestionTrackingId);
                    }
                    catch (Exception exception)
                    {
                        await this.loggingBroker.LogErrorAsync(exception);
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