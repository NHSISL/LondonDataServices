// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.AddressMatcher.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnExtractPostcodeIfArgsIsInvalidOrEmptyStringAndLogItAsync(
           string invalidText)
        {
            // given
            string cleanedAddress = invalidText;

            var invalidArgumentAddressMatcherProcessingException =
                new InvalidArgumentAddressMatcherProcessingException(
                    message: "Invalid address matcher processing argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentAddressMatcherProcessingException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressMatcherValidationException =
                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressMatcherProcessingException);

            // when
            Action extractPostcodeAction = () =>
                addressMatcherProcessingService.ExtractPostCode(cleanedAddress);

            AddressMatcherProcessingValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherProcessingValidationException>(extractPostcodeAction);

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
