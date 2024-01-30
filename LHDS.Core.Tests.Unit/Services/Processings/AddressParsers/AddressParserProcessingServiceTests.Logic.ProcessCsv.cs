// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldGetParsedAddressByString()
        {
            // Given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            List<Address> randomAddresses = new List<Address>();
            List<Address> storageAddresses = randomAddresses;
            List<Address> expectedAdresses = storageAddresses.DeepClone();

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(inputAddress))
                    .ReturnsAsync(randomAddresses);

            // When
            List<Address> actualParserAddress = 
                await this.addressParserProcessingService.ProcessCsvAsync(inputAddress);

            // Then
            actualParserAddress.Should().BeEquivalentTo(expectedAdresses);

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(It.IsAny<string>()),
                    Times.Once);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}