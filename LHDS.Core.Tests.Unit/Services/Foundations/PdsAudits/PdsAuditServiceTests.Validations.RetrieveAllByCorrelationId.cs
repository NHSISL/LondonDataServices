// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveAllByCorrelationIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidCorrelationId = Guid.Empty;

            var invalidPdsAuditException =
                new InvalidPdsAuditException(message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CorrelationId),
                values: "Id is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            // when
            ValueTask<IQueryable<PdsAudit>> retrieveAllByCorrelationIdTask =
                this.pdsAuditService.RetrieveAllPdsAuditsByCorrelationIdAsync(invalidCorrelationId);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    retrieveAllByCorrelationIdTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}