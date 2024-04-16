using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAuditIsNullAndLogItAsync()
        {
            // given
            Audit nullAudit = null;

            var nullAuditException =
                new NullAuditException(message: "Audit is null.");

            var expectedAuditValidationException =
                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException: nullAuditException);

            // when
            ValueTask<Audit> addAuditTask =
                this.auditService.AddAuditAsync(nullAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(
                    addAuditTask.AsTask);

            // then
            actualAuditValidationException.Should().BeEquivalentTo(expectedAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}