// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveOrAddIfIngestionTrackingIsNullAndLogItAsync()
        {
            // given
            IngestionTracking nullIngestionTracking = null;

            var nullIngestionTrackingProcessingException =
                new NullIngestionTrackingProcessingException(message: "IngestionTracking is null.");

            var expectedIngestionTrackingProcessingValidationException =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: nullIngestionTrackingProcessingException);

            // when
            ValueTask<IngestionTracking> AddIngestionTrackingTask =
                this.ingestionTrackingProcessingService.RetrieveOrAddIngestionTrackingAsync(nullIngestionTracking);

            IngestionTrackingProcessingValidationException actualIngestionTrackingProcessingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingValidationException>(
                    AddIngestionTrackingTask.AsTask);

            //then
            actualIngestionTrackingProcessingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingProcessingValidationException))),
                        Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveOrAddIfIdIsInvalidAndLogItAsync()
        {
            // given
            IngestionTracking invalidIngestionTracking = new IngestionTracking();

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
            ValueTask<IngestionTracking> RetrieveIngestionTrackingTask =
                this.ingestionTrackingProcessingService.RetrieveOrAddIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingProcessingValidationException actualIngestionTrackingProcessingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingValidationException>(
                    RetrieveIngestionTrackingTask.AsTask);

            //then
            actualIngestionTrackingProcessingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingProcessingValidationException))),
                        Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
