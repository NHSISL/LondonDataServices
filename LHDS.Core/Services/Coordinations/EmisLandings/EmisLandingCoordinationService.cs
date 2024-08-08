// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.SubscriberCredentials;

namespace LHDS.Core.Services.Coordinations.EmisLandings
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

        public ValueTask<List<string>> ProcessAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateProcessArgs(supplierId);

                List<Guid> subscriberAgreementIds = await this.subscriberCredentialOrchestration
                    .RetrieveAllActiveSubscriberCredentialIds();

                List<string> processedPaths = new List<string>();
                var exceptions = new List<Exception>();

                foreach (Guid subscriberAgreementId in subscriberAgreementIds)
                {
                    try
                    {
                        List<string> processedFiles = await TryCatch(async () =>
                        {
                            SubscriberCredential maybeSubscriberCredential =
                                await this.subscriberCredentialOrchestration
                                    .RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, false);

                            List<string> processedItems = await this.emisLandingOrchestrationService
                                .ProcessAsync(maybeSubscriberCredential, supplierId);

                            return processedItems;
                        });

                        processedPaths.AddRange(processedFiles);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        Console.WriteLine(ex.ToString());
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to process files for {exceptions.Count} subscriber agreements",
                        exceptions);
                }

                return processedPaths;
            });

        public ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(Guid subscriberAgreementId) =>
            TryCatch(async () =>
            {
                ValidateArgsOnRetrieveListOfDocumentsToProcess(subscriberAgreementId);

                SubscriberCredential maybeSubscriberCredential = await this.subscriberCredentialOrchestration
                    .RetrieveSubscriberCredentialByIdAsync(
                        subscriberCredentialId: subscriberAgreementId,
                        externalUse: false);

                List<string> listOfDocumentsToProcess = await this.emisLandingOrchestrationService
                    .RetrieveListOfDocumentsToProcessAsync(maybeSubscriberCredential);

                return listOfDocumentsToProcess;
            });

        public ValueTask RetrieveDownloadByFileNameAsync(Stream output, string fileName) =>
            TryCatch(async () =>
            {
                ValidateFileNameOnRetrieve(output, fileName);
                Guid subscriberCredentialId = Guid.Parse(fileName.Split("/")[5]);

                SubscriberCredential subscriberCredential =
                    await this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(
                        subscriberCredentialId, false);

                await this.emisLandingOrchestrationService.RetrieveDownloadByFileNameAsync(
                    output,
                    fileName,
                    subscriberCredential);
            });

        public ValueTask RedecryptDocumentByIngestionIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateArgsOnRedecryptDocumentByIngestionId(ingestionTrackingId);
                await this.emisLandingOrchestrationService.RedecryptDocumentByIngestionIdAsync(ingestionTrackingId);
            });
    }
}