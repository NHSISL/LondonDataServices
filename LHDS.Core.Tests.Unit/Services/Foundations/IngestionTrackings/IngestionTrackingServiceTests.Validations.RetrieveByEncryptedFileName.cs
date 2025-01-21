// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByEncryptedFileNameIfFileNameIsInvalidAndLogItAsync()
        {
            // given
            string encryptedFileName = String.Empty;

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException(message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.FileName),
                values: "Text is required");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> retrieveIngestionTrackingByFileNameTask =
                this.ingestionTrackingService.RetrieveIngestionTrackingByEncryptedFileNameAsync(encryptedFileName);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    retrieveIngestionTrackingByFileNameTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}