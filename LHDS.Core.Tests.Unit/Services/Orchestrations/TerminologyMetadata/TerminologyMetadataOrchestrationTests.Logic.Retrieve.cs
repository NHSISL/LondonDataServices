// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
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
        [Fact]
        public async Task ShouldRetrievePagedArtifactMetadataAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomString = GetRandomString();
            string resourceType = randomString;

            TerminologyPoll retrievedTerminologyPoll =
                CreateRandomTerminologyPoll(resourceType, lastPoll: randomDateTimeOffset.AddDays(-3));

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType)).
                    ReturnsAsync(retrievedTerminologyPoll);
          
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;
            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
            relativeUrl = relativeUrl.Replace("{{datestamp}}", retrievedTerminologyPoll.LastPoll.ToString());

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

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveAllCodingSystemsAsync(relativeUrl))
                    .ReturnsAsync(pageOneOntologyAssets);

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveAllCodingSystemsAsync(pageOneOntologyAssets.NextPage))
                    .ReturnsAsync(pageTwoRetrievedOntologyAssets);

            List<OntologyAsset> assets = new List<OntologyAsset>();
            assets.AddRange(pageOneOntologyAssets.Assets);
            assets.AddRange(pageTwoRetrievedOntologyAssets.Assets);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            TerminologyPoll updatedTerminologyPoll = retrievedTerminologyPoll.DeepClone();
            updatedTerminologyPoll.LastPoll = randomDateTimeOffset;
            TerminologyPoll storageTerminologyPoll = updatedTerminologyPoll.DeepClone();

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(It.Is(SameTerminologyPollAs(updatedTerminologyPoll)))).
                    ReturnsAsync(storageTerminologyPoll);

            // when
            await this.terminologyMetadataOrchestrationService.RetrieveArtifactMetadataAsync(resourceType);

            // then
            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveOrAddTerminologyPollAsync(resourceType),
                    Times.Once);

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveAllCodingSystemsAsync(relativeUrl),
                    Times.Once);

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveAllCodingSystemsAsync(pageOneOntologyAssets.NextPage),
                    Times.Once);

            for (int i = 0; i < assets.Count; i++)
            {
                TerminologyArtifact terminologyArtifact = terminologyArtifacts[i];

                this.terminologyArtifactProcessingServiceMock.Verify(service =>
                    service.ModifyOrAddTerminologyArtifactAsync(It.Is(SameTerminologyArtifactAs(terminologyArtifact))),
                        Times.Once());
            }

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(assets.Count + 1));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
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