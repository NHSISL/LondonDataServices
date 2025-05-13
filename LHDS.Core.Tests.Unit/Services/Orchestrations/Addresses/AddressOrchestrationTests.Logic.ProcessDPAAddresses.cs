// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldProcessDPAAddressesAsync()
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
            string inputDpaCsvFile = randomFile;
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            int inputBatchSize = GetRandomNumber();
            int numberOfBatches = GetRandomNumber();
            int numberOfRecords = inputBatchSize * numberOfBatches;
            List<string> returnedStringList = CreateRandomStringList(numberOfRecords);
            IQueryable<Address> randomExistingAddresses = CreateRandomAddresses(numberOfRecords);
            IQueryable<Address> existingAddresses = randomExistingAddresses.DeepClone();
            List<Address> randomAddressList = CreateRandomAddresses(numberOfRecords).ToList();
            List<Address> newAddresses = randomAddressList.DeepClone();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchNewAddresses = [.. newAddresses.GetRange(batchStartLine, inputBatchSize)];

                List<Address> batchExisitngAddresses = [
                    .. existingAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];

                List<Address> dpaFileBatchAddresses = [.. batchNewAddresses, .. batchExisitngAddresses];

                this.fileBrokerMock.Setup(broker =>
                    broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, i * inputBatchSize))
                        .ReturnsAsync(returnedStringList.GetRange(batchStartLine, inputBatchSize));

                addressOrchestrationServiceMock.Setup(service =>
                    service.MapDPADataToAddressesAsync(inputDpaCsvFile, inputBatchSize, i * inputBatchSize))
                        .ReturnsAsync(dpaFileBatchAddresses);

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(existingAddresses);
            }

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, numberOfRecords))
                    .ReturnsAsync([]);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessDPAAddressesAsync(inputDpaCsvFile, inputBatchSize);

            // Then
            this.identifierBrokerMock.Verify(broker =>
               broker.GetIdentifierAsync(),
                   Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - DPA Processing",
                    "Processing DPA File",
                    $"Starting processing file {inputDpaCsvFile}.",
                    inputDpaCsvFile,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file {inputDpaCsvFile}."),
                        Times.Once);

            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchNewAddresses = [.. newAddresses.GetRange(batchStartLine, inputBatchSize)];
                List<Address> batchExisitngAddresses = [.. existingAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];
                List<Address> dpaFileBatchAddresses = [.. batchNewAddresses, .. batchExisitngAddresses];

                this.fileBrokerMock.Verify(broker =>
                    broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, i * inputBatchSize),
                        Times.Once);

                addressOrchestrationServiceMock.Verify(service =>
                    service.MapDPADataToAddressesAsync(inputDpaCsvFile, inputBatchSize, i * inputBatchSize),
                        Times.Once());

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(existingAddresses);

                this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(It.Is(SameAddressesAs(batchNewAddresses)), inputDpaCsvFile),
                    Times.Once);

                this.addressProcessingServiceMock.Verify(service =>
                    service.BulkModifyAddressesAsync(It.Is(SameAddressesAs(batchExisitngAddresses)), inputDpaCsvFile),
                        Times.Once);

                this.auditBrokerMock.Verify(broker =>
                    broker.LogInformationAsync(
                        "Address Import - DPA Processing",
                        "Processing DPA File",
                        
                        $"Processing DPA File - Processing lines {batchStartLine} to " +
                            $"{batchEndLine}. Correlation Id: {inputCorrelationId}.",
                        
                        inputDpaCsvFile,
                        inputCorrelationId.ToString()),
                            Times.Once);

                this.loggingBrokerMock.Verify(broker =>
                    broker.LogInformationAsync(
                        $"Processing DPA File - Processing lines {batchStartLine} to " +
                            $"{batchEndLine}. Correlation Id: {inputCorrelationId}."),
                            Times.Once);
            }

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(inputDpaCsvFile, inputBatchSize, numberOfRecords),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Exactly(numberOfBatches));

            this.auditBrokerMock.Verify(broker =>
               broker.LogInformationAsync(
                   "Address Import - DPA Processing",
                   "Processing DPA File",
                   $"Finished processing file {inputDpaCsvFile}.",
                   inputDpaCsvFile,
                   inputCorrelationId.ToString()),
                       Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Finished processing file {inputDpaCsvFile}."),
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

