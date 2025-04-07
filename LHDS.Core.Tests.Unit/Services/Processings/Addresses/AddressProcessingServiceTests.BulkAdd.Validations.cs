// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Addresses
{
    public partial class AddressProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnBulkAddIfAddressProcessingIsNullAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidFileName = invalidText;
            List<Address> nullAddresses = null;

            var invalidArgumentAddressProcessingException =
                new InvalidArgumentAddressProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentAddressProcessingException.AddData(
                key: "Addresses",
                values: "Addresses is required");

            invalidArgumentAddressProcessingException.AddData(
                key: "FileName",
                values: "Text is required");

            var expectedAddressProcessingValidationException =
                new AddressProcessingValidationException(
                    message: "Address processing validation error occurred, please try again.",
                    innerException: invalidArgumentAddressProcessingException);

            // when
            ValueTask BulkAddAddressTask = this.addressProcessingService
                .BulkAddAddressesAsync(addresses: nullAddresses, fileName: invalidFileName);

            AddressProcessingValidationException actualAddressProcessingValidationException =
                await Assert.ThrowsAsync<AddressProcessingValidationException>(BulkAddAddressTask.AsTask);

            //then
            actualAddressProcessingValidationException.Should()
                .BeEquivalentTo(expectedAddressProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressProcessingValidationException))),
                        Times.Once);

            this.addressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
