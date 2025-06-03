// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessDocumentsAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            Guid randomGuid = Guid.NewGuid();
            Guid supplierId = randomGuid;
            List<string> randomFileNames = GetRandomStrings();
            List<string> processedFileNames = randomFileNames.DeepClone();
            List<string> expectedFileNames = processedFileNames.DeepClone();


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

            emisLandingOrchestrationServiceMock.Setup(service =>
                service.ProcessSubscriberFiles(inputSubscriberCredential, supplierId))
                    .ReturnsAsync(processedFileNames);

            emisLandingOrchestrationServiceMock.Setup(service =>
                service.MarkItemsAsDeleteThatHasNotBeenSeen())
                    .Returns(ValueTask.CompletedTask);

            // when
            await emisLandingOrchestrationServiceMock.Object.ProcessAsync(
                subscriberCredential: inputSubscriberCredential, supplierId);

            // then

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.ProcessSubscriberFiles(inputSubscriberCredential, supplierId),
                    Times.Once);

            emisLandingOrchestrationServiceMock.Verify(service =>
                service.MarkItemsAsDeleteThatHasNotBeenSeen(),
                    Times.Once);

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