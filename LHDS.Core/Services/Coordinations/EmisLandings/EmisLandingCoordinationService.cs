// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
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

        public async ValueTask<string> ProcessFileAsync(string fileName)
        {
            string[] parts = fileName.Split("/");

            if (parts.Length > 0)
            {
                string extractSubscriberCredentialIdString = parts[5];

                SubscriberCredential maybeSubscriberCredential = await this.subscriberCredentialOrchestration
                    .RetrieveSubscriberCredentialByIdAsync(new Guid(extractSubscriberCredentialIdString));

                string processedItem =
                    await this.emisLandingOrchestrationService.ProcessFileAsync(fileName, maybeSubscriberCredential);

                return processedItem;
            }
            else
            {
                throw new InvalidArgumentEmisLandingCoordinationException("Invalid file name format.");
            }
        }

        private static Guid GetLastRandomGuid(string file)
        {

            FileInfo fi = new FileInfo(file);
            string fileName = fi.Name;

            var elements = fileName.Split('_');
            if (elements.Length < 5)
            {
                //throw Exception here 
            }

            return Guid.Parse(elements[5].ToString());
        }
    }
}