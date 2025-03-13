// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnRetrieveObjectsInBatchByBatchRefIfIdIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidBatchReference = invalidText;

            var invalidArgumentIngestionTrackingProcessingException =
                new InvalidArgumentIngestionTrackingProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentIngestionTrackingProcessingException.AddData(
                key: "batchReference",
                values: "Text is required");

            var expectedIngestionTrackingProcessingValidationException =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: invalidArgumentIngestionTrackingProcessingException);

            // when
            ValueTask<List<string>> RetrieveIngestionTrackingTask =
                this.ingestionTrackingProcessingService.RetrieveObjectsInBatchByBatchReferenceAsync(invalidBatchReference);

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
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
