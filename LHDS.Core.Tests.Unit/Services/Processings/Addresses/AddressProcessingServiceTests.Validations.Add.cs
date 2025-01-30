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
        public async Task ShouldThrowValidationExceptionsOnAddIfAddressProcessingIsNullAndLogItAsync()
        {
            // given
            Address nullAddress = null;

            var nullAddressProcessingException =
                new NullAddressProcessingException(message: "Address is null.");

            var expectedAddressProcessingValidationException =
                new AddressProcessingValidationException(
                    message: "Address processing validation error occurred, please try again.",
                    innerException: nullAddressProcessingException);

            // LogErrorAsync(
            ValueTask<Address> AddAddressTask =
                this.addressProcessingService.AddAddressAsync(nullAddress);

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
    }
}
