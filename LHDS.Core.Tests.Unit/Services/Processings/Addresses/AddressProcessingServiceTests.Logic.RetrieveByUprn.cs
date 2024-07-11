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
        public async Task ShouldRetrieveAddressByUprnAsync()
        {
            //given
            string randomString = GetRandomString();
            string inputUPRN = randomString;
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> expectedAddresses = randomAddresses.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAllAddresses())
                    .Returns(expectedAddresses.AsQueryable());

            // when
            Address actualAddresses =
                await this.addressProcessingService.RetrieveAddressByUPRNAsync(inputUPRN);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAllAddresses(),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
