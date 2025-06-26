// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidResolvedAddressAuditId = Guid.Empty;

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.Id),
                values: "Id is required");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> removeResolvedAddressAuditByIdTask =
                this.resolvedAddressAuditService.RemoveResolvedAddressAuditByIdAsync(invalidResolvedAddressAuditId);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    removeResolvedAddressAuditByIdTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}