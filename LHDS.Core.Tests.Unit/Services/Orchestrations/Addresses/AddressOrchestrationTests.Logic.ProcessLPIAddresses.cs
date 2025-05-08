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
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string randomFile = GetRandomString();
            string inputLpiCsvFile = randomFile;
            int inputBatchSize = GetRandomNumber();
            int numberOfBatches = GetRandomNumber();
            int numberOfRecords = inputBatchSize * numberOfBatches;
            List<string> returnedStringList = CreateRandomStringList(numberOfRecords);
            IQueryable<Address> randomExistingAddresses = CreateRandomAddresses(numberOfRecords);
            IQueryable<Address> databaseExistingAddresses = randomExistingAddresses.DeepClone();
            IQueryable<Address> lpiFileExistingAddresses = randomExistingAddresses.DeepClone();
            List<Address> randomAddressList = CreateRandomAddresses(numberOfRecords).ToList();
            List<Address> newAddresses = randomAddressList.DeepClone();

            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchNewAddresses = [.. newAddresses.GetRange(batchStartLine, inputBatchSize)];

                List<Address> batchExisitngAddresses = [
                    .. databaseExistingAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];

                List<Address> lpiFileBatchAddresses = [.. batchNewAddresses, .. batchExisitngAddresses];

                this.fileBrokerMock.Setup(broker =>
                    broker.ReadLinesBatchAsync(inputLpiCsvFile, inputBatchSize, i * inputBatchSize))
                        .ReturnsAsync(returnedStringList.GetRange(batchStartLine, inputBatchSize));

                addressOrchestrationServiceMock.Setup(service =>
                    service.MapLPIDataToAddressesAsync(inputLpiCsvFile, inputBatchSize, i * inputBatchSize))
                        .ReturnsAsync(lpiFileBatchAddresses);

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(databaseExistingAddresses);
            }

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(inputLpiCsvFile, inputBatchSize, numberOfRecords))
                    .ReturnsAsync([]);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessLPIAddressesAsync(inputLpiCsvFile, inputBatchSize);

            // Then
            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchNewAddresses = [.. newAddresses.GetRange(batchStartLine, inputBatchSize)];

                List<Address> batchExisitngAddresses = [
                    .. databaseExistingAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];

                List<Address> lpiFileBatchAddresses = [.. batchNewAddresses, .. batchExisitngAddresses];

                this.fileBrokerMock.Verify(broker =>
                    broker.ReadLinesBatchAsync(inputLpiCsvFile, inputBatchSize, i * inputBatchSize),
                        Times.Once);

                addressOrchestrationServiceMock.Verify(service =>
                    service.MapLPIDataToAddressesAsync(inputLpiCsvFile, inputBatchSize, i * inputBatchSize),
                        Times.Once());

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(databaseExistingAddresses);

                this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(It.Is(SameAddressesAs(batchNewAddresses)), inputLpiCsvFile),
                    Times.Once);

                this.addressProcessingServiceMock.Verify(service =>
                    service.BulkModifyAddressesAsync(It.Is(SameAddressesAs(batchExisitngAddresses)), inputLpiCsvFile),
                        Times.Once);
            }

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(inputLpiCsvFile, inputBatchSize, numberOfRecords),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Exactly(numberOfBatches));

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

