// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.Identity.Client;
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
                CreateRandomTerminologyPolls(resourceType, lastPoll: randomDateTimeOffset.AddDays(-3));

            TerminologyPoll retrievedTerminologyPoll = terminologyPolls.First();
            string relativeUrl = this.terminologyMetadataConfiguration.ResourceURL;
            relativeUrl = relativeUrl.Replace("{{resourceType}}", resourceType);
            relativeUrl = relativeUrl.Replace("{{datestamp}}", randomDateTimeOffset.ToString());
            dynamic randomArtifactProperties = CreateRandomArtifactProperties(resourceType);

            OntologyAssets retrievedOntologyAssets = 
                CreateArtiFactFromRandomData(randomArtifactProperties);

            List<TerminologyArtifact> outputTerminologyArtifacts = 
                CreateTerminologyArtiFactFromRandomData(randomArtifactProperties);

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls()).
                    Returns(terminologyPolls);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.ontologyProcessingServiceMock.SetupSequence(service =>
                service.RetrieveAllCodingSystemsAsync(relativeUrl))
                    .ReturnsAsync(retrievedOntologyAssets);

            for (int i = 0; i < retrievedOntologyAssets.Assets.Count; i++)

            {
                var item = retrievedOntologyAssets.Assets[i];
                string user = GetRandomString();
                DateTimeOffset 

                TerminologyArtifact terminologyArtifact = new TerminologyArtifact
                {
                    FullUrl = item.FullUrl,
                    ResourceType = item.ResourceType,
                    Version = item.Version,
                    Name = item.Name,
                    Title = item.Title,
                    Status = item.Status,
                    LastUpdated = item.LastUpdated,
                    IsCore = false,
                    IsDownloaded = false,
                    CreatedBy = item.LastUpdated,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedDate = item.UpdatedDate,
                    CreatedDate = item.CreatedDate
                };
                
                this.terminologyArtifactProcessingServiceMock.Setup(service =>
                    service.ModifyOrAddTerminologyArtifactAsync(terminologyArtifact)
                    .Returns(Task.CompletedTask);
            }

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