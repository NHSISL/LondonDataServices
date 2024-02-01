// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnIsMatchWithInvalidInputsAndLogItAsync(string invalidText)
        {
            // given
            string invalidAddress = invalidText;

            var invalidArgumentResolvedAddressProcessingException =
                new InvalidArgumentResolvedAddressProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentResolvedAddressProcessingException.AddData(
                key: "Address",
                values: "Text is required");

            var expectedResolvedAddressProcessingValidationException =
                new ResolvedAddressProcessingValidationException(
                    message: "ResolvedAddress processing validation error occurred, please try again.",
                    innerException: invalidArgumentResolvedAddressProcessingException);

            // when
            ValueTask<(bool IsMatched, Guid? ItemId)> isMatchTask = this.resolvedAddressProcessingService
                .IsExactMatchForResolvedAddressAsync(address: invalidAddress);

            ResolvedAddressProcessingValidationException actualResolvedAddressProcessingValidationException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingValidationException>(isMatchTask.AsTask);

            //then
            actualResolvedAddressProcessingValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressProcessingValidationException))),
                        Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}