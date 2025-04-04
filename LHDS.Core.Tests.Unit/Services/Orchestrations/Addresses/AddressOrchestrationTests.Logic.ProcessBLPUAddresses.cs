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
            IQueryable<Address> existingAddresses = CreateRandomAddresses();
            IQueryable<Address> existingAddressesNoPostcodeDb = CreateRandomAddresses();
            IQueryable<Address> newBlpuewAddresses = CreateRandomAddresses();

            List<Address> blpuFileAddresses = [
                .. existingAddresses,
                .. existingAddressesNoPostcodeDb,
                .. newBlpuewAddresses];

            List<Address> databaseAddresses = existingAddresses.DeepClone().ToList();
            List<Address> databaseAddressesNoPostcode = existingAddressesNoPostcodeDb.DeepClone().ToList();

            foreach (Address address in databaseAddressesNoPostcode)
            {
                address.PostCode = string.Empty;
            }

            List<Address> retrievedDatabaseAddresses = [
                ..databaseAddresses,
                .. databaseAddressesNoPostcode];

            List<Address> expectedModifiedAddresses = existingAddressesNoPostcodeDb.DeepClone().ToList();
            List<Address> expectedAddedAddresses = newBlpuewAddresses.DeepClone().ToList();

            addressOrchestrationServiceMock.Setup(service =>
                service.MapBLPUDataToAddressesAsync(inputBlpuCsvFile))
                    .ReturnsAsync(blpuFileAddresses);

            this.addressProcessingServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(retrievedDatabaseAddresses.AsQueryable());

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessBLPUAddressesAsync(inputBlpuCsvFile);

            // Then
            addressOrchestrationServiceMock.Verify(service =>
                service.MapBLPUDataToAddressesAsync(inputBlpuCsvFile),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(It.Is(SameAddressesAs(expectedAddedAddresses)), inputBlpuCsvFile),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(
                    It.Is(SameAddressesAs(expectedModifiedAddresses)),
                    inputBlpuCsvFile),
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

