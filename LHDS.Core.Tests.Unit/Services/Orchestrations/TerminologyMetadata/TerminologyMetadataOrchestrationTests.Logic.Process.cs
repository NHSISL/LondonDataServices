// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task ShouldProcessRetrieveArtifactMetadataAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomString = GetRandomString();
            string resourceType = randomString;

            IQueryable<TerminologyPoll> terminologyPolls = 
                CreateRandomTerminologyPolls(resourceType, lastPoll: randomDateTimeOffset);

            TerminologyPoll retrievedTerminologyPoll = terminologyPolls.First();
            string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;
            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
            relativeUrl = relativeUrl.Replace("{{datestamp}}", randomDateTimeOffset.ToString());
            dynamic randomArtifactProperties = CreateRandomArtifactProperties(resourceType);
            string nextPageUrl = "";

            OntologyAssets retrievedOntologyAssets = 
                CreateArtiFactFromRandomData(randomArtifactProperties, nextPageUrl);

            TerminologyArtifact outputTerminologyArtifact = 
                CreateTerminologyArtiFactFromRandomData(randomArtifactProperties);

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls()).
                    Returns(terminologyPolls);

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveAllCodingSystemsAsync(relativeUrl))
                    .ReturnsAsync(retrievedOntologyAssets);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(retrievedTerminologyPoll)).
                    ReturnsAsync(retrievedTerminologyPoll);

            // when
            await this.terminologyMetadataOrchestrationService.RetrieveArtifacMetadataAsync(resourceType);

            // then
            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once);

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(relativeUrl),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(retrievedTerminologyPoll),
                    Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}