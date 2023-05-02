using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidPdsAuditId = Guid.Empty;

            var invalidPdsAuditException =
                new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Id),
                values: "Id is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> removePdsAuditByIdTask =
                this.pdsAuditService.RemovePdsAuditByIdAsync(invalidPdsAuditId);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    removePdsAuditByIdTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}