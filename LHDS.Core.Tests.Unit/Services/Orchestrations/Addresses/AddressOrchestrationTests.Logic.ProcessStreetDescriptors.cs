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
            Guid randomGuid = Guid.NewGuid();
            Guid inputCorrelationId = randomGuid;
            int numberOfRecords = GetRandomNumber();

            IQueryable<Address> randomAddresses =
                CreateRandomAddresses(numberOfRecords);

            IQueryable<Address> retrievedAddresses =
                randomAddresses.DeepClone();

            List<Address> streetDescriptorAddresses =
                randomAddresses.DeepClone().ToList();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(inputCorrelationId);

            foreach (Address retrievedAddress in retrievedAddresses)
            {
                retrievedAddress.Thoroughfare = string.Empty;
                retrievedAddress.PostTown = string.Empty;
                retrievedAddress.DependentLocality = string.Empty;
            }

            addressOrchestrationServiceMock.Setup(service =>
                service.MapStreetDescriptorDataToAddressesAsync(
                    inputStreetDescriptorFile,
                    default))
                        .Returns(
                            streetDescriptorAddresses
                                .ToAsyncEnumerable());

            this.addressProcessingServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(retrievedAddresses);

            AddressOrchestrationService service =
                addressOrchestrationServiceMock.Object;

            // When
            await service.ProcessStreetDescriptorDataAsync(
                inputStreetDescriptorFile);

            // Then
            this.identifierBrokerMock.Verify(broker =>
               broker.GetIdentifierAsync(),
                   Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    "Address Import - Street Descriptors Processing",
                    "Processing Street Descriptors File",
                    $"Starting processing file {inputStreetDescriptorFile}.",
                    inputStreetDescriptorFile,
                    inputCorrelationId.ToString()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Starting processing file " +
                        $"{inputStreetDescriptorFile}."),
                        Times.Once);

            addressOrchestrationServiceMock.Verify(service =>
                service.MapStreetDescriptorDataToAddressesAsync(
                    inputStreetDescriptorFile,
                    default),
                        Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.BulkModifyAddressesAsync(
                    It.IsAny<List<Address>>(),
                    inputStreetDescriptorFile),
                        Times.Once);

            this.auditBrokerMock.Verify(broker =>
               broker.LogInformationAsync(
                   "Address Import - Street Descriptors Processing",
                   "Processing Street Descriptors File",
                   $"Finished processing file " +
                       $"{inputStreetDescriptorFile}.",
                   inputStreetDescriptorFile,
                   inputCorrelationId.ToString()),
                       Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(
                    $"Finished processing file " +
                        $"{inputStreetDescriptorFile}."),
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

