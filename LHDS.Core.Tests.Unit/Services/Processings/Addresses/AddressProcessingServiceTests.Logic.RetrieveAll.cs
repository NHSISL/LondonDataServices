// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAddresses()
        {
            // given
            IQueryable<Address> randomAddresses = CreateRandomAddresses();
            IQueryable<Address> storageAddresses = randomAddresses;
            IQueryable<Address> expectedAddresses = storageAddresses;

            this.addressServiceMock.Setup(broker =>
                broker.RetrieveAllAddresses())
                    .Returns(storageAddresses);

            // when
            IQueryable<Address> actualAddresses =
                this.addressProcessingService.RetrieveAllAddresses();

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressServiceMock.Verify(broker =>
                broker.RetrieveAllAddresses(),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
