// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
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

        public ValueTask<List<string>> ProcessAsync() =>
            TryCatch(async () =>
            {
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
                            SubscriberCredential maybeSubscriberCredential = await this.subscriberCredentialOrchestration
                                .RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId);

                            List<string> processedItems = await this.emisLandingOrchestrationService
                                .ProcessAsync(maybeSubscriberCredential);

                            return processedItems;
                        });

                        processedPaths.AddRange(processedFiles);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
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

        public ValueTask<string> ProcessFileAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateFileNameOnLand(fileName);
                string[] parts = fileName.Split("/");

                if (parts.Length > 0)
                {
                    string extractSubscriberCredentialId = parts[5];

                    SubscriberCredential maybeSubscriberCredential = await this.subscriberCredentialOrchestration
                        .RetrieveSubscriberCredentialByIdAsync(new Guid(extractSubscriberCredentialId));

                    string processedItem =
                        await this.emisLandingOrchestrationService.ProcessFileAsync(
                            fileName: fileName,
                            subscriberCredential: maybeSubscriberCredential);

                    return processedItem;
                }
                else
                {
                    throw new InvalidArgumentEmisLandingCoordinationException("Invalid file name format.");
                }
            });
    }
}