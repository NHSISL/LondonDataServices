// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Theory(Skip = "No longer used, will refactor out")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnIsMatchWithInvalidInputsAndLogItAsync(string invalidText)
        {
            //// given
            //string invalidAddress = invalidText;

            //var invalidArgumentAddressProcessingException =
            //    new InvalidArgumentAddressProcessingException(
            //        message: "Invalid argument(s). Please correct the errors and try again.");

            //invalidArgumentAddressProcessingException.AddData(
            //    key: "Address",
            //    values: "Text is required");

            //var expectedAddressProcessingValidationException =
            //    new AddressProcessingValidationException(
            //        message: "Address processing validation error occurred, please try again.",
            //        innerException: invalidArgumentAddressProcessingException);

            //// when
            //ValueTask<bool> isMatchTask = this.addressProcessingService
            //    .IsExactMatchForAddressAsync(addressToMatch: invalidAddress);

            //AddressProcessingValidationException actualAddressProcessingValidationException =
            //    await Assert.ThrowsAsync<AddressProcessingValidationException>(isMatchTask.AsTask);

            ////then
            //actualAddressProcessingValidationException.Should()
            //    .BeEquivalentTo(expectedAddressProcessingValidationException);

            //this.loggingBrokerMock.Verify(broker =>
            //    broker.LogError(It.Is(SameExceptionAs(
            //        expectedAddressProcessingValidationException))),
            //            Times.Once);

            //this.addressServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
