using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Audits;
using LHDS.Core.Models.Audits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAuditIsNullAndLogItAsync()
        {
            // given
            Audit nullAudit = null;
            var nullAuditException = new NullAuditException();

            var expectedAuditValidationException =
                new AuditValidationException(nullAuditException);

            // when
            ValueTask<Audit> modifyAuditTask =
                this.auditService.ModifyAuditAsync(nullAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(
                    modifyAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAuditAsync(It.IsAny<Audit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidAudit = new Audit
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidAuditException = new InvalidAuditException();

            invalidAuditException.AddData(
                key: nameof(Audit.Id),
                values: "Id is required");

            //invalidAuditException.AddData(
            //    key: nameof(Audit.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Audit model

            invalidAuditException.AddData(
                key: nameof(Audit.CreatedDate),
                values: "Date is required");

            invalidAuditException.AddData(
                key: nameof(Audit.CreatedByUserId),
                values: "Id is required");

            invalidAuditException.AddData(
                key: nameof(Audit.UpdatedDate),
                values: "Date is required");

            invalidAuditException.AddData(
                key: nameof(Audit.UpdatedByUserId),
                values: "Id is required");

            var expectedAuditValidationException =
                new AuditValidationException(invalidAuditException);

            // when
            ValueTask<Audit> modifyAuditTask =
                this.auditService.ModifyAuditAsync(invalidAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(
                    modifyAuditTask.AsTask);

            //then
            actualAuditValidationException.Should().BeEquivalentTo(expectedAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAuditAsync(It.IsAny<Audit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}