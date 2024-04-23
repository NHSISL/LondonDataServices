// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldRemoveAddressByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAddressId = randomId;
            Address randomAddress = CreateRandomAddress();
            Address storageAddress = randomAddress;
            Address expectedInputAddress = storageAddress;
            Address deletedAddress = expectedInputAddress;
            Address expectedAddress = deletedAddress.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(inputAddressId))
                    .ReturnsAsync(storageAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAddressAsync(expectedInputAddress))
                    .ReturnsAsync(deletedAddress);

            // when
            Address actualAddress = await this.addressService
                .RemoveAddressByIdAsync(inputAddressId);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(inputAddressId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressAsync(expectedInputAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}