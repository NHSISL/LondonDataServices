// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationTests
    {
        [Theory]
        [InlineData("CodeSystem")]
        [InlineData("ValueSet")]
        [InlineData("ConceptMap")]
        public async Task ShouldRetrievePagedArtifactMetadataAsync(string resourceType)
        {
            // given
            string[] resourceTypes = new string[] { resourceType };
            Guid randomId = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomString = GetRandomString();

            TerminologyPoll retrievedTerminologyPoll =
                CreateRandomTerminologyPoll(resourceType, lastPoll: randomDateTimeOffset.AddDays(-3));

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType)).
                    ReturnsAsync(retrievedTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            string relativeUrl = this.ontologyConfiguration.TerminologyServerResourceRelativeUrl;
            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);

            relativeUrl = relativeUrl.Replace("{{datestamp}}", retrievedTerminologyPoll.LastPoll
                .ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));

            List<dynamic> randomArtifactPropertiesPageOne =
                CreateRandomArtifactProperties(resourceType, randomDateTimeOffset, randomId);

            List<dynamic> randomArtifactPropertiesPageTwo =
                CreateRandomArtifactProperties(resourceType, randomDateTimeOffset, randomId);

            List<dynamic> allRandomArifactProperties = new List<dynamic>();
            allRandomArifactProperties.AddRange(randomArtifactPropertiesPageOne);
            allRandomArifactProperties.AddRange(randomArtifactPropertiesPageTwo);

            List<TerminologyArtifact> terminologyArtifacts =
                CreateTerminologyArtiFactFromRandomData(allRandomArifactProperties);

            OntologyAssets pageOneOntologyAssets =
                CreateOntologyAssetFromRandomData(randomArtifactPropertiesPageOne);

            OntologyAssets pageTwoRetrievedOntologyAssets =
                CreateOntologyAssetFromRandomData(randomArtifactPropertiesPageTwo);

            pageTwoRetrievedOntologyAssets.NextPage = string.Empty;

            switch (resourceType)
            {
                case "CodeSystem":
                    {
                        this.ontologyProcessingServiceMock.Setup(service =>
                            service.RetrieveAllCodingSystemsAsync(relativeUrl))
                                .ReturnsAsync(pageOneOntologyAssets);

                        this.ontologyProcessingServiceMock.Setup(service =>
                            service.RetrieveAllCodingSystemsAsync(pageOneOntologyAssets.NextPage))
                                .ReturnsAsync(pageTwoRetrievedOntologyAssets);

                        break;

                    }

                case "ValueSet":
                    {
                        this.ontologyProcessingServiceMock.Setup(service =>
                            service.RetrieveAllValueSetsAsync(relativeUrl))
                                .ReturnsAsync(pageOneOntologyAssets);

                        this.ontologyProcessingServiceMock.Setup(service =>
                            service.RetrieveAllValueSetsAsync(pageOneOntologyAssets.NextPage))
                                .ReturnsAsync(pageTwoRetrievedOntologyAssets);
                        break;
                    }

                case "ConceptMap":
                    {
                        this.ontologyProcessingServiceMock.Setup(service =>
                            service.RetrieveAllConceptMapsAsync(relativeUrl))
                                .ReturnsAsync(pageOneOntologyAssets);

                        this.ontologyProcessingServiceMock.Setup(service =>
                            service.RetrieveAllConceptMapsAsync(pageOneOntologyAssets.NextPage))
                                .ReturnsAsync(pageTwoRetrievedOntologyAssets);

                        break;
                    }

                default:
                    throw new ArgumentException($"Unsupported resourceType: {resourceType}");
            }

            List<OntologyAsset> assets = new List<OntologyAsset>();
            assets.AddRange(pageOneOntologyAssets.Assets);
            assets.AddRange(pageTwoRetrievedOntologyAssets.Assets);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomId);

            TerminologyPoll updatedTerminologyPoll = retrievedTerminologyPoll.DeepClone();
            updatedTerminologyPoll.LastPoll = randomDateTimeOffset;
            updatedTerminologyPoll.UpdatedDate = randomDateTimeOffset;
            TerminologyPoll storageTerminologyPoll = updatedTerminologyPoll.DeepClone();

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.Is(SameTerminologyPollAs(updatedTerminologyPoll)))).
                    ReturnsAsync(storageTerminologyPoll);

            // when
            await this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceTypes);

            // then
            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType),
                    Times.Once);


            switch (resourceType)
            {
                case "CodeSystem":
                    {
                        this.ontologyProcessingServiceMock.Verify(service =>
                            service.RetrieveAllCodingSystemsAsync(relativeUrl),
                                Times.Once);

                        this.ontologyProcessingServiceMock.Verify(service =>
                            service.RetrieveAllCodingSystemsAsync(pageOneOntologyAssets.NextPage),
                                Times.Once);

                        break;

                    }

                case "ValueSet":
                    {
                        this.ontologyProcessingServiceMock.Verify(service =>
                            service.RetrieveAllValueSetsAsync(relativeUrl),
                                Times.Once);

                        this.ontologyProcessingServiceMock.Verify(service =>
                            service.RetrieveAllValueSetsAsync(pageOneOntologyAssets.NextPage),
                                Times.Once);

                        break;
                    }

                case "ConceptMap":
                    {
                        this.ontologyProcessingServiceMock.Verify(service =>
                            service.RetrieveAllConceptMapsAsync(relativeUrl),
                                Times.Once);

                        this.ontologyProcessingServiceMock.Verify(service =>
                            service.RetrieveAllConceptMapsAsync(pageOneOntologyAssets.NextPage),
                                Times.Once);

                        break;
                    }

                default:
                    throw new ArgumentException($"Unsupported resourceType: {resourceType}");
            }

            for (int i = 0; i < assets.Count; i++)
            {
                TerminologyArtifact terminologyArtifact = terminologyArtifacts[i];

                this.terminologyArtifactProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddTerminologyArtifactAsync(It.Is(SameTerminologyArtifactAs(terminologyArtifact))),
                        Times.Once);
            }

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(assets.Count + 2));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(assets.Count));

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.Is(SameTerminologyPollAs(updatedTerminologyPoll))),
                    Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}