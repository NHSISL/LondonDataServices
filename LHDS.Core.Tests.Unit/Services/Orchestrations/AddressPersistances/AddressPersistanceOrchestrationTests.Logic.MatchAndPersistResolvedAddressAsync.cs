// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMatchAndPersistResolvedAddressExactMatchAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;

            List<KeyValuePair<string, string>> randomResolvedAddressComponents =
                GenerateRandomKeyValuePairAddressFromJson(randomResolvedAddress.JsonPostalAddress);

            List<Address> randomAddresses = CreateRandomAddressList(GetRandomNumber());
            List<Address> storageAddresses = randomAddresses;
            string postCode = GetRandomString();

            List<KeyValuePair<string, string>> randomAddressComponents = GenerateRandomKeyValuePairAddress();

            string jsonAddress = ConvertToJSONString(randomAddressComponents);

            HashSet<AddressMatch> addressesToMatch = storageAddresses.Select(address => new AddressMatch
            {
                PostalAddress = ConvertToString(randomAddressComponents),
                JsonPostalAddress = jsonAddress,
                AddressComponents = GenerateRandomKeyValuePairAddressFromJson(jsonAddress)
            }).ToHashSet();

            HashSet<AddressMatch> resolvedMatchedAddresses = addressesToMatch.DeepClone();
            AddressMatch matchedAddress = resolvedMatchedAddresses.First();
            matchedAddress.IsMatched = true;
            matchedAddress.BestMatch = BestMatchEnum.Single;

            ResolvedAddress updatedResolvedAddress = UpdateResolvedAddress(inputResolvedAddress, matchedAddress);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(inputResolvedAddress.PostalAddress))
                    .Returns(postCode);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressByPostCodeAsync(inputResolvedAddress.PostCode))
                    .ReturnsAsync(storageAddresses); 

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.FindBestMatch(addressesToMatch, randomResolvedAddressComponents))
                    .ReturnsAsync(matchedAddress);

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(updatedResolvedAddress))
                    .ReturnsAsync(updatedResolvedAddress);

            // When
            ResolvedAddress actualResolvedAddress =
                await this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(inputResolvedAddress);

            // Then
            actualResolvedAddress.Should().BeEquivalentTo(updatedResolvedAddress);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(postCode),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(processing =>
                processing.RetrieveAddressByPostCodeAsync(inputResolvedAddress.PostCode),
                    Times.Once);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.FindBestMatch(addressesToMatch, randomResolvedAddressComponents),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(updatedResolvedAddress),
                    Times.Once);

            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            addressProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

