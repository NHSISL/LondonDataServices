// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyAddressIfOneExistsAndNotAddAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address storageAddress = randomAddress;
            Address modifiedAddress = storageAddress.DeepClone();
            modifiedAddress.UpdatedDate = DateTimeOffset.UtcNow;
            Address updatedAddress = modifiedAddress.DeepClone();
            Address expectedAddress = updatedAddress;
            List<Address> storageAddresses = new List<Address> { storageAddress };

            this.addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Returns(value: storageAddresses.AsQueryable());

            this.addressServiceMock.Setup(service =>
                service.ModifyAddressAsync(modifiedAddress))
                    .ReturnsAsync(value: updatedAddress);

            // When
            Address actualAddress = await this.addressProcessingService
                .ModifyOrAddAddressAsync(modifiedAddress);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            this.addressServiceMock.Verify(service =>
                service.ModifyAddressAsync(modifiedAddress),
                    Times.Once);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(modifiedAddress),
                    Times.Never);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddAddressIfAddressDoesNotExistsAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress.DeepClone();
            Address expectedAddress = storageAddress;
            List<Address> emptyList = new List<Address>();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Returns(value: emptyList.AsQueryable());

            this.addressServiceMock.Setup(service =>
                service.AddAddressAsync(inputAddress))
                    .ReturnsAsync(value: storageAddress);

            // When
            await this.addressProcessingService.ModifyOrAddAddressAsync(inputAddress);

            // Then
            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(inputAddress),
                    Times.Once);

            this.addressServiceMock.Verify(service =>
                service.ModifyAddressAsync(inputAddress),
                    Times.Never);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
