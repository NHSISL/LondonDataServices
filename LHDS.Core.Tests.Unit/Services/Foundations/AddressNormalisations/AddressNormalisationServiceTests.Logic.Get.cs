using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Fact]
        public async Task ShouldGetNormalisedAddressAsync()
        {
            // given
            string randomAddress = GetRandomString();
            string inputAddress = randomAddress;
            AddressNormalisation randomAddressNormalisation = CreateRandomAddressNormalisation();
            AddressNormalisation inputAddressNormalisation = randomAddressNormalisation;
            AddressNormalisation storageAddressNormalisation = inputAddressNormalisation;
            AddressNormalisation expectedAddressNormalisation = storageAddressNormalisation.DeepClone();

            var expectedResult = new ValueTask<(string PostalAddress, string JsonPostalAddress)>(
                (storageAddressNormalisation.PostalAddress, storageAddressNormalisation.JsonPostalAddress));

            this.addressNormalisationBrokerMock.Setup(broker =>
                broker.GetNormalisedAddress(randomAddress))
                    .Returns(expectedResult);

            // when
            AddressNormalisation actualAddressNormalisation = 
                await this.addressNormalisationService.GetNormalisedAddress(randomAddress);

            // then
            actualAddressNormalisation.Should().BeEquivalentTo(expectedAddressNormalisation);

            this.addressNormalisationBrokerMock.Verify(broker =>
                broker.GetNormalisedAddress(randomAddress),
                    Times.Once);

            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}