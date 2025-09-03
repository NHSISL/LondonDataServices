// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyIngestionTrackingAsync()
        {
            // Given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = inputIngestionTracking;
            IngestionTracking expectedIngestionTracking = storageIngestionTracking.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.ModifyIngestionTrackingAsync(inputIngestionTracking))
                    .ReturnsAsync(storageIngestionTracking);

            // When
            await this.ingestionTrackingProcessingService.ModifyIngestionTrackingAsync(inputIngestionTracking);

            // Then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.ModifyIngestionTrackingAsync(inputIngestionTracking),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
