// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
            Guid invalidSubscriberAgreementId = Guid.Empty;

            var invalidArgumentIngestionTrackingProcessingException =
                new InvalidArgumentIngestionTrackingProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentIngestionTrackingProcessingException.AddData(
                key: "batchReference",
                values: "Text is required");

            invalidArgumentIngestionTrackingProcessingException.AddData(
                key: "subscriberAgreementId",
                values: "Id is required");

            var expectedIngestionTrackingProcessingValidationException =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: invalidArgumentIngestionTrackingProcessingException);

            // when
            ValueTask<List<string>> RetrieveIngestionTrackingTask =
                this.ingestionTrackingProcessingService.RetrieveObjectsInBatchByBatchReferenceAsync(
                    invalidBatchReference,
                    invalidSubscriberAgreementId);

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
