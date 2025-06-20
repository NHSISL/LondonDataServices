// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Services.Orchestrations.Tpp;
using Moq;
using Xunit;


namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldReProcessAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            tppOrchestrationServiceMock.Setup(service =>
                service.ProcessFileAsync(inputFileName, inputSupplierId))
                    .ReturnsAsync(inputSupplierId);

            // when
            Guid returnedGuid = await tppOrchestrationServiceMock.Object.ProcessAsync(
                fileName: inputFileName,
                supplierId: inputSupplierId);

            // then
            tppOrchestrationServiceMock.Verify(service =>
                service.ProcessFileAsync(inputFileName, inputSupplierId),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}