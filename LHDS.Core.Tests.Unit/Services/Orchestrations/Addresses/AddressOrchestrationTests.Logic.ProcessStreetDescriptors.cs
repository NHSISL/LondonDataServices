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
        public async Task ShouldProcessStreetDescriptorsAsync()
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
            string inputStreetDescriptorFile = randomFile;
            int inputBatchSize = GetRandomNumber();
            int numberOfBatches = GetRandomNumber();
            int numberOfRecords = inputBatchSize * numberOfBatches;
            List<string> returnedStringList = CreateRandomStringList(numberOfRecords);
            IQueryable<Address> randomAddresses = CreateRandomAddresses(numberOfRecords);
            IQueryable<Address> retrievedAddresses = randomAddresses.DeepClone();
            List<Address> streetDescriptorAddresses = randomAddresses.DeepClone().ToList();

            foreach (Address retrievedAddress in retrievedAddresses)
            {
                retrievedAddress.Thoroughfare = string.Empty;
                retrievedAddress.PostTown = string.Empty;
                retrievedAddress.DependentLocality = string.Empty;
            }

            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchExisitngAddresses = [
                    .. streetDescriptorAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];

                this.fileBrokerMock.Setup(broker =>
                    broker.ReadLinesBatchAsync(
                        inputStreetDescriptorFile,
                        inputBatchSize,
                        i * inputBatchSize))
                            .ReturnsAsync(returnedStringList.GetRange(batchStartLine, inputBatchSize));

                addressOrchestrationServiceMock.Setup(service =>
                    service.MapStreetDescriptorDataToAddressesAsync(
                        inputStreetDescriptorFile,
                        inputBatchSize,
                        i * inputBatchSize))
                            .ReturnsAsync(batchExisitngAddresses);

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(retrievedAddresses);
            }

            this.fileBrokerMock.Setup(broker =>
                broker.ReadLinesBatchAsync(inputStreetDescriptorFile, inputBatchSize, numberOfRecords))
                    .ReturnsAsync([]);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessStreetDescriptorDataAsync(inputStreetDescriptorFile, inputBatchSize);

            // Then
            for (int i = 0; i < numberOfBatches; i++)
            {
                int batchStartLine = i * inputBatchSize;
                int batchEndLine = batchStartLine + inputBatchSize;
                List<Address> batchExisitngAddresses = [
                    .. streetDescriptorAddresses.ToList().GetRange(batchStartLine, inputBatchSize)];

                this.fileBrokerMock.Verify(broker =>
                    broker.ReadLinesBatchAsync(
                        inputStreetDescriptorFile,
                        inputBatchSize,
                        i * inputBatchSize),
                            Times.Once);

                addressOrchestrationServiceMock.Verify(service =>
                    service.MapStreetDescriptorDataToAddressesAsync(
                        inputStreetDescriptorFile,
                        inputBatchSize,
                        i * inputBatchSize),
                            Times.Once());

                this.addressProcessingServiceMock.Setup(service =>
                    service.RetrieveAllAddressesAsync())
                        .ReturnsAsync(retrievedAddresses);

                this.addressProcessingServiceMock.Verify(service =>
                    service.BulkModifyAddressesAsync(
                        It.Is(SameAddressesAs(batchExisitngAddresses)),
                        inputStreetDescriptorFile),
                            Times.Once);
            }

            this.fileBrokerMock.Verify(broker =>
                broker.ReadLinesBatchAsync(inputStreetDescriptorFile, inputBatchSize, numberOfRecords),
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

