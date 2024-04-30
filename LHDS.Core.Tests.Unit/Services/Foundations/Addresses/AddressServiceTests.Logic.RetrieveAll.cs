// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public void ShouldReturnAddresses()
        {
            // given
            IQueryable<Address> randomAddresses = CreateRandomAddresses();
            IQueryable<Address> storageAddresses = randomAddresses;
            IQueryable<Address> expectedAddresses = storageAddresses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddresses())
                    .Returns(storageAddresses);

            // when
            IQueryable<Address> actualAddresses =
                this.addressService.RetrieveAllAddresses();

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddresses(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}