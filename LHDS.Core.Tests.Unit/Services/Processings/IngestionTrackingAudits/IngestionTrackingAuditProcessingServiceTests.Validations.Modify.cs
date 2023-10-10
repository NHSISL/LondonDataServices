// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnModifyIfIngestionTrackingAuditProcessingIsNullAndLogItAsync()
        {
            // given
            Audit nullIngestionTrackingAudit = null;

            var nullIngestionTrackingAuditProcessingException =
                new NullIngestionTrackingAuditProcessingException(message: "IngestionTrackingAudit is null.");

            var expectedIngestionTrackingAuditProcessingValidationException =
                new IngestionTrackingAuditProcessingValidationException(
                    message: "IngestionTrackingAudit processing validation error occurred, please try again.",
                    innerException: nullIngestionTrackingAuditProcessingException);

            // when
            ValueTask<Audit> AddIngestionTrackingAuditTask =
                this.ingestionTrackingAuditProcessingService
                    .ModifyIngestionTrackingAuditAsync(nullIngestionTrackingAudit);

            IngestionTrackingAuditProcessingValidationException actualIngestionTrackingAuditProcessingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingValidationException>(
                    AddIngestionTrackingAuditTask.AsTask);

            //then
            actualIngestionTrackingAuditProcessingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditProcessingValidationException))),
                        Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
