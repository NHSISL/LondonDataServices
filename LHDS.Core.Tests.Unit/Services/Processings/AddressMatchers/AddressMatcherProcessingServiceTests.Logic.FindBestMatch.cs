// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAttemptFindBestMatchOnNone()
        {

        }


        [Fact]
        public async Task ShouldAttemptFindBestMatchOnSingle()
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
        public async Task ShouldAttemptFindBestMatchOnMultiple()
        {
            // given
            HashSet<AddressMatch> inputAddressMatches = CreateMultipleBestMatch();
            HashSet<AddressMatch> outputAddressMatches = CreateMultipleBestMatch();
            AddressMatch expectedAddressMatch = outputAddressMatches.FirstOrDefault();
            List<KeyValuePair<string, string>> randomAddressComponents = CreateKeyValuePairList();
            List<KeyValuePair<string, string>> inputAddressComponents = randomAddressComponents;

            this.addressMatcherServiceMock.Setup(service =>
                service.CalculateMatchingAddressComponents(inputAddressComponents, inputAddressMatches))
                    .Returns(outputAddressMatches);

            // when
            AddressMatch actualAddressMatch = await this.addressMatcherProcessingService
                .FindBestMatch(inputAddressMatches, inputAddressComponents);

            // then
            actualAddressMatch.Should().BeEquivalentTo(expectedAddressMatch);
        }
    }
}
