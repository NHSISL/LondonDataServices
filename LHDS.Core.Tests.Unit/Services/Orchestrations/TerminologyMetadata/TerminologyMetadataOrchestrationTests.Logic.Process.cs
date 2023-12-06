// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using System;
using System.Linq;
using System.Threading.Tasks;
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
            IQueryable<TerminologyPoll> terminologyPolls = CreateRandomTerminologyPolls(resourceType);

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls()).
                    Returns(terminologyPolls);

            string url = $"{{terminology-server}}/{resourceType}?_lastUpdated=ge{{datestamp}}" +
                        "&_name=dm+dCOMBINATION_PACK_IND&_elements=name,title,url,version,status&_count=10";

            this.ontologyProcessingServiceMock.Setup(service =>
                service.RetrieveAllCodingSystemsAsync(url))
                    .ReturnsAsync(ontolgyAsset);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.terminologyPollProcessingServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(terminologyPolls.First))..ReturnsAsync(terminologyPoll);

            // when
            await this.terminologyMetadataOrchestrationService.RetrieveArtifacMetadataAsync(resourceType);

            // then
            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once);

            this.ontologyProcessingServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(url),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.terminologyPollProcessingServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(),
                    Times.Once);

            this.terminologyPollProcessingServiceMock.VerifyNoOtherCalls();
            this.ontologyProcessingServiceMock.VerifyNoOtherCalls();
            this.terminologyArtifactProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}