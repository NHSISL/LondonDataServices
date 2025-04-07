// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveOrAddIfAddressProcessingIsNullAndLogItAsync()
        {
            // given
            Address nullAddress = null;

            var nullAddressProcessingException =
                new NullAddressProcessingException(message: "Address is null.");

            var expectedAddressProcessingValidationException =
                new AddressProcessingValidationException(
                    message: "Address processing validation error occurred, please try again.",
                    innerException: nullAddressProcessingException);

            // when
            ValueTask<Address> AddAddressTask =
                this.addressProcessingService.RetrieveOrAddAddressAsync(nullAddress);

            AddressProcessingValidationException actualAddressProcessingValidationException =
                await Assert.ThrowsAsync<AddressProcessingValidationException>(AddAddressTask.AsTask);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveOrAddIfIdIsInvalidAndLogItAsync()
        {
            // given
            Address invalidAddress = new Address();

            var invalidArgumentAddressProcessingException =
                new InvalidArgumentAddressProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentAddressProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedAddressProcessingValidationException =
                new AddressProcessingValidationException(
                    message: "Address processing validation error occurred, please try again.",
                    innerException: invalidArgumentAddressProcessingException);

            // when
            ValueTask<Address> RetrieveAddressTask =
                this.addressProcessingService.RetrieveOrAddAddressAsync(invalidAddress);

            AddressProcessingValidationException actualAddressProcessingValidationException =
                await Assert.ThrowsAsync<AddressProcessingValidationException>(RetrieveAddressTask.AsTask);

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
