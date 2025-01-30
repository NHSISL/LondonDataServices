// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionsOnAddIfIngestionTrackingAuditProcessingIsNullAndLogItAsync()
        {
            // given
            IngestionTrackingAudit nullIngestionTrackingAudit = null;

            var nullIngestionTrackingAuditProcessingException =
                new NullIngestionTrackingAuditProcessingException(message: "IngestionTrackingAudit is null.");

            var expectedIngestionTrackingAuditProcessingValidationException =
                new IngestionTrackingAuditProcessingValidationException(
                    message: "IngestionTrackingAudit processing validation error occurred, please try again.",
                    innerException: nullIngestionTrackingAuditProcessingException);

            // when
            ValueTask<IngestionTrackingAudit> AddIngestionTrackingAuditTask =
                this.ingestionTrackingAuditProcessingService.AddIngestionTrackingAuditAsync(nullIngestionTrackingAudit);

            IngestionTrackingAuditProcessingValidationException actualIngestionTrackingAuditProcessingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingValidationException>(AddIngestionTrackingAuditTask.AsTask);

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
