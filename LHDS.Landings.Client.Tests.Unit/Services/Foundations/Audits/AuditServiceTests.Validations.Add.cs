// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAuditIsNullAndLogItAsync()
        {
            // given
            Audit nullAudit = null;

            var nullAuditException =
                new NullAuditException();

            var expectedAuditValidationException =
                new AuditValidationException(nullAuditException);

            // when
            ValueTask<Audit> addAuditTask =
                this.auditService.AddAuditAsync(nullAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(() =>
                    addAuditTask.AsTask());

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidAudit = new Audit
            {
                IngestionTrackingId = invalidText,
                Message = invalidText,
            };

            var invalidAuditException =
                new InvalidAuditException();

            invalidAuditException.AddData(
                key: nameof(Audit.Id),
                values: "Id is required");

            invalidAuditException.AddData(
                key: nameof(Audit.IngestionTrackingId),
                values: "Text is required");

            invalidAuditException.AddData(
                key: nameof(Audit.Message),
                values: "Text is required");

            invalidAuditException.AddData(
                key: nameof(Audit.CreatedDate),
                values: "Date is required");

            var expectedAuditValidationException =
                new AuditValidationException(invalidAuditException);

            // when
            ValueTask<Audit> addAuditTask =
                this.auditService.AddAuditAsync(invalidAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(() =>
                    addAuditTask.AsTask());

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAuditAsync(It.IsAny<Audit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            Audit randomAudit = CreateRandomAudit(invalidDateTime);
            Audit invalidAudit = randomAudit;

            var invalidAuditException =
                new InvalidAuditException();

            invalidAuditException.AddData(
                key: nameof(Audit.CreatedDate),
                values: "Date is not recent");

            var expectedAuditValidationException =
                new AuditValidationException(invalidAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Audit> addAuditTask =
                this.auditService.AddAuditAsync(invalidAudit);

            AuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<AuditValidationException>(() =>
                    addAuditTask.AsTask());

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAuditAsync(It.IsAny<Audit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}