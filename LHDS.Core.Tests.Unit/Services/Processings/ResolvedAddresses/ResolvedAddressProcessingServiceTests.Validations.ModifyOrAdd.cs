// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfResolvedAddressProcessingIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress nullResolvedAddress = null;

            var nullResolvedAddressProcessingException =
                new NullResolvedAddressProcessingException(message: "Resolved address is null.");

            var expectedResolvedAddressProcessingValidationException =
                new ResolvedAddressProcessingValidationException(
                    message: "Resolved address processing validation error occurred, please try again.",
                    innerException: nullResolvedAddressProcessingException);

            // when
            ValueTask<ResolvedAddress> AddResolvedAddressTask =
                this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressProcessingValidationException actualResolvedAddressProcessingValidationException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingValidationException>(AddResolvedAddressTask.AsTask);

            //then
            actualResolvedAddressProcessingValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressProcessingValidationException))),
                        Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyOrAddIfResolvedAddressDoesNotHaveValidIdAndLogItAsync()
        {
            // given
            ResolvedAddress emptyResolvedAddress = new ResolvedAddress();

            var invalidArgumentResolvedAddressProcessingException =
                new InvalidArgumentResolvedAddressProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentResolvedAddressProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedResolvedAddressProcessingValidationException =
                new ResolvedAddressProcessingValidationException(
                    message: "Resolved address processing validation error occurred, please try again.",
                    innerException: invalidArgumentResolvedAddressProcessingException);

            // when
            ValueTask<ResolvedAddress> AddResolvedAddressTask =
                this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(emptyResolvedAddress);

            ResolvedAddressProcessingValidationException actualResolvedAddressProcessingValidationException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingValidationException>(AddResolvedAddressTask.AsTask);

            //then
            actualResolvedAddressProcessingValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressProcessingValidationException))),
                        Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}