using System;
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
        public async Task ShouldThrowValidationExceptionOnModifyIfAuditIsNullAndLogItAsync()
        {
            // given
            Audit nullAudit = null;
            var nullAuditException = new NullAuditException(message: "Audit is null.");

            var expectedAuditValidationException =
                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException: nullAuditException);

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

            var invalidAuditException = 
                new InvalidAuditException(
                    message: "Invalid audit. Please correct the errors and try again.");

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
                key: nameof(Audit.CreatedBy),
                values: "Text is required");

            invalidAuditException.AddData(
                key: nameof(Audit.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Audit.CreatedDate)}"
                });

            invalidAuditException.AddData(
                key: nameof(Audit.UpdatedBy),
                values: "Text is required");

            var expectedAuditValidationException =
                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException: invalidAuditException);

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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Audit randomAudit = CreateRandomAudit(randomDateTimeOffset);
            Audit invalidAudit = randomAudit;
            
            var invalidAuditException = 
                new InvalidAuditException(
                    message: "Invalid audit. Please correct the errors and try again.");

            invalidAuditException.AddData(
                key: nameof(Audit.UpdatedDate),
                values: $"Date is the same as {nameof(Audit.CreatedDate)}");

            var expectedAuditValidationException =
                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException: invalidAuditException);

            // when
            ValueTask<Audit> modifyAuditTask =
                this.auditService.ModifyAuditAsync(invalidAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(
                    modifyAuditTask.AsTask);

            // then
            actualAuditValidationException.Should().BeEquivalentTo(expectedAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAuditByIdAsync(invalidAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}