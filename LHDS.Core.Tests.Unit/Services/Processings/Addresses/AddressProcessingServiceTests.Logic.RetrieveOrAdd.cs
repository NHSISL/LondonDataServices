// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveAddressIfOneExistsAndNotAddAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id))
                    .ReturnsAsync(value: storageAddress);

            // When
            Address actualAddress = await this.addressProcessingService
                .RetrieveOrAddAddressAsync(inputAddress);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id),
                    Times.Once);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(inputAddress),
                    Times.Never);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddAddressIfOneDoesNotExistAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id))
                    .ReturnsAsync(value: null);

            this.addressServiceMock.Setup(service =>
                service.AddAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // When
            await this.addressProcessingService.RetrieveOrAddAddressAsync(inputAddress);

            // Then
            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(inputAddress.Id),
                    Times.Once);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(inputAddress),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
