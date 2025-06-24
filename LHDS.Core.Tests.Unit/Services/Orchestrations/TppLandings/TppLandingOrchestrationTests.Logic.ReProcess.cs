// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
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
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            List<string> randomFileNames = GetRandomStrings();

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

            List<IngestionTracking> ingestionTrackings = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTime,
                fileNames: randomFileNames,
                supplierId: randomSupplierId);

            foreach (IngestionTracking ingestionTracking in ingestionTrackings)
            {
                ingestionTracking.IsDownloaded = false;
                ingestionTracking.RetryCount = 1;

                tppOrchestrationServiceMock.Setup(service =>
                    service.ProcessFileAsync(ingestionTracking.FileName, ingestionTracking.SupplierId))
                        .ReturnsAsync(inputSupplierId);
            }

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(ingestionTrackings.AsQueryable());

            // when
            await tppOrchestrationServiceMock.Object.ReProcessAsync(
                supplierId: inputSupplierId);

            // then
            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            foreach (IngestionTracking ingestionTracking in ingestionTrackings)
            {
                tppOrchestrationServiceMock.Verify(service =>
                    service.ProcessFileAsync(ingestionTracking.FileName, ingestionTracking.SupplierId),
                        Times.Once);
            }

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