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
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string randomFile = GetRandomString();
            string inputBlpuCsvFile = randomFile;
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            int numberOfRecords = GetRandomNumber();

            IQueryable<Address> randomExistingAddresses =
                CreateRandomAddresses(numberOfRecords);

            IQueryable<Address> existingAddresses =
                randomExistingAddresses.DeepClone();

            List<Address> blpuFileAddresses =
                existingAddresses.DeepClone().ToList();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapBLPUDataToAddressesAsync(
                    inputBlpuCsvFile,
                    default))
                        .Returns(
                            blpuFileAddresses
                                .ToAsyncEnumerable());

            this.addressProcessingServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(existingAddresses);

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessBLPUAddressesAsync(inputBlpuCsvFile);

            // Then
            this.identifierBrokerMock.Verify(broker =>
               broker.GetIdentifierAsync(),
                   Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - BLPU Processing",
                    "Processing BLPU File",
                    $"Starting processing file {inputBlpuCsvFile}.",
                    inputBlpuCsvFile,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file {inputBlpuCsvFile}."),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapBLPUDataToAddressesAsync(
                    inputBlpuCsvFile,
                    default),
                        Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(
                    It.IsAny<List<Address>>(),
                    inputBlpuCsvFile),
                        Times.Once);

            this.auditBrokerMock.Verify(broker =>
               broker.LogInformationAsync(
                   "Address Import - BLPU Processing",
                   "Processing BLPU File",
                   $"Finished processing file {inputBlpuCsvFile}.",
                   inputBlpuCsvFile,
                   inputCorrelationId.ToString()),
                       Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Finished processing file {inputBlpuCsvFile}."),
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

