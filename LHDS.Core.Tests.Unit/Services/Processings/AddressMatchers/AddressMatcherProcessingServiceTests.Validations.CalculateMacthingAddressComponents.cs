// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnCalculateMacthingAddressIfArgsIsInvalidAndLogItAsync()
        {
            // given
            List<KeyValuePair<string, string>> invalidIncomingAddressComponents =
                new List<KeyValuePair<string, string>>();

            HashSet<AddressMatch> invalidPossibleAddresses = new HashSet<AddressMatch>();

            var invalidArgumentAddressMatcherException =
                new InvalidArgumentAddressMatcherProcessingException(
                    message: "Invalid address matcher processing argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentAddressMatcherException.AddData(
                key: "IncomingAddressComponents",
                values: "Values is required");

            invalidArgumentAddressMatcherException.AddData(
                key: "PossibleAddresses",
                values: "Values is required");

            var expectedAddressMatcherValidationException =
                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressMatcherException);

            // when
            ValueTask<HashSet<AddressMatch>> calculateMacthingAddressComponentsTask =
                addressMatcherProcessingService.CalculateMatchingAddressComponents(
                    addressComponents: invalidIncomingAddressComponents,
                    possibleAddressMatches: invalidPossibleAddresses);

            AddressMatcherProcessingValidationException actualAddressMatcherValidationException =
                await Assert.ThrowsAsync<AddressMatcherProcessingValidationException>(
                    calculateMacthingAddressComponentsTask.AsTask);

            // then
            actualAddressMatcherValidationException.Should()
                .BeEquivalentTo(expectedAddressMatcherValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressMatcherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
