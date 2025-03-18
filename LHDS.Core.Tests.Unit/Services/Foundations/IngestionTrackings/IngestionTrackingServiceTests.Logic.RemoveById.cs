// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            Guid randomId = Guid.NewGuid();
            Guid inputIngestionTrackingId = randomId;

            IngestionTracking randomIngestionTracking = 
                CreateRandomIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedInputIngestionTracking = storageIngestionTracking;
            IngestionTracking deletedIngestionTracking = expectedInputIngestionTracking;
            IngestionTracking expectedIngestionTracking = deletedIngestionTracking.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteIngestionTrackingAsync(expectedInputIngestionTracking),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}