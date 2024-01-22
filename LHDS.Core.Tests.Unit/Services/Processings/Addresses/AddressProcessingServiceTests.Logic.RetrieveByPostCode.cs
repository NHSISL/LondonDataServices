// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            string randomString = GetRandomString();
            string inputPostCode = randomString;

            foreach (var address in randomAddresses)
            {
                address.PostCode = inputPostCode;
            }

            List<Address> expectedAddresses = randomAddresses.DeepClone();

            this.addressServiceMock.Setup(service =>
                service.RetrieveAddressByPostCodeAsync(inputPostCode))
                    .ReturnsAsync(expectedAddresses);

            // when
            List<Address> actualAddresses =
                await this.addressProcessingService.RetrieveAddressByPostCodeAsync(inputPostCode);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressServiceMock.Verify(service =>
                service.RetrieveAddressByPostCodeAsync(inputPostCode),
                    Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
