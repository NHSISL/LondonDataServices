// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDownloadByFileNameAndLogAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Document randomDocument = CreateRandomDocument();
            Document inputDocument = randomDocument;
            Document expectedDocument = inputDocument.DeepClone();

            this.emisLandingExtractionOrchestrationServiceMock.Setup(service =>
                service.RetrieveDownloadByFileNameAsync(
                    inputDocument.FileName,
                    It.Is(SameSubscriberCredentialAs(inputSubscriberCredential))))
                        .ReturnsAsync(inputDocument.DocumentData);

            // when
            Document actualDocument =
                await this.emisLandingCoordinationService.RetrieveDownloadByFileNameAsync(inputDocument.FileName);

            // then
            actualDocument.Should().BeEquivalentTo(expectedDocument);

            this.emisLandingExtractionOrchestrationServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(
                    inputDocument.FileName,
                    It.Is(SameSubscriberCredentialAs(inputSubscriberCredential))),
                        Times.Once);

            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

