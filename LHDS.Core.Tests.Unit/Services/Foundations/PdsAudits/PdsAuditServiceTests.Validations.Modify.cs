// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditIsNullAndLogItAsync()
        {
            // given
            PdsAudit nullPdsAudit = null;
            var nullPdsAuditException = new NullPdsAuditException(message: "PdsAudit is null.");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: nullPdsAuditException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(nullPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidPdsAudit = new PdsAudit
            {
                FileName = invalidText,
                Message = invalidText
            };

            var invalidPdsAuditException = new InvalidPdsAuditException(
                message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Id),
                values: "Id is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.FileName),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Message),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: "Date is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: "Text is required");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(PdsAudit.CreatedDate)}"
                });

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedBy),
                values: "Text is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            //then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePdsAuditAsync(It.IsAny<PdsAudit>()),
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
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit;

            var invalidPdsAuditException = new InvalidPdsAuditException(
                message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Never);

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
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(randomDateTimeOffset);
            randomPdsAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidPdsAuditException =
                new InvalidPdsAuditException(message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedPdsAuditValidatonException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(randomPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPdsAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit nonExistPdsAudit = randomPdsAudit;
            PdsAudit nullPdsAudit = null;

            var notFoundPdsAuditException =
                new NotFoundPdsAuditException(nonExistPdsAudit.Id);

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: notFoundPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(nonExistPdsAudit.Id))
                .ReturnsAsync(nullPdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(nonExistPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(nonExistPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

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
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            PdsAudit storagePdsAudit = invalidPdsAudit.DeepClone();
            storagePdsAudit.CreatedDate = storagePdsAudit.CreatedDate.AddMinutes(randomMinutes);
            storagePdsAudit.UpdatedDate = storagePdsAudit.UpdatedDate.AddMinutes(randomMinutes);

            var invalidPdsAuditException = new InvalidPdsAuditException(
                message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedDate),
                values: $"Date is not the same as {nameof(PdsAudit.CreatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                .ReturnsAsync(storagePdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPdsAuditValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            PdsAudit storagePdsAudit = invalidPdsAudit.DeepClone();
            invalidPdsAudit.CreatedBy = Guid.NewGuid().ToString();
            storagePdsAudit.UpdatedDate = storagePdsAudit.CreatedDate;

            var invalidPdsAuditException = new InvalidPdsAuditException(
                message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.CreatedBy),
                values: $"Text is not the same as {nameof(PdsAudit.CreatedBy)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                .ReturnsAsync(storagePdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should().BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPdsAuditValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = randomPdsAudit.DeepClone();

            var invalidPdsAuditException = new InvalidPdsAuditException(
                message: "Invalid pdsAudit. Please correct the errors and try again.");

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.UpdatedDate),
                values: $"Date is the same as {nameof(PdsAudit.UpdatedDate)}");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: invalidPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id))
                .ReturnsAsync(storagePdsAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            // then
            await Assert.ThrowsAsync<PdsAuditValidationException>(
                modifyPdsAuditTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPdsAuditModifyIfInvalidLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            PdsAudit randomPdsAudit = CreateRandomModifyPdsAudit(randomDateTimeOffset);
            PdsAudit invalidPdsAudit = randomPdsAudit.DeepClone();
            invalidPdsAudit.CreatedBy = GetRandomString(256);
            invalidPdsAudit.UpdatedBy = invalidPdsAudit.CreatedBy;

            var invalidPdsAuditException = new InvalidPdsAuditException(
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<PdsAudit> modifyPdsAuditTask =
                this.pdsAuditService.ModifyPdsAuditAsync(invalidPdsAudit);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    modifyPdsAuditTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(invalidPdsAudit.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}