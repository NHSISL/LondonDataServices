// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldRetrieveObjectsInBatchByBatchReferenceAsync()
        {
            // Given
            string batchReference = GetRandomString();
            List<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();

            randomIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.Batch = batchReference;
            });

            List<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;

            List<string> ingestionTrackingObjects = randomIngestionTrackings
                .Where(ingestionTrackingObject => ingestionTrackingObject.Batch == batchReference)
                    .Select(ingestionTracking => ingestionTracking.ObjectName).ToList();

            List<string> expectedIngestionTracking = ingestionTrackingObjects.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable());

            // When
            List<string> actualIngestionTracking =
                await this.ingestionTrackingProcessingService
                    .RetrieveObjectsInBatchByBatchReferenceAsync(batchReference);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldRetrieveDecryptedObjectsInBatchByBatchReferenceAsync(bool decrypted)
        {
            // Given
            string batchReference = GetRandomString();
            Guid subscriberAgreementId = Guid.NewGuid();
            List<IngestionTracking> randomDecryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomDecryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = true;
            });

            List<IngestionTracking> randomUnEncryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomUnEncryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = false;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
            });

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>();
            storageIngestionTrackings.AddRange(randomDecryptedIngestionTrackings);
            storageIngestionTrackings.AddRange(randomUnEncryptedIngestionTrackings);

            List<string> ingestionTrackingObjects = storageIngestionTrackings

                .Where(ingestionTrackingObject =>
                    ingestionTrackingObject.Batch == batchReference
                    && ingestionTrackingObject.Decrypted == decrypted
                    && ingestionTrackingObject.SubscriberAgreementId == subscriberAgreementId)

                .Select(ingestionTracking => ingestionTracking.ObjectName)
                    .ToList();

            List<string> expectedIngestionTracking = ingestionTrackingObjects.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable());

            // When
            List<string> actualIngestionTracking =
                await this.ingestionTrackingProcessingService.RetrieveObjectsInBatchByBatchReferenceAsync(
                    batchReference,
                    decrypted,
                    subscriberAgreementId);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
