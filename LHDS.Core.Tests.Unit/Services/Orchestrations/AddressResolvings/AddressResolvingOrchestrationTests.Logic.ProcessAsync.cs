// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressResolvings
{
    public partial class AddressResolvingOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldResolveAddressesForExactMatchAndLogAsync()
        {
            // Given
            List<Address> randomAddresses = CreateRandomAddressList();
            AddressNormalisation randomNormalisedAddress = CreateRandomAddressNormalisation();
            AddressNormalisation inputNormalisedAddress = randomNormalisedAddress;
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            ResolvedAddress updatedResolvedAddress = storageResolvedAddress.DeepClone();
            updatedResolvedAddress.IsProcessed = false;
            string randomPostCode = randomAddresses.First().PostCode;
            AddressNormalisation expectedAddress = inputNormalisedAddress.DeepClone();
            bool isExactMacth = true;

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.IsExactMatchForResolvedAddressAsync(inputNormalisedAddress.PostalAddress))
                    .ReturnsAsync((isExactMacth, inputResolvedAddress.Id));

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id))
                    .ReturnsAsync(storageResolvedAddress);

            // When
            AddressNormalisation actualAddress = await this.addressResolvingOrchestrationService.ResolvedAddressAsync(inputNormalisedAddress);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.IsExactMatchForResolvedAddressAsync(inputNormalisedAddress.PostalAddress),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

