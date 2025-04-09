// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyAddressAsync()
        {
            // Given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.ModifyAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // When
            await this.addressProcessingService.ModifyAddressAsync(inputAddress);

            // Then
            this.addressServiceMock.Verify(service =>
                service.ModifyAddressAsync(inputAddress),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
