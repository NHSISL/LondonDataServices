// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGetNormalisedAddress()
        {
            // Given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            AddressNormalisation addressNormalisation = new AddressNormalisation
            {
                PostalAddress = GetRandomString(),
                JsonPostalAddress = GetRandomString()
            };

            this.addressNormalisationServiceMock.Setup(service =>
                service.GetNormalisedAddress(inputAddress)).ReturnsAsync(addressNormalisation);

            var expectedNormalisedAddress = addressNormalisation;

            // When
            AddressNormalisation actualNormalisedAddress = 
                await this.addressNormalisationProcessingService.GetNormalisedAddress(inputAddress);

            // Then
            actualNormalisedAddress.Should().BeEquivalentTo(expectedNormalisedAddress);

            this.addressNormalisationServiceMock.Verify(service =>
                service.GetNormalisedAddress(It.IsAny<string>()),
                    Times.Once);

            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}