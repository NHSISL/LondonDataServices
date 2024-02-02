// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnCalculateMacthingAddressIfArgsIsInvalidAndLogItAsync()
        {
            // given
            List<KeyValuePair<string, string>> invalidIncomingAddressComponents =
                new List<KeyValuePair<string, string>>();

            HashSet<AddressMatch> invalidPossibleAddresses = new HashSet<AddressMatch>();

            var invalidArgumentAddressMatcherException =
                new InvalidArgumentAddressMatcherException(
                    message: "Invalid address matcher argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentAddressMatcherException.AddData(
                key: "IncomingAddressComponents",
                values: "Values is required");

            invalidArgumentAddressMatcherException.AddData(
                key: "PossibleAddresses",
                values: "Values is required");

            var expectedAddressMatcherValidationException =
                new AddressMatcherValidationException(
                    message: "Address matcher validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressMatcherException);

            // when
            Action calculateMacthingAddressComponentsAction = () =>
                addressMatcherService.CalculateMacthingAddressComponents(
                    addressComponents: invalidIncomingAddressComponents,
                    possibleAddressMatches: invalidPossibleAddresses);

            AddressMatcherValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherValidationException>(
                    calculateMacthingAddressComponentsAction);

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