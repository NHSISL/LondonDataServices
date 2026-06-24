// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAuditIsNullAndLogItAsync()
        {
            // given
            IngestionTrackingAudit nullIngestionTrackingAudit = null;

            var nullIngestionTrackingAuditException =
                new NullIngestionTrackingAuditException(message: "IngestionTrackingAudit is null.");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: nullIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nullIngestionTrackingAudit))
                    .ReturnsAsync(nullIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(nullIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullIngestionTrackingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
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
            var invalidIngestionTrackingAudit = new IngestionTrackingAudit
            {
                Message = invalidText
            };

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
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
                values: "Date is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: "Text is required");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(IngestionTrackingAudit.CreatedDate)}"
                });

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values: "Text is required");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyAuditTask.AsTask);

            //then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
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
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomIngestionTrackingAudit(randomDateTimeOffset);
            IngestionTrackingAudit invalidAudit = randomIngestionTrackingAudit;

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTrackingAudit.CreatedDate)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidAudit))
                    .ReturnsAsync(invalidAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(invalidAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidAudit.Id),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset);

            randomIngestionTrackingAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingAuditValidatonException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(randomIngestionTrackingAudit))
                    .ReturnsAsync(randomIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(randomIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(randomIngestionTrackingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset);

            IngestionTrackingAudit nonExistIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit nullIngestionTrackingAudit = null;

            var notFoundAuditException =
                new NotFoundIngestionTrackingAuditException(
                    message: $"Couldn't find IngestionTrackingAudit with Id: {nonExistIngestionTrackingAudit.Id}.");

            var expectedAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: notFoundAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistIngestionTrackingAudit))
                    .ReturnsAsync(nonExistIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(nonExistIngestionTrackingAudit.Id))
                .ReturnsAsync(nullIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when 
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(nonExistIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistIngestionTrackingAudit),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(nonExistIngestionTrackingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAuditValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit storageIngestionTrackingAudit = invalidIngestionTrackingAudit.DeepClone();

            storageIngestionTrackingAudit.CreatedDate = storageIngestionTrackingAudit.CreatedDate
                .AddMinutes(randomMinutes);

            storageIngestionTrackingAudit.UpdatedDate = storageIngestionTrackingAudit.UpdatedDate
                .AddMinutes(randomMinutes);

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedDate),
                values: $"Date is not the same as {nameof(IngestionTrackingAudit.CreatedDate)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit))
                        .ReturnsAsync(invalidIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingAuditValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit storageIngestionTrackingAudit = invalidIngestionTrackingAudit.DeepClone();
            invalidIngestionTrackingAudit.CreatedBy = Guid.NewGuid().ToString();
            storageIngestionTrackingAudit.UpdatedDate = storageIngestionTrackingAudit.CreatedDate;

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: $"Text is not the same as {nameof(IngestionTrackingAudit.CreatedBy)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit))
                        .ReturnsAsync(invalidIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should().BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingAuditValidationException))),
                       Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset);
            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTrackingAudit.UpdatedDate)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "IngestionTrackingAudit validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id))
                .ReturnsAsync(storageIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit))
                        .ReturnsAsync(invalidIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            // then
            await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                modifyIngestionTrackingAuditTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedByAndUpdatedByIsInvalidLengthAndLogItAsync()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            IngestionTrackingAudit randomIngestionTrackingAudit = CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset);
            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            invalidIngestionTrackingAudit.CreatedBy = GetRandomString(256);
            invalidIngestionTrackingAudit.UpdatedBy = invalidIngestionTrackingAudit.CreatedBy;

            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
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

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidIngestionTrackingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}