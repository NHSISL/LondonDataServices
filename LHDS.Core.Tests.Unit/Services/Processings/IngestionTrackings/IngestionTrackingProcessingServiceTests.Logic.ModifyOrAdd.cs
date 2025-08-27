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

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyIngestionTrackingIfOneExistsAndNotAddAsync()
        {
            // Given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking storageIngestionTracking = randomIngestionTracking;
            IngestionTracking modifiedIngestionTracking = storageIngestionTracking.DeepClone();
            modifiedIngestionTracking.UpdatedDate = DateTimeOffset.UtcNow;
            IngestionTracking updatedIngestionTracking = modifiedIngestionTracking.DeepClone();
            IngestionTracking expectedIngestionTracking = updatedIngestionTracking;

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(modifiedIngestionTracking.Id))
                    .ReturnsAsync(value: storageIngestionTracking);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(modifiedIngestionTracking))
                    .ReturnsAsync(value: updatedIngestionTracking);

            // When
            IngestionTracking actualIngestionTracking = await this.ingestionTrackingProcessingService
                .ModifyOrAddIngestionTrackingAsync(modifiedIngestionTracking);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(modifiedIngestionTracking.Id),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(modifiedIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.AddIngestionTrackingAsync(modifiedIngestionTracking),
                    Times.Never);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddIngestionTrackingIfIngestionTrackingDoesNotExistsAsync()
        {
            // Given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking.DeepClone();
            IngestionTracking expectedIngestionTracking = storageIngestionTracking;

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id))
                    .ReturnsAsync(value: null);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.AddIngestionTrackingAsync(inputIngestionTracking))
                    .ReturnsAsync(value: storageIngestionTracking);

            // When
            await this.ingestionTrackingProcessingService.ModifyOrAddIngestionTrackingAsync(inputIngestionTracking);

            // Then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTracking.Id),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
            service.AddIngestionTrackingAsync(inputIngestionTracking),
            Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(inputIngestionTracking),
                    Times.Never);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
