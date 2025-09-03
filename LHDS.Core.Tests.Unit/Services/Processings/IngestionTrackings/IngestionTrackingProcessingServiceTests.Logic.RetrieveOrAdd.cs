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
        public async Task ShouldRetrieveIngestionTrackingIfOneExistsAndNotAddAsync()
        {
            // Given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ReturnsAsync(value: storageIngestionTracking);

            // When
            IngestionTracking actualIngestionTracking = await this.ingestionTrackingProcessingService
                .RetrieveOrAddIngestionTrackingAsync(inputIngestionTracking);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(inputIngestionTracking),
                    Times.Never);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddIngestionTrackingIfOneDoesNotExistAsync()
        {
            // Given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ReturnsAsync(value: null);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(inputIngestionTracking))
                    .ReturnsAsync(storageIngestionTracking);

            // When
            await this.ingestionTrackingProcessingService.RetrieveOrAddIngestionTrackingAsync(inputIngestionTracking);

            // Then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(inputIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
