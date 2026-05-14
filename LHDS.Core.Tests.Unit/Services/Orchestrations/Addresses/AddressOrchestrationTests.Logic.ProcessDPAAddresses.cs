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
            int numberOfRecords = GetRandomNumber();

            IQueryable<Address> randomExistingAddresses =
                CreateRandomAddresses(numberOfRecords);

            IQueryable<Address> existingAddresses =
                randomExistingAddresses.DeepClone();

            List<Address> randomAddressList =
                CreateRandomAddresses(numberOfRecords).ToList();

            List<Address> newAddresses = randomAddressList.DeepClone();

            HashSet<string> existingUprns =
                existingAddresses.Select(a => a.UPRN).ToHashSet();

            foreach (Address address in newAddresses.Where(
                a => existingUprns.Contains(a.UPRN)))
            {
                address.UPRN = Guid.NewGuid().ToString();
            }

            List<Address> dpaFileAddresses =
                [.. newAddresses, .. existingAddresses.ToList()];

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            addressOrchestrationServiceMock.Setup(service =>
                service.MapDPADataToAddressesAsync(
                    inputDpaCsvFile))
                        .Returns(
                            dpaFileAddresses
                                .ToAsyncEnumerable());

            this.addressProcessingServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(existingAddresses);

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessDPAAddressesAsync(inputDpaCsvFile);

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

            addressOrchestrationServiceMock.Verify(service =>
                service.MapDPADataToAddressesAsync(inputDpaCsvFile),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(
                    It.Is(SameAddressesAs(newAddresses)),
                    inputDpaCsvFile),
                        Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(
                    It.IsAny<List<Address>>(),
                    inputDpaCsvFile),
                        Times.Once);

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

