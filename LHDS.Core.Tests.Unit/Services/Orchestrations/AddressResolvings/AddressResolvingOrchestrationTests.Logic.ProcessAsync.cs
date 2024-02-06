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
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<Address> randomAddresses = CreateRandomAddressList();
            AddressNormalisation randomNormalisedAddress = CreateRandomAddressNormalisation();
            AddressNormalisation inputNormalisedAddress = randomNormalisedAddress;
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            ResolvedAddress updatedResolvedAddress = storageResolvedAddress;
            updatedResolvedAddress.IsProcessed = false;
            updatedResolvedAddress.UpdatedDate = randomDateTimeOffset;
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
            AddressNormalisation actualAddress =
                await this.addressResolvingOrchestrationService.ResolvedAddressAsync(inputNormalisedAddress);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.IsExactMatchForResolvedAddressAsync(inputNormalisedAddress.PostalAddress),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                   Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(
                    It.Is(SameResolvedAddressAs(updatedResolvedAddress))),
                    Times.Once());

            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            serializationBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldResolveAddressesForNotExactMatchAndLogAsync()
        {
            // Given
            List<Address> randomAddresses = CreateRandomAddressList();
            List<Address> storageAddresses = randomAddresses;
            List<KeyValuePair<string, string>> randomAddressComponents = GenerateRandomKeyValuePairAddress();

            HashSet<AddressMatch> addressesToMatch = storageAddresses.Select(address => new AddressMatch
            {
                PostalAddress = ConvertToString(randomAddressComponents),
                JsonPostalAddress = ConvertToJSONString(randomAddressComponents),
                AddressComponents = randomAddressComponents
            }).ToHashSet();

            HashSet<AddressMatch> resolvedMatchedAddresses = addressesToMatch.DeepClone();
            AddressMatch matchedAddress = resolvedMatchedAddresses.First();
            AddressNormalisation randomNormalisedAddress = CreateRandomAddressNormalisation();
            AddressNormalisation inputNormalisedAddress = randomNormalisedAddress;
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            ResolvedAddress updatedResolvedAddress = storageResolvedAddress.DeepClone();
            ResolvedAddress newResolvedAddress = CreateRandomResolvedAddress();
            updatedResolvedAddress.IsProcessed = false;
            string randomPostCode = randomAddresses.First().PostCode;
            string inputPostCode = randomPostCode;
            string storagePostCode = inputPostCode.DeepClone();
            AddressNormalisation expectedAddress = inputNormalisedAddress.DeepClone();
            bool isExactMacth = false;

            ResolvedAddress finalResolvedAddress = new ResolvedAddress
            {
                UPRN = matchedAddress.UPRN,
                UPSN = matchedAddress.UPSN,
                PostCode = storagePostCode,
                PostalAddress = inputNormalisedAddress.PostalAddress,
                JsonPostalAddress = inputNormalisedAddress.JsonPostalAddress,
                MatchAlgorithmUsed = (MatchAlgorithmEnum)Enum.Parse(typeof(MatchAlgorithmEnum), ((int)matchedAddress.BestMatch).ToString()),
                BestMatchType = matchedAddress.BestMatch,
                IsMatched = matchedAddress.IsMatched,
                IsProcessed = false,
                MatchedWithPostalAddress = matchedAddress.PostalAddress,
                MatchedWithJsonPostalAddress = matchedAddress.JsonPostalAddress,
            };

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.IsExactMatchForResolvedAddressAsync(inputNormalisedAddress.PostalAddress))
                    .ReturnsAsync((isExactMacth, inputResolvedAddress.Id));

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(inputNormalisedAddress.PostalAddress))
                    .Returns(storagePostCode);

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressByPostCodeAsync(storagePostCode))
                    .ReturnsAsync(storageAddresses);

            foreach (var address in storageAddresses)
            {
                var components = ConvertStringToKeyValue(address.JsonPostalAddress);

                this.serializationBrokerMock.Setup(broker =>
                    broker.Deserialize<IList<KeyValuePair<string, string>>>(address.JsonPostalAddress))
                        .Returns(components);
            }

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.FindBestMatch(resolvedMatchedAddresses, inputNormalisedAddress.AddressComponents))
                    .ReturnsAsync(matchedAddress);

            // When
            AddressNormalisation actualAddress =
                await this.addressResolvingOrchestrationService.ResolvedAddressAsync(inputNormalisedAddress);

            // Then
            actualAddress.Should().BeEquivalentTo(expectedAddress);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.IsExactMatchForResolvedAddressAsync(inputNormalisedAddress.PostalAddress),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.IsExactMatchForResolvedAddressAsync(inputNormalisedAddress.PostalAddress),
                    Times.Once);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(inputPostCode),
                    Times.Once);

            this.addressProcessingServiceMock.Verify(processing =>
                processing.RetrieveAddressByPostCodeAsync(storagePostCode),
                    Times.Once);

            this.serializationBrokerMock.Verify(broker =>
                broker.Deserialize<IList<KeyValuePair<string, string>>>(inputNormalisedAddress.JsonPostalAddress),
                    Times.Once);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.FindBestMatch(resolvedMatchedAddresses, inputNormalisedAddress.AddressComponents),
                    Times.Once);

            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
            serializationBrokerMock.VerifyNoOtherCalls();
        }
    }
}