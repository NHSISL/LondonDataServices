using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldModifyAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset);
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress.DeepClone();
            storageAddress.UpdatedDate = randomAddress.CreatedDate;
            Address updatedAddress = inputAddress;
            Address expectedAddress = updatedAddress.DeepClone();
            Guid addressId = inputAddress.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressAsync(inputAddress))
                    .ReturnsAsync(updatedAddress);

            // when
            Address actualAddress =
                await this.addressService.ModifyAddressAsync(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(inputAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}