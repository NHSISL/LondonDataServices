// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldBulkModifyIngestionTrackingAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = randomIngestionTrackingItems;

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(inputIngestionTrackingItems, 10000))
                    .Returns(ValueTask.CompletedTask);

            // when
            await ingestionTrackingServiceMock.Object
                .BulkModifyIngestionTrackingAsync(inputIngestionTrackingItems);

            // then
            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(inputIngestionTrackingItems, 10000),
                    Times.Exactly(1));

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}