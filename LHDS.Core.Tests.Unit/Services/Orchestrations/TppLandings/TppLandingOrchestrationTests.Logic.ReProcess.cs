// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Tpp;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldReProcessOnlyFilteredIngestionTrackingsAsync()
        {
            // given
            Guid supplierId = Guid.NewGuid();
            Guid otherSupplierId = Guid.NewGuid();
            DateTimeOffset currentDateTime = GetRandomDateTimeOffset();

            IngestionTracking validItem = CreateRandomIngestionTracking(currentDateTime);
            validItem.SupplierId = supplierId;
            validItem.IsDownloaded = false;
            validItem.RetryCount = 3;

            IngestionTracking invalidWrongSupplier = CreateRandomIngestionTracking(currentDateTime);
            invalidWrongSupplier.SupplierId = otherSupplierId;
            invalidWrongSupplier.IsDownloaded = false;
            invalidWrongSupplier.RetryCount = 1;

            IngestionTracking invalidAlreadyDownloaded = CreateRandomIngestionTracking(currentDateTime);
            invalidAlreadyDownloaded.SupplierId = supplierId;
            invalidAlreadyDownloaded.IsDownloaded = true;
            invalidAlreadyDownloaded.RetryCount = 1;

            IngestionTracking invalidRetryCountTooHigh = CreateRandomIngestionTracking(currentDateTime);
            invalidRetryCountTooHigh.SupplierId = supplierId;
            invalidRetryCountTooHigh.IsDownloaded = false;
            invalidRetryCountTooHigh.RetryCount = 4;

            List<IngestionTracking> allIngestionTrackings =
            [
                validItem,
                invalidWrongSupplier,
                invalidAlreadyDownloaded,
                invalidRetryCountTooHigh
            ];

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                subscriberAgreementProcessingServiceMock.Object,
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
                service.ProcessFileAsync(validItem.FileName, supplierId))
                    .ReturnsAsync(validItem.Id);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(allIngestionTrackings.AsQueryable());

            // when
            List<Guid> actualProcessedIds =
                await tppOrchestrationServiceMock.Object.ReProcessAsync(supplierId);

            // then
            actualProcessedIds.Should().BeEquivalentTo([validItem.Id]);

            tppOrchestrationServiceMock.Verify(service =>
                service.ProcessFileAsync(validItem.FileName, supplierId),
                    Times.Once);

            tppOrchestrationServiceMock.Verify(service =>
                service.ProcessFileAsync(invalidWrongSupplier.FileName, invalidWrongSupplier.SupplierId),
                    Times.Never);

            tppOrchestrationServiceMock.Verify(service =>
                service.ProcessFileAsync(invalidAlreadyDownloaded.FileName, invalidAlreadyDownloaded.SupplierId),
                    Times.Never);

            tppOrchestrationServiceMock.Verify(service =>
                service.ProcessFileAsync(invalidRetryCountTooHigh.FileName, invalidRetryCountTooHigh.SupplierId),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}