// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveIngestionTrackingByIdAsync()
        {
            // Given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RemoveIngestionTrackingByIdAsync(randomIngestionTracking.Id))
                    .ReturnsAsync(storageIngestionTracking);

            // When
            IngestionTracking actualIngestionTracking =
                await this.ingestionTrackingProcessingService
                    .RemoveIngestionTrackingByIdAsync(randomIngestionTracking.Id);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RemoveIngestionTrackingByIdAsync(randomIngestionTracking.Id),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
