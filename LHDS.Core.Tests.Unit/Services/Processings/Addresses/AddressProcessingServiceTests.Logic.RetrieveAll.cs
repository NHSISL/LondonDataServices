// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAddresses()
        {
            // given
            IQueryable<Address> randomAddresses = CreateRandomAddresses();
            IQueryable<Address> storageAddresses = randomAddresses;
            IQueryable<Address> expectedAddresses = storageAddresses;

            this.addressServiceMock.Setup(broker =>
                broker.RetrieveAllAddressesAsync())
                    .ReturnsAsync(storageAddresses);

            // when
            IQueryable<Address> actualAddresses =
                await this.addressProcessingService.RetrieveAllAddressesAsync();

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressServiceMock.Verify(broker =>
                broker.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}