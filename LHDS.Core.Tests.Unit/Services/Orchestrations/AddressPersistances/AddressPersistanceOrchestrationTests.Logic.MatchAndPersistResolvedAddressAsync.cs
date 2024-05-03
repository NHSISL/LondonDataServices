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
        [Theory]
        [InlineData(BestMatchEnum.Single)]
        [InlineData(BestMatchEnum.Multiple)]
        public async Task ShouldMatchAndPersistResolvedAddressMatchAndLogAsync(BestMatchEnum bestMatch)
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

            HashSet<AddressMatch> addressesToMatch = storageAddresses.Select(address => new AddressMatch
            {
                PostalAddress = address.PostalAddress,
                JsonPostalAddress = address.JsonPostalAddress,
                AddressComponents = GenerateRandomKeyValuePairAddressFromJson(address.JsonPostalAddress)
            }).ToHashSet();

            HashSet<AddressMatch> resolvedMatchedAddresses = addressesToMatch.DeepClone();
            AddressMatch matchedAddress = resolvedMatchedAddresses.First();
            matchedAddress.IsMatched = true;
            matchedAddress.BestMatch = bestMatch;

            ResolvedAddress updatedResolvedAddress = 
                UpdateResolvedAddress(inputResolvedAddress, matchedAddress);

            updatedResolvedAddress.UpdatedDate = randomDateTimeOffset;

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(inputResolvedAddress.PostalAddress))
                    .Returns(postCode);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressesByPostCodeAsync(postCode))
                    .ReturnsAsync(storageAddresses); 

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.FindBestMatch(
                    It.Is(SameAddressToMatchAs(addressesToMatch)), 
                    It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))))
                        .ReturnsAsync(matchedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTimeOffset())
                   .Returns(randomDateTimeOffset);

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(updatedResolvedAddress))
                    .ReturnsAsync(updatedResolvedAddress);

            // When
            ResolvedAddress actualResolvedAddress =
                await this.addressPersistanceOrchestrationService
                    .MatchAndPersistResolvedAddressAsync(inputResolvedAddress);

            // Then
            actualResolvedAddress.Should().BeEquivalentTo(updatedResolvedAddress);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(inputResolvedAddress.PostalAddress),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(processing =>
                processing.RetrieveAddressesByPostCodeAsync(postCode),
                    Times.Once);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.FindBestMatch(
                    It.Is(SameAddressToMatchAs(addressesToMatch)),
                    It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(updatedResolvedAddress),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformation(
                    "Resolved Address",
                    "Successfully resolved and address to the database",
                    $"Successfully persisted address with id: " +
                        $"{updatedResolvedAddress.Id} with a {updatedResolvedAddress.MatchAlgorithmEnum} match",
                    updatedResolvedAddress.MatchAlgorithmEnum.ToString(),
                    updatedResolvedAddress.Id),
                        Times.Once());

            this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldMatchAndPersistResolvedAddressNoMatchAndLogAsync()
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

            HashSet<AddressMatch> addressesToMatch = storageAddresses.Select(address => new AddressMatch
            {
                PostalAddress = address.PostalAddress,
                JsonPostalAddress = address.JsonPostalAddress,
                AddressComponents = GenerateRandomKeyValuePairAddressFromJson(address.JsonPostalAddress)
            }).ToHashSet();

            HashSet<AddressMatch> resolvedMatchedAddresses = addressesToMatch.DeepClone();
            
            AddressMatch matchedAddress = new AddressMatch();
            matchedAddress.IsMatched = true;
            matchedAddress.BestMatch = BestMatchEnum.None;

            ResolvedAddress updatedResolvedAddress =
                UpdateResolvedAddress(inputResolvedAddress, matchedAddress);

            updatedResolvedAddress.UpdatedDate = randomDateTimeOffset;

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(inputResolvedAddress.PostalAddress))
                    .Returns(postCode);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressesByPostCodeAsync(postCode))
                    .ReturnsAsync(storageAddresses);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.FindBestMatch(
                    It.Is(SameAddressToMatchAs(addressesToMatch)),
                    It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))))
                        .ReturnsAsync(matchedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTimeOffset())
                   .Returns(randomDateTimeOffset);

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(updatedResolvedAddress))
                    .ReturnsAsync(updatedResolvedAddress);

            // When
            ResolvedAddress actualResolvedAddress =
                await this.addressPersistanceOrchestrationService
                    .MatchAndPersistResolvedAddressAsync(inputResolvedAddress);

            // Then
            actualResolvedAddress.Should().BeEquivalentTo(updatedResolvedAddress);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(inputResolvedAddress.PostalAddress),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(processing =>
                processing.RetrieveAddressesByPostCodeAsync(postCode),
                    Times.Once);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.FindBestMatch(
                    It.Is(SameAddressToMatchAs(addressesToMatch)),
                    It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(updatedResolvedAddress),
                    Times.Once);

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformation(
                    "Resolved Address",
                    "Successfully resolved and address to the database",
                    $"Successfully persisted address with id: " +
                        $"{updatedResolvedAddress.Id} with a {updatedResolvedAddress.MatchAlgorithmEnum} match",
                    updatedResolvedAddress.MatchAlgorithmEnum.ToString(),
                    updatedResolvedAddress.Id),
                        Times.Once());

            this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

