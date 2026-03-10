// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldBulkAddOrModifyBatchAsync()
        {
            // given
            int randomBatchSize = GetRandomNumber();
            int inputBatchSize = randomBatchSize;
            List<IngestionTracking> randomNewIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> randomExistingIngestionTrackingItems = CreateRandomIngestionTrackings().ToList();
            List<IngestionTracking> inputIngestionTrackingItems = new List<IngestionTracking>();
            inputIngestionTrackingItems.AddRange(randomNewIngestionTrackingItems);
            inputIngestionTrackingItems.AddRange(randomExistingIngestionTrackingItems);
            int batchCount = GetBatchSize(inputIngestionTrackingItems.Count, inputBatchSize);

            Mock<IngestionTrackingService> ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomExistingIngestionTrackingItems.AsQueryable());

            int totalRecords = inputIngestionTrackingItems.Count;

            for (int i = 0; i < totalRecords; i += randomBatchSize)
            {
                var batch = inputIngestionTrackingItems.Skip(i).Take(randomBatchSize).ToList();
                List<Guid> batchIds = batch.Select(address => address.Id).ToList();
                List<Guid> storageIds = randomExistingIngestionTrackingItems.Select(address => address.Id).ToList();

                var existingIds = storageIds
                    .Where(storageId => batchIds.Contains(storageId))
                    .Select(storageId => storageId)
                    .ToList();

                var existingIngestionTrackingItems = batch.Where(address => existingIds.Contains(address.Id)).ToList();
                var newIngestionTrackingItems = batch.Where(address => !existingIds.Contains(address.Id)).ToList();

                ingestionTrackingServiceMock.Setup(service =>
                    service.AssignAuditValuesAndValidateOnBulkAddAsync(newIngestionTrackingItems))
                        .ReturnsAsync(newIngestionTrackingItems);

                this.storageBrokerMock.Setup(broker =>
                    broker.BulkInsertIngestionTrackingsAsync(newIngestionTrackingItems))
                        .Returns(ValueTask.CompletedTask);

                ingestionTrackingServiceMock.Setup(service =>
                    service.AssignAuditValuesAndValidateOnBulkModifyAsync(existingIngestionTrackingItems))
                        .ReturnsAsync(existingIngestionTrackingItems);

                this.storageBrokerMock.Setup(broker =>
                    broker.BulkUpdateIngestionTrackingsAsync(existingIngestionTrackingItems))
                        .Returns(ValueTask.CompletedTask);
            }

            // when
            await ingestionTrackingServiceMock.Object.BulkAddOrModifyBySplittingIntoBatchesAsync(
                inputIngestionTrackingItems, inputBatchSize);

            // then
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Exactly(batchCount));

            for (int i = 0; i < totalRecords; i += randomBatchSize)
            {
                var batch = inputIngestionTrackingItems.Skip(i).Take(randomBatchSize).ToList();
                List<Guid> batchIds = batch.Select(address => address.Id).ToList();
                List<Guid> storageIds = randomExistingIngestionTrackingItems.Select(address => address.Id).ToList();

                var existingIds = storageIds
                    .Where(storageId => batchIds.Contains(storageId))
                    .Select(storageId => storageId)
                    .ToList();

                var existingIngestionTrackingItems = batch.Where(address => existingIds.Contains(address.Id)).ToList();
                var newIngestionTrackingItems = batch.Where(address => !existingIds.Contains(address.Id)).ToList();

                if (newIngestionTrackingItems.Count != 0)
                {
                    ingestionTrackingServiceMock.Verify(service =>
                        service.AssignAuditValuesAndValidateOnBulkAddAsync(newIngestionTrackingItems),
                            Times.Once);

                    this.storageBrokerMock.Verify(broker =>
                        broker.BulkInsertIngestionTrackingsAsync(newIngestionTrackingItems),
                            Times.Once);
                }

                if (existingIngestionTrackingItems.Count != 0)
                {
                    ingestionTrackingServiceMock.Verify(service =>
                        service.AssignAuditValuesAndValidateOnBulkModifyAsync(existingIngestionTrackingItems),
                        Times.Once);

                    this.storageBrokerMock.Verify(broker =>
                        broker.BulkUpdateIngestionTrackingsAsync(existingIngestionTrackingItems),
                            Times.Once);
                }
            }

            ingestionTrackingServiceMock.Verify(service =>
                service.BulkAddOrModifyBySplittingIntoBatchesAsync(inputIngestionTrackingItems, inputBatchSize),
                    Times.Once);

            ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}