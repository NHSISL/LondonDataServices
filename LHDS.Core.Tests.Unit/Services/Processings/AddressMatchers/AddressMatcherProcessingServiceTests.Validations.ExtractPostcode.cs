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

        [Theory]
        [InlineData("123 Main Street, London, W1A 1AA, W2 2BB, United Kingdom")]
        [InlineData("123 Main Street, London, CR2 1AA, W2 2BB, United Kingdom")]
        public async Task ShouldThrowValidationExceptionOnExtractPostcodeIfReturnedGreaterThanOneAndLogItAsync(
            string duplicateAddresse)
        {
            // given
            string cleanedAddress = duplicateAddresse;

            var multiplePostCodesAddressMatcherProcessingServiceException =
                new MultiplePostCodesAddressMatcherProcessingServiceException(
                    message: "Multiple Postcodes validation error occurred, please try again.");

            var expectedAddressMatcherValidationException =
                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException: multiplePostCodesAddressMatcherProcessingServiceException);

            // when
            Action extractPostcodeAction = () =>
                addressMatcherProcessingService.ExtractPostCode(cleanedAddress);

            AddressMatcherProcessingValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherProcessingValidationException>(extractPostcodeAction);

            // then
            actualAddressMatcherValidationException.Should()
                .BeEquivalentTo(expectedAddressMatcherValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogNothing(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressMatcherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("123 Main Street, London, United Kingdom")]
        [InlineData("123 Hayes Street, London, United Kingdom")]
        public async Task ShouldThrowValidationExceptionOnExtractPostcodeIfReturnedNoPostcodeAndLogItAsync(
           string duplicateAddresse)
        {
            // given
            string cleanedAddress = duplicateAddresse;

            var postCodeNotFoundAddressMatcherProcessingServiceException =
                new PostCodeNotFoundAddressMatcherProcessingServiceException(
                    message: "No Postcodes found validation error occurred, please try again.");

            var expectedAddressMatcherValidationException =
                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException: postCodeNotFoundAddressMatcherProcessingServiceException);

            // when
            Action extractPostcodeAction = () =>
                addressMatcherProcessingService.ExtractPostCode(cleanedAddress);

            AddressMatcherProcessingValidationException actualAddressMatcherValidationException =
                Assert.Throws<AddressMatcherProcessingValidationException>(extractPostcodeAction);

            // then
            actualAddressMatcherValidationException.Should()
                .BeEquivalentTo(expectedAddressMatcherValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogNothing(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressMatcherValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
