using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.IngestionTrackings;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldAddIngestionTrackingAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(randomDateTimeOffset);
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertIngestionTrackingAsync(inputIngestionTracking))
                    .ReturnsAsync(storageIngestionTracking);

            // when
            IngestionTracking actualIngestionTracking = await this.ingestionTrackingService
                .AddIngestionTrackingAsync(inputIngestionTracking);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAsync(inputIngestionTracking),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}