// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldThrowValidationExceptionsOnMarkAsBatchCompleteIfIdIsInvalidAndLogItAsync(
            bool isBatchComplete)
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentIngestionTrackingProcessingException =
                new InvalidArgumentIngestionTrackingProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentIngestionTrackingProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedIngestionTrackingProcessingValidationException =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: invalidArgumentIngestionTrackingProcessingException);

            // when
            ValueTask markAsBatchCompleteTask =
                this.ingestionTrackingProcessingService.MarkAsBatchCompleteAsync(invalidId, isBatchComplete);

            IngestionTrackingProcessingValidationException actualIngestionTrackingProcessingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingValidationException>(
                    markAsBatchCompleteTask.AsTask);

            //then
            actualIngestionTrackingProcessingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingProcessingValidationException))),
                        Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
