// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessSubscriberFilesAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = randomGuid;
            Download download = new Download { SubscriberCredential = inputSubscriberCredential };
            int count = GetRandomNumber();
            List<string> randomExternalFileNames = GetRandomStrings(count);
            List<string> randomProcessedFileNames = GetRandomStrings(count);
            List<string> expectedOutput = randomProcessedFileNames.DeepClone();

            var emisLandingOrchestrationServiceMock = new Mock<EmisLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                this.blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            downloadProcessingServiceMock.Setup(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(download))))
                    .ReturnsAsync(randomExternalFileNames);

            for (int i = 0; i < randomExternalFileNames.Count; i++)
            {
                emisLandingOrchestrationServiceMock.Setup(service =>
                    service.ProcessFileAsync(inputSubscriberCredential, supplierId, randomExternalFileNames[i]))
                        .ReturnsAsync(randomProcessedFileNames[i]);
            }

            // when
            List<string> actualOutput = await emisLandingOrchestrationServiceMock.Object.ProcessSubscriberFiles(
                subscriberCredential: inputSubscriberCredential, supplierId);

            // then
            actualOutput.Should().BeEquivalentTo(expectedOutput);

            downloadProcessingServiceMock.Verify(service =>
                service.RetrieveListOfDownloadsToProcessAsync(It.Is(SameDownloadAs(download))),
                    Times.Once);

            for (int i = 0; i < randomExternalFileNames.Count; i++)
            {
                emisLandingOrchestrationServiceMock.Verify(service =>
                    service.ProcessFileAsync(inputSubscriberCredential, supplierId, randomExternalFileNames[i]),
                        Times.Once);
            }

            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}