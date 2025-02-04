// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.OptOuts
{
    public partial class OptOutProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRemoveIfOptOutProcessingIdIsNullAndLogItAsync()
        {
            // given
            Guid invalidIngestionTrackingId = Guid.Empty;

            var invalidArgumentOptOutProcessingException =
                new InvalidArgumentOptOutProcessingException(
                    message: "Invalid opt out processing argument. Please correct the errors and try again.");

            invalidArgumentOptOutProcessingException.AddData(
                key: nameof(OptOut.Id),
                values: "Id is required");

            var expectedOptOutProcessingValidationException =
                new OptOutProcessingValidationException(
                    innerException: invalidArgumentOptOutProcessingException);

            // when
            ValueTask<OptOut> RemoveOptOutTask =
                this.optOutProcessingService.RemoveOptOutByIdAsync(invalidIngestionTrackingId);

            OptOutProcessingValidationException actualOptOutProcessingValidationException =
                await Assert.ThrowsAsync<OptOutProcessingValidationException>(RemoveOptOutTask.AsTask);

            //then
            actualOptOutProcessingValidationException.Should()
                .BeEquivalentTo(expectedOptOutProcessingValidationException);

            this.optOutServiceMock.Verify(service =>
                service.RemoveOptOutByIdAsync(invalidIngestionTrackingId),
                    Times.Never());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutProcessingValidationException))),
                        Times.Once);

            this.optOutServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}