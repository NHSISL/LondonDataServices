// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldMarkAsBatchCompleteAsync(bool isBatchComplete)
        {
            // Given
            string batchReference = GetRandomString();
            List<IngestionTracking> randomIngestionTrackingsFirstBatch = CreateRandomIngestionTrackings();
            List<IngestionTracking> randomIngestionTrackingsSecondBatch = CreateRandomIngestionTrackings();
            Guid inputIngestionTrackingId = randomIngestionTrackingsFirstBatch.First().Id;
            IngestionTracking batchCompleteIngestionTrackingItem = randomIngestionTrackingsFirstBatch.First();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTrackingId))
                    .ReturnsAsync(batchCompleteIngestionTrackingItem);

            randomIngestionTrackingsFirstBatch.ForEach(ingestionTracking =>
            {
                ingestionTracking.Batch = batchReference;
                ingestionTracking.IsBatchComplete = !isBatchComplete;
            });

            List<IngestionTracking> randomIngestionTrackingsBatch =
                [.. randomIngestionTrackingsFirstBatch, .. randomIngestionTrackingsFirstBatch];

            List<IngestionTracking> storageIngestionTrackings = randomIngestionTrackingsBatch;
            List<IngestionTracking> setAsBatchCompleteItems = randomIngestionTrackingsFirstBatch.DeepClone();

            setAsBatchCompleteItems.ForEach(batchCompleteIngestionTracking =>
            {
                batchCompleteIngestionTracking.IsBatchComplete = isBatchComplete;

                this.ingestionTrackingServiceMock.Setup(service =>
                    service.ModifyIngestionTrackingAsync(
                        It.Is(SameIngestionTrackingAs(batchCompleteIngestionTracking))))
                            .ReturnsAsync(batchCompleteIngestionTracking);
            });

            // When
            await this.ingestionTrackingProcessingService
                .MarkAsBatchCompleteAsync(inputIngestionTrackingId, isBatchComplete);

            // Then
            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTrackingId),
                    Times.Once);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);


            setAsBatchCompleteItems.ForEach(batchCompleteIngestionTracking =>
            {
                this.ingestionTrackingServiceMock.Setup(service =>
                    service.ModifyIngestionTrackingAsync(It.Is(SameIngestionTrackingAs(batchCompleteIngestionTracking))))
                        .ReturnsAsync(batchCompleteIngestionTracking);
            });

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
