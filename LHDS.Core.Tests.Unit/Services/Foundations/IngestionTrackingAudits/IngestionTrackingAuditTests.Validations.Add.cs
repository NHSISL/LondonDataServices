// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingAuditIsNullAndLogItAsync()
        {
            // given
            IngestionTrackingAudit nullIngestionTrackingAudit = null;

            var nullIngestionTrackingAuditException =
                new NullIngestionTrackingAuditException(message: "IngestionTrackingAudit is null.");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: nullIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(nullIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingAuditsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidIngestionTrackingAudit = new IngestionTrackingAudit
            {
                Message = invalidText,
            };

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyAddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.Id),
                values: "Id is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.IngestionTrackingId),
                values: "Id is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.Message),
                values: "Text is required");

            invalidIngestionTrackingAuditException.AddData(
                 key: nameof(IngestionTrackingAudit.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidIngestionTrackingAudit.CreatedBy}'."
                ]);

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: "Date is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: "Text is required");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfIngestionTrackingAuditsIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            IngestionTrackingAudit invalidIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidIngestionTrackingAudit.CreatedBy = inputCreatedByUpdatedByString;
            invalidIngestionTrackingAudit.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: "Text is exceeding max length");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyAddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;
            invalidIngestionTrackingAudit.CreatedDate = GetRandomDateTimeOffset();
            invalidIngestionTrackingAudit.UpdatedDate = GetRandomDateTimeOffset();

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: $"Date is not the same as {nameof(IngestionTrackingAudit.CreatedDate)}");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedDate),
                values: $"Date is not recent");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyAddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;
            invalidIngestionTrackingAudit.CreatedBy = GetRandomString();
            invalidIngestionTrackingAudit.UpdatedBy = GetRandomString();

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidIngestionTrackingAudit.CreatedBy}'.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: $"Text is not the same as {nameof(IngestionTrackingAudit.CreatedBy)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyAddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset invalidDateTime = randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(invalidDateTime, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyAddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<IngestionTrackingAudit> addIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.AddIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    addIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}