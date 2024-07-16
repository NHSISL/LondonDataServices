// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowValidationExceptionsOnBulkModifyIfResolvedAddressProcessingIsNullAndLogItAsync()
        {
            // given
            List<ResolvedAddress> nullResolvedAddresses = null;

            var invalidArgumentResolvedAddressProcessingException =
                new InvalidArgumentResolvedAddressProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentResolvedAddressProcessingException.AddData(
                key: "resolvedAddresses",
                values: "Resolved addresses is required");

            invalidArgumentResolvedAddressProcessingException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedResolvedAddressProcessingValidationException =
                new ResolvedAddressProcessingValidationException(
                    message: "Resolved address processing validation error occurred, please try again.",
                    innerException: invalidArgumentResolvedAddressProcessingException);

            // when
            ValueTask bulkModifyResolvedAddressTask = this.resolvedAddressProcessingService
                .BulkModifyResolvedAddressesAsync(resolvedAddresses: nullResolvedAddresses);

            ResolvedAddressProcessingValidationException actualResolvedAddressProcessingValidationException =
                await Assert.ThrowsAsync<ResolvedAddressProcessingValidationException>(
                    bulkModifyResolvedAddressTask.AsTask);

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