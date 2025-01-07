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
        public async Task ShouldModifyIngestionTrackingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTracking randomIngestionTracking = CreateRandomModifyIngestionTracking(randomDateTimeOffset);
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking.DeepClone();
            storageIngestionTracking.UpdatedDate = randomIngestionTracking.CreatedDate;
            IngestionTracking updatedIngestionTracking = inputIngestionTracking;
            IngestionTracking expectedIngestionTracking = updatedIngestionTracking.DeepClone();
            Guid ingestionTrackingId = inputIngestionTracking.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(ingestionTrackingId))
                    .ReturnsAsync(storageIngestionTracking);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateIngestionTrackingAsync(inputIngestionTracking))
                    .ReturnsAsync(updatedIngestionTracking);

            // when
            IngestionTracking actualIngestionTracking =
                await this.ingestionTrackingService.ModifyIngestionTrackingAsync(inputIngestionTracking);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(inputIngestionTracking),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}