// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using LHDS.Core.Services.Foundations.TerminologyArtifacts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyArtifactIsNullAndLogItAsync()
        {
            // given
            TerminologyArtifact nullTerminologyArtifact = null;

            var nullTerminologyArtifactException =
                new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: nullTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.AddTerminologyArtifactAsync(nullTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyArtifactsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            var invalidTerminologyArtifact = new TerminologyArtifact
            {
                FullUrl = invalidText,
                ResourceType = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact))
                    .ReturnsAsync(invalidTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.Id),
                values: "Id is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.FullUrl),
                values: "Text is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.ResourceType),
                values: "Text is required");

            invalidTerminologyArtifactException.AddData(
                 key: nameof(TerminologyArtifact.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' but found '{invalidTerminologyArtifact.CreatedBy}'."
                ]);

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: "Date is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedBy),
                values: "Text is required");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                terminologyArtifactService.AddTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomTerminologyArtifact(randomDateTimeOffset, randomUserId);

            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact;
            invalidTerminologyArtifact.CreatedDate = GetRandomDateTimeOffset();
            invalidTerminologyArtifact.UpdatedDate = GetRandomDateTimeOffset();

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: $"Date is not the same as {nameof(TerminologyArtifact.CreatedDate)}");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedDate),
                values: $"Date is not recent");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact))
                    .ReturnsAsync(invalidTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                terminologyArtifactService.AddTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomTerminologyArtifact(randomDateTimeOffset, randomUserId);

            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact;
            invalidTerminologyArtifact.CreatedBy = GetRandomString();
            invalidTerminologyArtifact.UpdatedBy = GetRandomString();

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedBy),
                values: $"Expected value to be '{randomUserId}' " +
                    $"but found '{invalidTerminologyArtifact.CreatedBy}'.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedBy),
                values: $"Text is not the same as {nameof(TerminologyArtifact.CreatedBy)}");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact))
                    .ReturnsAsync(invalidTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                terminologyArtifactService.AddTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

            TerminologyArtifact randomTerminologyArtifact =
                CreateRandomTerminologyArtifact(invalidDateTime, randomUserId);

            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact;

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedDate),
                values: "Date is not recent");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact))
                    .ReturnsAsync(invalidTerminologyArtifact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                terminologyArtifactService.AddTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidTerminologyArtifact),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}