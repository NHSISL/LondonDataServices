using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Xunit;
using LHDS.Core.Models.Foundations.AddressNormalisation;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;

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
                    message: "Invalid addressNormalisation argument. Please correct the errors and try again.");

            invalidAddressNormalisationArgumentException.AddData(
                key: nameof(invalidAddress),
                values: "Text is required");

            var expectedAddressNormalisationValidationException =
                new AddressNormalisationValidationException(
                    message: "AddressNormalisation validation errors occurred, please try again.",
                    innerException: invalidAddressNormalisationArgumentException);

            // when
            ValueTask<AddressNormalisation> addAddressNormalisationTask =
                this.addressNormalisationService.GetNormalisedAddress(invalidAddress);

            AddressNormalisationValidationException actualAddressNormalisationValidationException =
                await Assert.ThrowsAsync<AddressNormalisationValidationException>(() =>
                    addAddressNormalisationTask.AsTask());

            // then
            actualAddressNormalisationValidationException.Should()
                .BeEquivalentTo(expectedAddressNormalisationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressNormalisationValidationException))),
                        Times.Once);

            this.addressNormalisationBrokerMock.Verify(broker =>
                broker.GetNormalisedAddress(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationBrokerMock.VerifyNoOtherCalls();
        }
    }
}