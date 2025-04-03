// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using LHDS.Core.Services.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;
            var nullOptOutException = new NullOptOutException(message: "OptOut is null.");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: nullOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(nullOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidOptOut = new OptOut
            {
                Status = invalidText,
                NhsNumber = invalidText,
            };

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.Id),
                values: "Id is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values:
                [
                    "Text is required",
                    "NHS Number invalid"
                ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: "Text is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: "Date is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: "Text is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidOptOut.UpdatedBy}'."
                    ]);

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            OptOut invalidOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidOptOut.NhsNumber = GetRandomString(11);
            invalidOptOut.Status = GetRandomString(51);
            invalidOptOut.CreatedBy = inputCreatedByUpdatedByString;
            invalidOptOut.UpdatedBy = inputCreatedByUpdatedByString;

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values: 
                [
                    "Text length should not be greater than 10", 
                    "NHS Number invalid"
                ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: $"Text length should not be greater than 50");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: "Text is exceeding max length");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut randomOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut;

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.CreatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut invalidOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidOptOut.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: "Date is not recent");

            var expectedOptOutValidatonException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut invalidOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut nonExistOptOut = invalidOptOut;

            var notFoundOptOutException = new NotFoundOptOutException(
                message: $"Couldn't find optOut with optOutId: {nonExistOptOut.Id}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: notFoundOptOutException);

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(nonExistOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(nonExistOptOut.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut.DeepClone();
            OptOut storageOptOut = invalidOptOut.DeepClone();
            storageOptOut.CreatedDate = storageOptOut.CreatedDate.AddMinutes(randomMinutes);
            storageOptOut.UpdatedDate = storageOptOut.UpdatedDate.AddMinutes(randomMinutes);

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: $"Date is not the same as {nameof(OptOut.CreatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedOptOutValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut.DeepClone();
            OptOut storageOptOut = invalidOptOut.DeepClone();
            invalidOptOut.CreatedBy = Guid.NewGuid().ToString();
            storageOptOut.UpdatedDate = storageOptOut.CreatedDate;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: $"Text is not the same as {nameof(OptOut.CreatedBy)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should().BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedOptOutValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            OptOut randomOptOut =
                CreateRandomModifyOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut;
            OptOut storageOptOut = randomOptOut.DeepClone();
            invalidOptOut.UpdatedDate = storageOptOut.UpdatedDate;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.UpdatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            var optOutServiceMock = new Mock<OptOutService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            optOutServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id))
                    .ReturnsAsync(storageOptOut);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                optOutServiceMock.Object.ModifyOptOutAsync(invalidOptOut);

            // then
            await Assert.ThrowsAsync<OptOutValidationException>(
                modifyOptOutTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}