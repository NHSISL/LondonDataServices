// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAddressByPostCodeAsync()
        {
            //given
            string randomString = GetRandomString();
            string inputPostCode = randomString;
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> expectedAddresses = randomAddresses.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressesByPostCodeAsync(inputPostCode))
                    .ReturnsAsync(expectedAddresses);

            // when
            List<Address> actualAddresses =
                await this.addressProcessingService.RetrieveAddressesByPostCodeAsync(inputPostCode);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressesByPostCodeAsync(inputPostCode),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
