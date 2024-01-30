// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
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
                new InvalidArgumentAddressMatcherProcessingException(
                    message: "Invalid address matcher processing argument(s), " +
                    "please correct the errors and try again.");

            invalidArgumentAddressMatcherException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressMatcherValidationException =
                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressMatcherException);

            // when
            Action cleanAddressAction = () =>
                addressMatcherProcessingService.CleanAddress(address);

            AddressMatcherProcessingValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherProcessingValidationException>(cleanAddressAction);

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
