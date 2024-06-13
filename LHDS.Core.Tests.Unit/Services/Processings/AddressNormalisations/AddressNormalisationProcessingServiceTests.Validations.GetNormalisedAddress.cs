// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnGetNormalisedAddressIfAddressIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidAddress = invalidText;

            var invalidArgumentAddressNormalisationProcessingException =
                new InvalidArgumentAddressNormalisationProcessingException(
                    message:
                    "Invalid address normalisation processing argument. Please correct the errors and try again.");

            invalidArgumentAddressNormalisationProcessingException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressNormalisationProcessingValidationException =
                new AddressNormalisationProcessingValidationException(
                    message: "Address normalisation processing validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressNormalisationProcessingException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
                this.addressNormalisationProcessingService.GetNormalisedAddress(invalidAddress);

            AddressNormalisationProcessingValidationException actualAddressNormalisationProcessingValidationException =
                await Assert.ThrowsAsync<AddressNormalisationProcessingValidationException>(
                    addAddressNormalisationTask.AsTask);

            // then
            actualAddressNormalisationProcessingValidationException.Should()
                .BeEquivalentTo(expectedAddressNormalisationProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressNormalisationProcessingValidationException))),
                        Times.Once);

            this.addressNormalisationServiceMock.Verify(broker =>
                broker.GetNormalisedAddress(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
        }
    }
}