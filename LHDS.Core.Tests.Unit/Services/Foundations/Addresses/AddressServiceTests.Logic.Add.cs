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
        public async Task ShouldAddAddressAsync()
        {
            // given

            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            Address randomAddress = CreateRandomAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Address inputAddress = randomAddress;
            Address storageAddress = inputAddress;
            Address expectedAddress = storageAddress.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAddressAsync(inputAddress))
                    .ReturnsAsync(storageAddress);

            // when
            Address actualAddress = await this.addressService
                .AddAddressAsync(inputAddress);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(inputAddress),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}