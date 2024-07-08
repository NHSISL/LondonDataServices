// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressMatchers;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory(Skip = "No longer used, will refactor out")]
        [InlineData(BestMatchEnum.Single)]
        [InlineData(BestMatchEnum.Multiple)]
        public async Task ShouldMatchAndPersistResolvedAddressMatchAndLogAsync(BestMatchEnum bestMatch)
        {
            //// Given
            //DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            //string postCode = GetRandomString();
            //ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            //randomResolvedAddress.PostCode = postCode;
            //ResolvedAddress inputResolvedAddress = randomResolvedAddress;

            //List<KeyValuePair<string, string>> randomResolvedAddressComponents =
            //    GenerateKeyValuePairAddressFromJson(randomResolvedAddress.JsonPostalAddress);

            //List<Address> randomAddresses = CreateRandomAddressList(GetRandomNumber());
            //List<Address> storageAddresses = randomAddresses;
            //List<KeyValuePair<string, string>> randomAddressComponents = GenerateRandomKeyValuePairAddress();

            //HashSet<AddressMatch> addressesToMatch = storageAddresses.Select(address => new AddressMatch
            //{
            //    PostalAddress = address.PostalAddress,
            //    JsonPostalAddress = address.JsonPostalAddress,
            //    NormalisedAddressComponents = GenerateKeyValuePairAddressFromJson(address.JsonPostalAddress),
            //    OriginalAddressComponents = GenerateKeyValuePairRepresentingAddress(address)
            //}).ToHashSet();

            //HashSet<AddressMatch> resolvedMatchedAddresses = addressesToMatch.DeepClone();
            //AddressMatch matchedAddress = resolvedMatchedAddresses.First();
            //matchedAddress.IsMatched = true;
            //matchedAddress.BestMatch = bestMatch;

            //ResolvedAddress updatedResolvedAddress =
            //    UpdateResolvedAddress(inputResolvedAddress, matchedAddress);

            //Address ordinanceAddress = storageAddresses.First();
            //updatedResolvedAddress.MatchedPostalAddress = ordinanceAddress.PostalAddress;
            //updatedResolvedAddress.MatchedJsonPostalAddress = ordinanceAddress.JsonPostalAddress;
            //updatedResolvedAddress.MatchedUPRN = ordinanceAddress.UPRN;
            //updatedResolvedAddress.MatchedUPSN = ordinanceAddress.UPSN;
            //updatedResolvedAddress.MatchedOrganisationName = ordinanceAddress.OrganisationName;
            //updatedResolvedAddress.MatchedDepartmentName = ordinanceAddress.DepartmentName;
            //updatedResolvedAddress.MatchedSubBuildingName = ordinanceAddress.SubBuildingName;
            //updatedResolvedAddress.MatchedBuildingName = ordinanceAddress.BuildingName;
            //updatedResolvedAddress.MatchedBuildingNumber = ordinanceAddress.BuildingNumber;
            //updatedResolvedAddress.MatchedDependentThoroughfare = ordinanceAddress.DependentThoroughfare;
            //updatedResolvedAddress.MatchedThoroughfare = ordinanceAddress.Thoroughfare;
            //updatedResolvedAddress.MatchedDoubleDependentLocality = ordinanceAddress.DoubleDependentLocality;
            //updatedResolvedAddress.MatchedDependentLocality = ordinanceAddress.DependentLocality;
            //updatedResolvedAddress.MatchedPostTown = ordinanceAddress.PostTown;
            //updatedResolvedAddress.MatchedPostCode = ordinanceAddress.PostCode;
            //updatedResolvedAddress.UpdatedDate = randomDateTimeOffset;
            //updatedResolvedAddress.CreatedDate = randomDateTimeOffset;
            //updatedResolvedAddress.CreatedBy = "System";
            //updatedResolvedAddress.UpdatedBy = "System";

            //this.addressMatcherProcessingServiceMock.Setup(processing =>
            //    processing.ExtractPostCode(inputResolvedAddress.PostalAddress))
            //        .Returns(postCode);

            //this.addressProcessingServiceMock.Setup(processing =>
            //    processing.RetrieveAddressesByPostCodeAsync(randomResolvedAddress.PostCode))
            //        .ReturnsAsync(storageAddresses);

            //this.addressMatcherProcessingServiceMock.Setup(processing =>
            //    processing.FindBestMatch(
            //        It.Is(SameAddressToMatchAs(addressesToMatch)),
            //        It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))))
            //            .ReturnsAsync(matchedAddress);

            //this.dateTimeBrokerMock.Setup(broker =>
            //   broker.GetCurrentDateTimeOffset())
            //       .Returns(randomDateTimeOffset);

            //this.resolvedAddressProcessingServiceMock.Setup(processing =>
            //    processing.ModifyOrAddResolvedAddressAsync(updatedResolvedAddress))
            //        .ReturnsAsync(updatedResolvedAddress);

            //// When
            //ResolvedAddress actualResolvedAddress =
            //    await this.addressPersistanceOrchestrationService
            //        .MatchAndPersistResolvedAddressAsync(inputResolvedAddress);

            //// Then
            //actualResolvedAddress.Should().BeEquivalentTo(updatedResolvedAddress);

            //this.addressMatcherProcessingServiceMock.Verify(processing =>
            //    processing.ExtractPostCode(inputResolvedAddress.PostalAddress),
            //        Times.Once);

            //this.addressProcessingServiceMock.Verify(processing =>
            //    processing.RetrieveAddressesByPostCodeAsync(randomResolvedAddress.PostCode),
            //        Times.Once);

            //this.addressMatcherProcessingServiceMock.Verify(processing =>
            //    processing.FindBestMatch(
            //        It.Is(SameAddressToMatchAs(addressesToMatch)),
            //        It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))),
            //        Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Once);

            //this.resolvedAddressProcessingServiceMock.Verify(processing =>
            //    processing.ModifyOrAddResolvedAddressAsync(updatedResolvedAddress),
            //        Times.Once);

            //this.auditBrokerMock.Verify(broker =>
            //    broker.LogInformation(
            //        "Resolved Address",
            //        "Successfully resolved and address to the database",
            //        $"Successfully persisted address with id: " +
            //            $"{updatedResolvedAddress.Id} with a {updatedResolvedAddress.MatchAlgorithmEnum} match",
            //        updatedResolvedAddress.MatchAlgorithmEnum.ToString(),
            //        updatedResolvedAddress.Id),
            //            Times.Once());

            //this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            //this.addressProcessingServiceMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "No longer used, will refactor out")]
        public async Task ShouldMatchAndPersistResolvedAddressNoMatchAndLogAsync()
        {
            //// Given
            //DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            //string postCode = GetRandomString();
            //ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            //randomResolvedAddress.PostCode = postCode;
            //ResolvedAddress inputResolvedAddress = randomResolvedAddress;

            //List<KeyValuePair<string, string>> randomResolvedAddressComponents =
            //    GenerateKeyValuePairAddressFromJson(randomResolvedAddress.JsonPostalAddress);

            //List<Address> randomAddresses = CreateRandomAddressList(GetRandomNumber());
            //List<Address> storageAddresses = randomAddresses;

            //List<KeyValuePair<string, string>> randomAddressComponents = GenerateRandomKeyValuePairAddress();

            //HashSet<AddressMatch> addressesToMatch = storageAddresses.Select(address => new AddressMatch
            //{
            //    PostalAddress = address.PostalAddress,
            //    JsonPostalAddress = address.JsonPostalAddress,
            //    NormalisedAddressComponents = GenerateKeyValuePairAddressFromJson(address.JsonPostalAddress),
            //    OriginalAddressComponents = GenerateKeyValuePairRepresentingAddress(address)
            //}).ToHashSet();

            //HashSet<AddressMatch> resolvedMatchedAddresses = addressesToMatch.DeepClone();

            //AddressMatch matchedAddress = new AddressMatch();
            //matchedAddress.IsMatched = true;
            //matchedAddress.BestMatch = BestMatchEnum.None;

            //ResolvedAddress updatedResolvedAddress =
            //    UpdateResolvedAddress(inputResolvedAddress, matchedAddress);

            //updatedResolvedAddress.MatchedPostalAddress = string.Empty;
            //updatedResolvedAddress.MatchedJsonPostalAddress = string.Empty;
            //updatedResolvedAddress.MatchedUPRN = string.Empty;
            //updatedResolvedAddress.MatchedUPSN = string.Empty;
            //updatedResolvedAddress.MatchedOrganisationName = string.Empty;
            //updatedResolvedAddress.MatchedDepartmentName = string.Empty;
            //updatedResolvedAddress.MatchedSubBuildingName = string.Empty;
            //updatedResolvedAddress.MatchedBuildingName = string.Empty;
            //updatedResolvedAddress.MatchedBuildingNumber = string.Empty;
            //updatedResolvedAddress.MatchedDependentThoroughfare = string.Empty;
            //updatedResolvedAddress.MatchedThoroughfare = string.Empty;
            //updatedResolvedAddress.MatchedDoubleDependentLocality = string.Empty;
            //updatedResolvedAddress.MatchedDependentLocality = string.Empty;
            //updatedResolvedAddress.MatchedPostTown = string.Empty;
            //updatedResolvedAddress.MatchedPostCode = string.Empty;
            //updatedResolvedAddress.UpdatedDate = randomDateTimeOffset;
            //updatedResolvedAddress.CreatedDate = randomDateTimeOffset;
            //updatedResolvedAddress.CreatedBy = "System";
            //updatedResolvedAddress.UpdatedBy = "System";

            //this.addressMatcherProcessingServiceMock.Setup(processing =>
            //    processing.ExtractPostCode(inputResolvedAddress.PostalAddress))
            //        .Returns(postCode);

            //this.addressProcessingServiceMock.Setup(processing =>
            //    processing.RetrieveAddressesByPostCodeAsync(inputResolvedAddress.PostCode))
            //        .ReturnsAsync(storageAddresses);

            //this.addressMatcherProcessingServiceMock.Setup(processing =>
            //    processing.FindBestMatch(
            //        It.Is(SameAddressToMatchAs(addressesToMatch)),
            //        It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))))
            //            .ReturnsAsync(matchedAddress);

            //this.dateTimeBrokerMock.Setup(broker =>
            //   broker.GetCurrentDateTimeOffset())
            //       .Returns(randomDateTimeOffset);

            //this.resolvedAddressProcessingServiceMock.Setup(processing =>
            //    processing.ModifyOrAddResolvedAddressAsync(updatedResolvedAddress))
            //        .ReturnsAsync(updatedResolvedAddress);

            //// When
            //ResolvedAddress actualResolvedAddress =
            //    await this.addressPersistanceOrchestrationService
            //        .MatchAndPersistResolvedAddressAsync(inputResolvedAddress);

            //// Then
            //actualResolvedAddress.Should().BeEquivalentTo(updatedResolvedAddress);

            //this.addressMatcherProcessingServiceMock.Verify(processing =>
            //    processing.ExtractPostCode(inputResolvedAddress.PostalAddress),
            //        Times.Once);

            //this.addressProcessingServiceMock.Verify(processing =>
            //    processing.RetrieveAddressesByPostCodeAsync(inputResolvedAddress.PostCode),
            //        Times.Once);

            //this.addressMatcherProcessingServiceMock.Verify(processing =>
            //    processing.FindBestMatch(
            //        It.Is(SameAddressToMatchAs(addressesToMatch)),
            //        It.Is(SameResolvedAddressAs(randomResolvedAddressComponents))),
            //        Times.Once);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Once);

            //this.resolvedAddressProcessingServiceMock.Verify(processing =>
            //    processing.ModifyOrAddResolvedAddressAsync(updatedResolvedAddress),
            //        Times.Once);

            //this.auditBrokerMock.Verify(broker =>
            //    broker.LogInformation(
            //        "Resolved Address",
            //        "Successfully resolved and address to the database",
            //        $"Successfully persisted address with id: " +
            //            $"{updatedResolvedAddress.Id} with a {updatedResolvedAddress.MatchAlgorithmEnum} match",
            //        updatedResolvedAddress.MatchAlgorithmEnum.ToString(),
            //        updatedResolvedAddress.Id),
            //            Times.Once());

            //this.addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            //this.addressProcessingServiceMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

