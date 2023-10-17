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
        public async Task ShouldAddAddressAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Address randomAddress = CreateRandomAddress(randomDateTimeOffset);
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // when
            Address actualAddress = await this.addressService
                .AddAddressAsync(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(inputAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}