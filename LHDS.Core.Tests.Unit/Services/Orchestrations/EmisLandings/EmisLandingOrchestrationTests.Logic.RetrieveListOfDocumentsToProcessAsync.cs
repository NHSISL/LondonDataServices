// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcessAsyncAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            int randomNumber = GetRandomNumber();
            List<string> randomFileNames = GetRandomStrings(count: randomNumber);
            List<string> storageFileNames = randomFileNames;
            List<string> expectedFileNames = storageFileNames.DeepClone();

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
            };

            this.downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))))
                    .ReturnsAsync(storageFileNames);

            // when
            List<string> actualFileNames = await this.emisLandingOrchestrationService
                .RetrieveListOfDocumentsToProcessAsync(subscriberCredential: inputSubscriberCredential);

            // then
            actualFileNames.Should().BeEquivalentTo(expectedFileNames);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.auditServiceMock.VerifyNoOtherCalls();
        }
    }
}