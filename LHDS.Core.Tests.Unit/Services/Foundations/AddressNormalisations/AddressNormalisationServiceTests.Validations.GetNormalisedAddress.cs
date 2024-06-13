// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressNormalisationIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidAddress = invalidText;

            var invalidAddressNormalisationArgumentException =
                new InvalidAddressNormalisationArgumentException(
                    message: "Invalid address normalisation argument. Please correct the errors and try again.");

            invalidAddressNormalisationArgumentException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressNormalisationValidationException =
                new AddressNormalisationValidationException(
                    message: "Address normalisation validation errors occurred, please try again.",
                    innerException: invalidAddressNormalisationArgumentException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
                this.addressNormalisationService.GetNormalisedAddress(invalidAddress);

            AddressNormalisationValidationException actualAddressNormalisationValidationException =
                await Assert.ThrowsAsync<AddressNormalisationValidationException>(addAddressNormalisationTask.AsTask);

            // then
            actualAddressNormalisationValidationException.Should()
                .BeEquivalentTo(expectedAddressNormalisationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressNormalisationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
        }
    }
}