// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAddressByIdAsync()
        {
            // given
            Address randomAddress = CreateRandomAddress();
            Address inputAddress = randomAddress;
            Address storageAddress = randomAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(inputAddress.Id))
                    .ReturnsAsync(storageAddress);

            // when
            Address actualAddress =
                await this.addressService.RetrieveAddressByIdAsync(inputAddress.Id);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(inputAddress.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}