// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        [Fact]
        public void ShouldThrowValidationExceptionOnRemoveNonDigitCharactersIfArgsIsInvalidAndLogIt()
        {
            // given
            IList<KeyValuePair<string, string>> invalidAddress = new List<KeyValuePair<string, string>>();

            var invalidArgumentAddressMatcherException =
                new InvalidArgumentAddressMatcherException(
                    message: "Invalid address matcher argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentAddressMatcherException.AddData(
                key: "AddressComponents",
                values: "Values is required");

            var expectedAddressMatcherValidationException =
                new AddressMatcherValidationException(
                    message: "Address matcher validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressMatcherException);

            // when
            Action removeNonDigitCharactersFromHouseNumberAction = () =>
                addressMatcherService.RemoveNonDigitCharactersFromHouseNumber(addressComponents: invalidAddress);

            AddressMatcherValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherValidationException>(removeNonDigitCharactersFromHouseNumberAction);

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

