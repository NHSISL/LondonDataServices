// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldBulkAddOrModifyBatchAsync()
        {
            // given
            int randomBatchSize = GetRandomNumber();
            int inputBatchSize = randomBatchSize;
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            List<Address> randomNewAddresses = CreateRandomAddresses(
                count: 1,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<Address> randomExistingAddresses = CreateRandomAddresses(
                count: 1,
                dateTimeOffset: randomDateTimeOffset,
                userId: randomEntraUser.EntraUserId);

            List<Address> inputAddresses = new List<Address>();
            inputAddresses.AddRange(randomNewAddresses);
            inputAddresses.AddRange(randomExistingAddresses);
            int batchCount = GetBatchSize(inputAddresses.Count, inputBatchSize);

            var addressServiceMock = new Mock<AddressService>(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.securityBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.auditBrokerMock.Object)
            {
                CallBase = true
            };

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressesAsync())
                    .ReturnsAsync(randomExistingAddresses.AsQueryable());

            int totalRecords = inputAddresses.Count;

            for (int i = 0; i < totalRecords; i += randomBatchSize)
            {
                var batch = inputAddresses.Skip(i).Take(randomBatchSize).ToList();
                List<Guid> batchIds = batch.Select(address => address.Id).ToList();
                List<Guid> storageIds = randomExistingAddresses.Select(address => address.Id).ToList();

                var existingIds = storageIds
                    .Where(storageId => batchIds.Contains(storageId))
                    .Select(storageId => storageId)
                    .ToList();

                var existingAddresses = batch.Where(address => existingIds.Contains(address.Id)).ToList();
                var newAddresses = batch.Where(address => !existingIds.Contains(address.Id)).ToList();

                addressServiceMock.Setup(service =>
                    service.ValidateAddressesAndAssignIdAndAuditOnAddAsync(newAddresses, randomFileName))
                        .ReturnsAsync(newAddresses);

                this.storageBrokerMock.Setup(broker =>
                    broker.BulkInsertAddressesAsync(newAddresses))
                        .Returns(ValueTask.CompletedTask);

                addressServiceMock.Setup(service =>
                    service.ValidateAddressesAndAssignAuditOnModifyAsync(existingAddresses, randomFileName))
                        .ReturnsAsync(existingAddresses);

                this.storageBrokerMock.Setup(broker =>
                    broker.BulkUpdateAddressesAsync(existingAddresses))
                        .Returns(ValueTask.CompletedTask);
            }

            // when
            await addressServiceMock.Object
                .BulkAddOrModifyBatchAsync(inputAddresses, inputFileName, inputBatchSize);

            // then
            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressesAsync(),
                    Times.Exactly(batchCount));

            for (int i = 0; i < totalRecords; i += randomBatchSize)
            {
                var batch = inputAddresses.Skip(i).Take(randomBatchSize).ToList();
                List<Guid> batchIds = batch.Select(address => address.Id).ToList();
                List<Guid> storageIds = randomExistingAddresses.Select(address => address.Id).ToList();

                var existingIds = storageIds
                    .Where(storageId => batchIds.Contains(storageId))
                    .Select(storageId => storageId)
                    .ToList();

                var existingAddresses = batch.Where(address => existingIds.Contains(address.Id)).ToList();
                var newAddresses = batch.Where(address => !existingIds.Contains(address.Id)).ToList();

                addressServiceMock.Verify(service =>
                    service.ValidateAddressesAndAssignIdAndAuditOnAddAsync(newAddresses, randomFileName),
                        Times.Once);

                this.storageBrokerMock.Verify(broker =>
                    broker.BulkInsertAddressesAsync(newAddresses),
                        Times.Once);

                addressServiceMock.Verify(service =>
                    service.ValidateAddressesAndAssignAuditOnModifyAsync(existingAddresses, randomFileName),
                        Times.Once);

                this.storageBrokerMock.Verify(broker =>
                    broker.BulkUpdateAddressesAsync(existingAddresses),
                        Times.Once);
            }

            addressServiceMock.Verify(service =>
                service.BulkAddOrModifyBatchAsync(inputAddresses, inputFileName, inputBatchSize),
                    Times.Once);

            addressServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}