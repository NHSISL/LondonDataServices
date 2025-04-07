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
        public async Task ShouldAddAddressAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.AddAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // When
            Address actualAddress = await this.addressProcessingService
                .AddAddressAsync(inputAddress);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.addressServiceMock.Verify(service =>
                service.AddAddressAsync(inputAddress),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
