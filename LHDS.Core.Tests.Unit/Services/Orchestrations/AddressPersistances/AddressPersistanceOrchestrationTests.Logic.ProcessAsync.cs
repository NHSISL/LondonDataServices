// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationTests
    {
        [Fact]
        public async Task ShouldProcessAddressesAndLogAsync()
        {
            // Given
            List<Address> randomAddresses = CreateRandomAddresses().ToList();

            foreach(Address address in randomAddresses)
            {
                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    JsonPostalAddress = address.JsonPostalAddress,
                    PostalAddress = address.PostalAddress,
                };

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(randomStringAddress))
                        .ReturnsAsync(addressNormalisation);

                this.addressProcessingServiceMock.Setup(service =>
                    service.ModifyOrAddAddressAsync(inputAddress))
                        .ReturnsAsync(storageAddress);

            }


            // Where
            await this.addressPersistanceOrchestrationService.ProcessAsync(inputAddress);

            // Then
            this.addressNormalisationProcessingServiceMock.Verify(service =>
                service.GetNormalisedAddress(randomStringAddress),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
                service.ModifyOrAddAddressAsync(inputAddress),
                        Times.Once);

            this.addressLoadingAuditProcessingServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(It.IsAny<AddressLoadingAudit>()), 
                    Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.addressLoadingAuditProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

