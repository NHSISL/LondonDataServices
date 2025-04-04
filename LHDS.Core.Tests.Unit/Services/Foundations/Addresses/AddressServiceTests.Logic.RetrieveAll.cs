// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldReturnAddresses()
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses();
            List<Address> storageAddresses = randomAddresses;
            List<Address> expectedAddresses = storageAddresses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressesAsync())
                    .ReturnsAsync(storageAddresses.AsQueryable());

            // when
            IQueryable<Address> actualAddresses =
                await this.addressService.RetrieveAllAddressesAsync();

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressesAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}