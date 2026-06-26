// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressToUprnFileLogIsNullAndLogItAsync()
        {
            // given
            AddressToUprnFileLog nullAddressToUprnFileLog = null;

            var nullAddressToUprnFileLogException =
                new NullAddressToUprnFileLogException(message: "Address to UPRN file log is null.");

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: nullAddressToUprnFileLogException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(nullAddressToUprnFileLog))
                    .ReturnsAsync(nullAddressToUprnFileLog);

            // when
            ValueTask<AddressToUprnFileLog> addTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(nullAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(addTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(nullAddressToUprnFileLog),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressToUprnFileLogIsInvalidAndLogItAsync()
        {
            // given
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidAddressToUprnFileLog = new AddressToUprnFileLog();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidAddressToUprnFileLog))
                    .ReturnsAsync(invalidAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidAddressToUprnFileLogException =
                new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again.");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.Id),
                values: "Id is required");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.FileName),
                values: "Text is required");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.CreatedDate),
                values:
                [
                    "Date is required",
                    "Date is not recent"
                ]);

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' but found ''."
                ]);

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.UpdatedDate),
                values: "Date is required");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.UpdatedBy),
                values: "Text is required");

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogException);

            // when
            ValueTask<AddressToUprnFileLog> addTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(invalidAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(addTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog invalidAddressToUprnFileLog = randomAddressToUprnFileLog;
            DateTimeOffset shiftedDateTimeOffset = randomDateTimeOffset.AddMinutes(minutes);
            invalidAddressToUprnFileLog.CreatedDate = shiftedDateTimeOffset;
            invalidAddressToUprnFileLog.UpdatedDate = shiftedDateTimeOffset;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidAddressToUprnFileLog))
                    .ReturnsAsync(invalidAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidAddressToUprnFileLogException =
                new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again.");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.CreatedDate),
                values: "Date is not recent");

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogException);

            // when
            ValueTask<AddressToUprnFileLog> addTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(invalidAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(addTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressToUprnFileLogExceedsLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(256);

            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog invalidAddressToUprnFileLog = randomAddressToUprnFileLog;
            invalidAddressToUprnFileLog.FileName = GetRandomStringWithLengthOf(451);
            invalidAddressToUprnFileLog.CreatedBy = randomUserId;
            invalidAddressToUprnFileLog.UpdatedBy = randomUserId;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(invalidAddressToUprnFileLog))
                    .ReturnsAsync(invalidAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidAddressToUprnFileLogException =
                new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again.");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.FileName),
                values: "Text is exceeding max length");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.CreatedBy),
                values: "Text is exceeding max length");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogException);

            // when
            ValueTask<AddressToUprnFileLog> addTask =
                this.addressToUprnFileLogService.AddAddressToUprnFileLogAsync(invalidAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(addTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(invalidAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
