// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentIngestionTrackingAuditProcessingException =
                new InvalidArgumentIngestionTrackingAuditProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentIngestionTrackingAuditProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedIngestionTrackingAuditProcessingValidationException =
                new IngestionTrackingAuditProcessingValidationException(
                    message: "IngestionTrackingAudit processing validation error occurred, please try again.",
                    innerException: invalidArgumentIngestionTrackingAuditProcessingException);

            // when
            ValueTask<IngestionTrackingAudit> RetrieveIngestionTrackingAuditTask =
                this.ingestionTrackingAuditProcessingService.RetrieveIngestionTrackingAuditByIdAsync(invalidId);

            IngestionTrackingAuditProcessingValidationException
                actualIngestionTrackingAuditProcessingValidationException =
                    await Assert.ThrowsAsync<IngestionTrackingAuditProcessingValidationException>(
                        RetrieveIngestionTrackingAuditTask.AsTask);

            //then
            actualIngestionTrackingAuditProcessingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditProcessingValidationException))),
                        Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
