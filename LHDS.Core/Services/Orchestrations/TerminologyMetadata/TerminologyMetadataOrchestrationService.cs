// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Services.Processings.Ontologies;
using LHDS.Core.Services.Processings.TerminologyArtifacts;
using LHDS.Core.Services.Processings.TerminologyPolls;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationService : ITerminologyMetadataOrchestrationService
    {
        private readonly ITerminologyPollProcessingService terminologyPollProcessingService;
        private readonly ITerminologyArtifactProcessingService terminologyArtifactProcessingService;
        private readonly IOntologyProcessingService ontologyProcessingService;
        private readonly OntologyConfiguration ontologyConfiguration;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;


        public TerminologyMetadataOrchestrationService(
            ITerminologyPollProcessingService terminologyPollProcessingService,
            ITerminologyArtifactProcessingService terminologyArtifactProcessingService,
            IOntologyProcessingService ontologyProcessingService,
            OntologyConfiguration ontologyConfiguration,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.terminologyPollProcessingService = terminologyPollProcessingService;
            this.terminologyArtifactProcessingService = terminologyArtifactProcessingService;
            this.ontologyProcessingService = ontologyProcessingService;
            this.ontologyConfiguration = ontologyConfiguration;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask RetrieveArtifactMetadataAsync(string[] resourceTypes) =>
            TryCatch(async () =>
            {
                ValidateResourceTypes(resourceTypes);
                var exceptions = new List<Exception>();

                foreach (var resourceType in resourceTypes)
                {
                    try
                    {
                        await TryCatch(async () =>
                        {
                            ValidateResourceType(resourceType);

                            TerminologyPoll retrievedTerminologyPoll =
                                await this.terminologyPollProcessingService
                                    .RetrieveOrAddTerminologyPollAsync(resourceType);

                            string relativeUrl = this.ontologyConfiguration.TerminologyServerResourceRelativeUrl;
                            ValidateResourceURL(relativeUrl);
                            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);

                            relativeUrl = relativeUrl.Replace("{{datestamp}}", retrievedTerminologyPoll.LastPoll
                                .ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));

                            DateTimeOffset currentPollDateTimeOffset =
                                this.dateTimeBroker.GetCurrentDateTimeOffset();

                            await ProcessArtifacts(relativeUrl, resourceType);
                            retrievedTerminologyPoll.LastPoll = currentPollDateTimeOffset;

                            DateTimeOffset currentDateTimeOffset =
                                this.dateTimeBroker.GetCurrentDateTimeOffset();

                            retrievedTerminologyPoll.UpdatedDate = currentDateTimeOffset;

                            await this.terminologyPollProcessingService
                                .ModifyTerminologyPollAsync(retrievedTerminologyPoll);
                        });
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to retrieve metadata for {exceptions.Count} artifacts",
                        exceptions);
                }
            });

        private async ValueTask ProcessArtifacts(string relativeUrl, string resourceType)
        {

            OntologyAssets? retrievedOntologyAssets = null;

            switch (resourceType)
            {
                case "CodeSystem":
                    {
                        retrievedOntologyAssets =
                            await this.ontologyProcessingService.RetrieveAllCodingSystemsAsync(relativeUrl);
                        break;

                    }

                case "ValueSet":
                    {
                        retrievedOntologyAssets =
                            await this.ontologyProcessingService.RetrieveAllValueSetsAsync(relativeUrl);
                        break;
                    }

                case "ConceptMap":
                    {
                        retrievedOntologyAssets =
                            await this.ontologyProcessingService.RetrieveAllConceptMapsAsync(relativeUrl);
                        break;
                    }

                default:
                    throw new ArgumentException($"Unsupported resourceType: {resourceType}");
            }

            if (retrievedOntologyAssets == null)
            {
                return;
            }

            foreach (var asset in retrievedOntologyAssets.Assets)
            {
                DateTimeOffset newDateTimeOffset = this.dateTimeBroker.GetCurrentDateTimeOffset();

                TerminologyArtifact terminologyArtifact = new TerminologyArtifact
                {
                    Id = this.identifierBroker.GetIdentifier(),
                    FullUrl = asset.FullUrl,
                    ResourceType = asset.ResourceType,
                    Version = asset.Version,
                    Name = asset.Name,
                    Title = asset.Title,
                    Status = asset.Status,
                    LastUpdated = asset.LastUpdated,
                    IsCore = false,
                    IsDownloaded = false,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    UpdatedDate = newDateTimeOffset,
                    CreatedDate = newDateTimeOffset
                };

                await this.terminologyArtifactProcessingService
                    .ModifyOrAddTerminologyArtifactAsync(terminologyArtifact);
            }

            if (!string.IsNullOrWhiteSpace(retrievedOntologyAssets.NextPage))
            {
                await ProcessArtifacts(retrievedOntologyAssets.NextPage, resourceType);
            }
        }
    }
}