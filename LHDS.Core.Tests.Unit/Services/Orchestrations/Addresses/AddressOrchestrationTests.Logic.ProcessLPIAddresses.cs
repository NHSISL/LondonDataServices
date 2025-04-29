// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessLPIAddressesAsync()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string randomFile = GetRandomString();
            string inputDpaCsvFile = randomFile;
            int inputSkipCounter = 0;
            int inputBatchSize = this.batchSize;
            List<string> returnedStringList = CreateRandomStringList();
            IQueryable<Address> randomExistingAddresses = CreateRandomAddresses(2);
            IQueryable<Address> databaseExistingAddresses = randomExistingAddresses.DeepClone();
            IQueryable<Address> lpiFileExistingAddresses = randomExistingAddresses.DeepClone();
            List<Address> randomAddressList = CreateRandomAddresses(2).ToList();
            List<Address> newAddresses = randomAddressList.DeepClone();
            List<Address> lpiFileAddresses = [.. lpiFileExistingAddresses.DeepClone(), .. newAddresses.DeepClone()];

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, inputSkipCounter))
                    .ReturnsAsync(returnedStringList);

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, inputSkipCounter + inputBatchSize))
                    .ReturnsAsync([]);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapLPIDataToAddressesAsync(inputDpaCsvFile, inputBatchSize, inputSkipCounter))
                    .ReturnsAsync(lpiFileAddresses);

            this.addressProcessingServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(databaseExistingAddresses);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessLPIAddressesAsync(inputDpaCsvFile);

            // Then
            this.fileBrokerMock.Verify(broker =>
               broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, inputSkipCounter),
                   Times.Once);

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, inputSkipCounter + inputBatchSize),
                    Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapLPIDataToAddressesAsync(inputDpaCsvFile, inputBatchSize, inputSkipCounter),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(It.Is(SameAddressesAs(newAddresses)), inputDpaCsvFile),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(
                    It.Is(SameAddressesAs(lpiFileExistingAddresses.ToList())),
                    inputDpaCsvFile),
                        Times.Once);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

