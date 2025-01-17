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

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAddressByUprnAsync()
        {
            // given
            string inputUPRN = "testUPRN";
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            Address expectedAddress = randomAddresses.First();
            expectedAddress.UPRN = inputUPRN;

            this.addressServiceMock.Setup(service =>
                service.RetrieveAllAddressesAsync())
                    .ReturnsAsync(randomAddresses.AsQueryable());

            // when
            Address actualAddress =
                await this.addressProcessingService.RetrieveAddressByUPRNAsync(inputUPRN);

            // then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddressesAsync(),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
