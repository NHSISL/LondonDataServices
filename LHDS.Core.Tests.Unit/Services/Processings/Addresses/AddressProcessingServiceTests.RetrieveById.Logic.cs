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
        public async Task ShouldRetrieveAddressByIdAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address storageAddress = randomAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByIdAsync(randomAddress.Id))
                    .ReturnsAsync(storageAddress);

            // When
            Address actualAddress =
                await this.addressProcessingService
                    .RetrieveAddressByIdAsync(randomAddress.Id);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByIdAsync(randomAddress.Id),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
