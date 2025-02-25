// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
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
            EntraUser randomEntraUser = CreateRandomEntraUser();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress.DeepClone();
            storageAddress.UpdatedDate = randomAddress.CreatedDate;
            Address updatedAddress = inputAddress;
            Address expectedAddress = updatedAddress.DeepClone();
            Guid addressId = inputAddress.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(addressId))
                    .ReturnsAsync(storageAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressAsync(inputAddress))
                    .ReturnsAsync(updatedAddress);

            // when
            Address actualAddress =
                await this.addressService.ModifyAddressAsync(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(brokers =>
                brokers.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(inputAddress.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(inputAddress),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}