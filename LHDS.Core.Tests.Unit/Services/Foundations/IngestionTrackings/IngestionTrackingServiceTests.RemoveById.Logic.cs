// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveIngestionTrackingByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputIngestionTrackingId = randomId;
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedInputIngestionTracking = storageIngestionTracking;
            IngestionTracking deletedIngestionTracking = expectedInputIngestionTracking;
            IngestionTracking expectedIngestionTracking = deletedIngestionTracking.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(inputIngestionTrackingId))
                    .ReturnsAsync(storageIngestionTracking);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteIngestionTrackingAsync(expectedInputIngestionTracking))
                    .ReturnsAsync(deletedIngestionTracking);

            // when
            IngestionTracking actualIngestionTracking = await this.ingestionTrackingService
                .RemoveIngestionTrackingByIdAsync(inputIngestionTrackingId);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(inputIngestionTrackingId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteIngestionTrackingAsync(expectedInputIngestionTracking),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}