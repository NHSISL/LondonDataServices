// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
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
        public async Task ShouldThrowValidationExceptionOnCleanAddressIfArgsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string address = invalidText;

            var invalidArgumentAddressMatcherException =
                new InvalidArgumentAddressMatcherException(
                    message: "Invalid Address Matcher argument(s), please correct the errors and try again.");

            invalidArgumentAddressMatcherException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressMatcherValidationException =
                new AddressMatcherValidationException(
                    message: "Address matcher validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressMatcherException);

            // when
            Action cleanAddressAction = () =>
                addressMatcherProcessingService.CleanAddress(address);

            AddressMatcherValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherValidationException>(cleanAddressAction);

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
