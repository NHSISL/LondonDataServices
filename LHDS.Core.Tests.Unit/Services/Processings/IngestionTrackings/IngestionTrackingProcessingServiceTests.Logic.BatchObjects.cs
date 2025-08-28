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
            Guid supplierId = Guid.NewGuid();
            Guid subscriberAgreementId = Guid.NewGuid();
            string batchReference = GetRandomString();
            List<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();

            randomIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
            });

            List<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;

            List<string> ingestionTrackingObjects = randomIngestionTrackings
                .Where(ingestionTrackingObject =>
                    ingestionTrackingObject.Batch == batchReference &&
                    ingestionTrackingObject.SubscriberAgreementId == subscriberAgreementId)
                        .Select(ingestionTracking => ingestionTracking.ObjectName).ToList();

            List<string> expectedIngestionTracking = ingestionTrackingObjects.DeepClone();

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings.AsQueryable());

            // When
            List<string> actualIngestionTracking =
                await this.ingestionTrackingProcessingService
                    .RetrieveObjectsInBatchByBatchReferenceAsync(
                        batchReference, subscriberAgreementId);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldRetrieveDecryptedObjectsInBatchByBatchReferenceAndDecryptedAsync(bool decrypted)
        {
            // Given
            Guid supplierId = Guid.NewGuid();
            Guid subscriberAgreementId = Guid.NewGuid();
            string batchReference = GetRandomString();
            List<IngestionTracking> randomDecryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomDecryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = true;
            });

            List<IngestionTracking> randomEncryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomEncryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = false;
            });

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>();
            storageIngestionTrackings.AddRange(randomDecryptedIngestionTrackings);
            storageIngestionTrackings.AddRange(randomEncryptedIngestionTrackings);

            List<string> ingestionTrackingObjects = storageIngestionTrackings

                .Where(ingestionTrackingObject =>
                    ingestionTrackingObject.Batch == batchReference
                    && ingestionTrackingObject.Decrypted == decrypted)

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
                    subscriberAgreementId,
                    decrypted);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveDecryptedObjectsInBatchByBatchReferenceAndSubscriberAgreementIsAsync()
        {
            // Given
            Guid supplierId = Guid.NewGuid();
            Guid subscriberAgreementId = Guid.NewGuid();
            string batchReference = GetRandomString();
            List<IngestionTracking> randomDecryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomDecryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = true;
            });

            List<IngestionTracking> randomEncryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomEncryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = false;
            });

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>();
            storageIngestionTrackings.AddRange(randomDecryptedIngestionTrackings);
            storageIngestionTrackings.AddRange(randomEncryptedIngestionTrackings);

            List<string> ingestionTrackingObjects = storageIngestionTrackings

                .Where(ingestionTrackingObject =>
                    ingestionTrackingObject.Batch == batchReference
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
                    batchReference: batchReference,
                    subscriberAgreementId: subscriberAgreementId);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldRetrieveDecryptedObjectsByBatchReferenceAndDecryptedAndSubscriberAgreementIsAsync(
            bool decrypted)
        {
            // Given
            Guid supplierId = Guid.NewGuid();
            Guid subscriberAgreementId = Guid.NewGuid();
            string batchReference = GetRandomString();
            List<IngestionTracking> randomDecryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomDecryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
                ingestionTracking.Decrypted = decrypted;
            });

            List<IngestionTracking> randomEncryptedIngestionTrackings = CreateRandomIngestionTrackings();

            randomEncryptedIngestionTrackings.ForEach(ingestionTracking =>
            {
                ingestionTracking.SupplierId = supplierId;
                ingestionTracking.SubscriberAgreementId = subscriberAgreementId;
                ingestionTracking.Batch = batchReference;
            });

            List<IngestionTracking> storageIngestionTrackings = new List<IngestionTracking>();
            storageIngestionTrackings.AddRange(randomDecryptedIngestionTrackings);
            storageIngestionTrackings.AddRange(randomEncryptedIngestionTrackings);

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
                    subscriberAgreementId,
                    decrypted);

            // Then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
