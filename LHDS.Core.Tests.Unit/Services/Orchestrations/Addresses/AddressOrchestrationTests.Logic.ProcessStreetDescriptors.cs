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
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string randomFile = GetRandomString();
            string inputStreetDescriptorFile = randomFile;
            IQueryable<Address> randomAddresses = CreateRandomAddresses();
            IQueryable<Address> retrievedAddresses = randomAddresses.DeepClone();
            List<Address> streetDescriptorAddresses = randomAddresses.DeepClone().ToList();

            foreach (Address retrievedAddress in retrievedAddresses)
            {
                retrievedAddress.Thoroughfare = string.Empty;
                retrievedAddress.PostTown = string.Empty;
                retrievedAddress.DependentLocality = string.Empty;
            }

            List<Address> expectedModifiedAddresses = streetDescriptorAddresses;

            addressOrchestrationServiceMock.Setup(service =>
                service.MapStreetDescriptorDataToAddressesAsync(inputStreetDescriptorFile))
                    .ReturnsAsync(streetDescriptorAddresses);

            this.addressProcessingServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(retrievedAddresses);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessStreetDescriptorDataAsync(inputStreetDescriptorFile);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapStreetDescriptorDataToAddressesAsync(inputStreetDescriptorFile),
                    Times.Once());

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(
                    It.Is(SameAddressesAs(expectedModifiedAddresses)),
                    inputStreetDescriptorFile),
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

