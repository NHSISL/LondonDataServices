// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using LHDS.Core.Services.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPdsAuditIsNullAndLogItAsync()
        {
            // given
            PdsAudit nullPdsAudit = null;

            var nullPdsAuditException =
                new NullPdsAuditException(message: "PdsAudit is null.");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: nullPdsAuditException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.AddPdsAuditAsync(nullPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfPdsAuditsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            var invalidPdsAudit = new PdsAudit
            {
                FileName = invalidText,
                Message = invalidText,
                MessageId = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Id),
                values: "Id is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CorrelationId),
                values: "Id is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.FileName),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Message),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.MessageId),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                 key: nameof(PdsAudit.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' but found '{invalidPdsAudit.CreatedBy}'."
                ]);

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: "Date is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedBy),
                values: "Text is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPdsAuditsIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(256);

            PdsAudit invalidPdsAudit =
                CreateRandomPdsAudit(randomDateTimeOffset, randomUserId);

            var inputCreatedByUpdatedByString = randomUserId;
            invalidPdsAudit.CreatedBy = inputCreatedByUpdatedByString;
            invalidPdsAudit.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: "Text is exceeding max length");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            PdsAudit randomPdsAudit =
                CreateRandomPdsAudit(randomDateTimeOffset, randomUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit;
            invalidPdsAudit.CreatedDate = GetRandomDateTimeOffset();
            invalidPdsAudit.UpdatedDate = GetRandomDateTimeOffset();

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is not the same as {nameof(PdsAudit.CreatedDate)}");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: $"Date is not recent");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            PdsAudit randomPdsAudit =
                CreateRandomPdsAudit(randomDateTimeOffset, randomUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit;
            invalidPdsAudit.CreatedBy = GetRandomString();
            invalidPdsAudit.UpdatedBy = GetRandomString();

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: $"Expected value to be '{randomUserId}' " +
                    $"but found '{invalidPdsAudit.CreatedBy}'.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedBy),
                values: $"Text is not the same as {nameof(PdsAudit.CreatedBy)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset invalidDateTime = randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            PdsAudit randomPdsAudit =
                CreateRandomPdsAudit(invalidDateTime, randomUserId);

            PdsAudit invalidPdsAudit = randomPdsAudit;

            var invalidPdsAuditException =
                new InvalidPdsAuditException(
                    message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: "Date is not recent");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit))
                    .ReturnsAsync(invalidPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                pdsAuditService.AddPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(addPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}