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
        public async Task ShouldProcessBLPUAddressesAsync()
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
            string inputBlpuCsvFile = randomFile;
            int inputBatchSize = GetRandomNumber();
            int numberOfBatches = GetRandomNumber();
            int numberOfRecords = inputBatchSize * numberOfBatches;
            List<string> returnedStringList = CreateRandomStringList(numberOfRecords);
            IQueryable<Address> randomExistingAddresses = CreateRandomAddresses(numberOfRecords);
            IQueryable<Address> existingAddresses = randomExistingAddresses.DeepClone();
            IQueryable<Address> existingAddressesNoPostcodeDb = CreateRandomAddresses(numberOfRecords);
            List<Address> randomAddresses = CreateRandomAddresses(numberOfRecords).ToList();
            List<Address> newBlpuAddresses = randomAddresses;

            foreach (Address address in existingAddressesNoPostcodeDb)
            {
                address.PostCode = string.Empty;
            }

            IQueryable<Address> databaseAddresses = existingAddresses.DeepClone().Concat(existingAddressesNoPostcodeDb.DeepClone());
            List<Address> databaseAddressesNoPostcode = existingAddressesNoPostcodeDb.DeepClone().ToList();

            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchNewAddresses = [.. newBlpuAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];
                List<Address> batchExisitngAddresses = [.. databaseAddressesNoPostcode.GetRange(batchStartLine, inputBatchSize)];
                List<Address> dpaFileBatchAddresses = [.. batchNewAddresses, .. batchExisitngAddresses];

                this.fileBrokerMock.Setup(broker =>
                    broker.ReadLinesBatchAsync(inputBlpuCsvFile, inputBatchSize, i * inputBatchSize))
                        .ReturnsAsync(returnedStringList.GetRange(batchStartLine, inputBatchSize));

                addressOrchestrationServiceMock.Setup(service =>
                    service.MapBLPUDataToAddressesAsync(inputBlpuCsvFile, inputBatchSize, i * inputBatchSize))
                        .ReturnsAsync(dpaFileBatchAddresses);

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(databaseAddresses);
            }

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(inputBlpuCsvFile, inputBatchSize, numberOfRecords))
                    .ReturnsAsync([]);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessBLPUAddressesAsync(inputBlpuCsvFile, inputBatchSize);

            // Then
            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchNewAddresses = [.. newBlpuAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];
                List<Address> batchExisitngAddresses = [.. databaseAddressesNoPostcode.GetRange(batchStartLine, inputBatchSize)];
                List<Address> dpaFileBatchAddresses = [.. batchNewAddresses, .. batchExisitngAddresses];


                this.fileBrokerMock.Verify(broker =>
                    broker.ReadLinesBatchAsync(inputBlpuCsvFile, inputBatchSize, i * inputBatchSize),
                        Times.Once);

                addressOrchestrationServiceMock.Verify(service =>
                    service.MapBLPUDataToAddressesAsync(inputBlpuCsvFile, inputBatchSize, i * inputBatchSize),
                        Times.Once());

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(existingAddresses);

                this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(It.Is(SameAddressesAs(batchNewAddresses)), inputBlpuCsvFile),
                    Times.Once);

                this.addressProcessingServiceMock.Verify(service =>
                    service.BulkModifyAddressesAsync(It.Is(SameAddressesAs(batchExisitngAddresses)), inputBlpuCsvFile),
                        Times.Once);
            }

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(inputBlpuCsvFile, inputBatchSize, numberOfRecords),
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

