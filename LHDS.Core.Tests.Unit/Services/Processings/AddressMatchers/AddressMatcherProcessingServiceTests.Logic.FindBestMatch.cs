// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressMatchers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAttemptFindBestMatchOnNoneWithSuccess()
        {
            // given
            HashSet<AddressMatch> inputAddressMatches = CreateNoBestMatch();
            HashSet<AddressMatch> outputAddressMatches = inputAddressMatches;
            HashSet<AddressMatch> cleanedOutputAddressMatches = CreateSingleBestMatch();

            AddressMatch expectedAddressMatch = cleanedOutputAddressMatches.FirstOrDefault();
            List<KeyValuePair<string, string>> inputAddressComponents = CreateRandomKeyValuePairList();
            List<KeyValuePair<string, string>> outputAddressComponents = CreateRandomKeyValuePairList();

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches))
                    .Returns(outputAddressMatches);

            this.addressMatcherServiceMock.Setup(service =>
                service.CheckForBestMatch(outputAddressMatches))
                    .Returns(BestMatchEnum.None);

            this.addressMatcherServiceMock.Setup(service =>
                service.RemoveNonDigitCharactersFromHouseNumber(inputAddressComponents))
                    .Returns(outputAddressComponents);

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(outputAddressComponents, inputAddressMatches))
                    .Returns(cleanedOutputAddressMatches);

            this.addressMatcherServiceMock.Setup(service =>
                service.CheckForBestMatch(cleanedOutputAddressMatches))
                    .Returns(BestMatchEnum.Single);

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMatch(inputAddressMatches, inputAddressComponents);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);

            this.addressMatcherServiceMock.Verify(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CheckForBestMatch(outputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.RemoveNonDigitCharactersFromHouseNumber(inputAddressComponents),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CalculateMatchingAddressComponents(outputAddressComponents, inputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CheckForBestMatch(cleanedOutputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAttemptFindBestMatchOnNoneWithoutSuccess()
        {
            // given
            HashSet<AddressMatch> inputAddressMatches = CreateNoBestMatch();
            HashSet<AddressMatch> outputAddressMatches = inputAddressMatches;
            HashSet<AddressMatch> cleanedOutputAddressMatches = CreateSingleBestMatch();

            AddressMatch expectedAddressMatch = new AddressMatch
            {
                IsMatched = false,
                BestMatch = BestMatchEnum.None
            };

            List<KeyValuePair<string, string>> inputAddressComponents = CreateRandomKeyValuePairList();
            List<KeyValuePair<string, string>> outputAddressComponents = CreateRandomKeyValuePairList();

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches))
                    .Returns(outputAddressMatches);

            this.addressMatcherServiceMock.Setup(service =>
                service.CheckForBestMatch(outputAddressMatches))
                    .Returns(BestMatchEnum.None);

            this.addressMatcherServiceMock.Setup(service =>
                service.RemoveNonDigitCharactersFromHouseNumber(inputAddressComponents))
                    .Returns(outputAddressComponents);

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(outputAddressComponents, inputAddressMatches))
                    .Returns(cleanedOutputAddressMatches);

            this.addressMatcherServiceMock.Setup(service =>
                service.CheckForBestMatch(cleanedOutputAddressMatches))
                    .Returns(BestMatchEnum.None);

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMatch(inputAddressMatches, inputAddressComponents);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);

            this.addressMatcherServiceMock.Verify(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CheckForBestMatch(outputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.RemoveNonDigitCharactersFromHouseNumber(inputAddressComponents),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CalculateMatchingAddressComponents(outputAddressComponents, inputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CheckForBestMatch(cleanedOutputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAttemptFindBestMatchOnSingleWithSuccess()
        {
            // given
            HashSet<AddressMatch> inputAddressMatches = CreateSingleBestMatch();
            HashSet<AddressMatch> outputAddressMatches = inputAddressMatches;
            AddressMatch expectedAddressMatch = outputAddressMatches.FirstOrDefault();
            List<KeyValuePair<string, string>> randomAddressComponents = CreateKeyValuePairList();
            List<KeyValuePair<string, string>> inputAddressComponents = randomAddressComponents;

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches))
                    .Returns(outputAddressMatches);

            this.addressMatcherServiceMock.Setup(service =>
                service.CheckForBestMatch(outputAddressMatches))
                    .Returns(BestMatchEnum.Single);

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMatch(inputAddressMatches, inputAddressComponents);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);

            this.addressMatcherServiceMock.Verify(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CheckForBestMatch(outputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAttemptFindBestMatchOnMultipleWithSuccess()
        {
            // given
            HashSet<AddressMatch> inputAddressMatches = CreateSingleBestMatch();
            HashSet<AddressMatch> outputAddressMatches = inputAddressMatches;
            AddressMatch expectedAddressMatch = inputAddressMatches.First().DeepClone();
            expectedAddressMatch.IsMatched = true;
            expectedAddressMatch.BestMatch = BestMatchEnum.Multiple;
            List<KeyValuePair<string, string>> randomAddressComponents = CreateKeyValuePairList();
            List<KeyValuePair<string, string>> inputAddressComponents = randomAddressComponents;

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches))
                    .Returns(outputAddressMatches);

            this.addressMatcherServiceMock.Setup(service =>
                service.CheckForBestMatch(outputAddressMatches))
                    .Returns(BestMatchEnum.Multiple);

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMatch(inputAddressMatches, inputAddressComponents);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);

            this.addressMatcherServiceMock.Verify(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.Verify(service =>
                service.CheckForBestMatch(outputAddressMatches),
                    Times.Once);

            this.addressMatcherServiceMock.VerifyNoOtherCalls();
        }
    }
}
