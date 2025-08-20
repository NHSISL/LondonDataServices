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
            DateTimeOffset currentDateTime = GetRandomDateTimeOffset();
            Guid subscriberAgreementId = Guid.NewGuid();
            List<IngestionTracking> randomIngestionTrackingFirstBatch = CreateRandomIngestionTrackings();
            List<IngestionTracking> randomIngestionTrackingSecondBatch = CreateRandomIngestionTrackings();

            randomIngestionTrackingFirstBatch.ForEach(ingestionTracking =>
            {
                ingestionTracking.Batch = batchReference;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.IsBatchComplete = !isBatchComplete;
            });

            List<IngestionTracking> randomIngestionTrackingBatch =
                [.. randomIngestionTrackingFirstBatch, .. randomIngestionTrackingSecondBatch];

            List<IngestionTracking> storageIngestionTrackingItems = randomIngestionTrackingBatch;
            List<IngestionTracking> setAsBatchCompleteItems = randomIngestionTrackingFirstBatch.DeepClone();

            Guid inputIngestionTrackingId = randomIngestionTrackingFirstBatch.First().Id;
            IngestionTracking batchCompleteIngestionTrackingItem = randomIngestionTrackingFirstBatch.First();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(inputIngestionTrackingId))
                    .ReturnsAsync(batchCompleteIngestionTrackingItem);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackingItems.AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            setAsBatchCompleteItems.ForEach(batchCompleteIngestionTracking =>
            {
                batchCompleteIngestionTracking.IsBatchComplete = isBatchComplete;
                batchCompleteIngestionTracking.LastBatchCompleteCheck = currentDateTime;

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            setAsBatchCompleteItems.ForEach(batchCompleteIngestionTracking =>
            {
                this.ingestionTrackingServiceMock.Verify(service =>
                    service.ModifyIngestionTrackingAsync(
                        It.Is(SameIngestionTrackingAs(batchCompleteIngestionTracking))),
                            Times.Once);
            });

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
