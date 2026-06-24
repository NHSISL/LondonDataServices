// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressToUprnFileLogIsNullAndLogItAsync()
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
                broker.ApplyModifyAuditValuesAsync(nullAddressToUprnFileLog))
                    .ReturnsAsync(nullAddressToUprnFileLog);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(nullAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(modifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullAddressToUprnFileLog),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressToUprnFileLogIsInvalidAndLogItAsync()
        {
            // given
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidAddressToUprnFileLog = new AddressToUprnFileLog();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidAddressToUprnFileLog))
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
                key: nameof(AddressToUprnFileLog.CreatedWhen),
                values: "Date is required");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.CreatedBy),
                values: "Text is required");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.UpdatedWhen),
                values:
                [
                    "Date is required",
                    $"Date is the same as {nameof(AddressToUprnFileLog.CreatedWhen)}",
                    "Date is not recent"
                ]);

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.UpdatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomUserId}' but found ''."
                ]);

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogException);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(invalidAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(modifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidAddressToUprnFileLog),
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
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfNotFoundAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomModifyAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog nonExistentAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog noAddressToUprnFileLog = null;

            var notFoundAddressToUprnFileLogException =
                new NotFoundAddressToUprnFileLogException(nonExistentAddressToUprnFileLog.Id);

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: notFoundAddressToUprnFileLogException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistentAddressToUprnFileLog))
                    .ReturnsAsync(nonExistentAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(nonExistentAddressToUprnFileLog.Id))
                    .ReturnsAsync(noAddressToUprnFileLog);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(nonExistentAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(modifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistentAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(nonExistentAddressToUprnFileLog.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedWhenNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomModifyAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog inputAddressToUprnFileLog = randomAddressToUprnFileLog;
            AddressToUprnFileLog storageAddressToUprnFileLog = inputAddressToUprnFileLog.DeepClone();
            storageAddressToUprnFileLog.CreatedWhen = GetRandomDateTimeOffset();
            storageAddressToUprnFileLog.UpdatedWhen = inputAddressToUprnFileLog.CreatedWhen;

            var invalidAddressToUprnFileLogException =
                new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again.");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.CreatedWhen),
                values: $"Date is not the same as {nameof(AddressToUprnFileLog.CreatedWhen)}");

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputAddressToUprnFileLog))
                    .ReturnsAsync(inputAddressToUprnFileLog);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id))
                    .ReturnsAsync(storageAddressToUprnFileLog);

            // when
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(inputAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(modifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputAddressToUprnFileLog),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(inputAddressToUprnFileLog.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressToUprnFileLogExceedsLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(256);

            AddressToUprnFileLog randomAddressToUprnFileLog =
                CreateRandomModifyAddressToUprnFileLog(randomDateTimeOffset, randomUserId);

            AddressToUprnFileLog invalidAddressToUprnFileLog = randomAddressToUprnFileLog;
            invalidAddressToUprnFileLog.FileName = GetRandomStringWithLengthOf(451);
            invalidAddressToUprnFileLog.CreatedBy = randomUserId;
            invalidAddressToUprnFileLog.UpdatedBy = randomUserId;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidAddressToUprnFileLog))
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
            ValueTask<AddressToUprnFileLog> modifyTask =
                this.addressToUprnFileLogService.ModifyAddressToUprnFileLogAsync(invalidAddressToUprnFileLog);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(modifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidAddressToUprnFileLog),
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
                broker.UpdateAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
