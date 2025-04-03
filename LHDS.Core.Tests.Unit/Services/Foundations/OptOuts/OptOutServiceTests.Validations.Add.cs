// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;

            var nullOptOutException =
                new NullOptOutException(message: "OptOut is null.");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: nullOptOutException);

            // when
            ValueTask<OptOut> addOptOutTask =
                this.optOutService.AddOptOutAsync(nullOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidOptOut = new OptOut
            {
                NhsNumber = invalidText,
                Status = invalidText,
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
                service.ApplyAddOptOutAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

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
                key: nameof(OptOut.Status),
                values: "Text is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values: 
                [
                    
                    "Text is required", 
                    "NHS Number invalid"
                ]);

            invalidOptOutException.AddData(
                 key: nameof(OptOut.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' but found '{invalidOptOut.CreatedBy}'."
                ]);

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: "Date is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: "Text is required");

            var expectedOptOutValidationException =
                new OptOutValidationException(
                    message: "OptOut validation errors occurred, please try again.",
                    innerException: invalidOptOutException);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutServiceMock.Object.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfOptOutsIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            OptOut invalidOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidOptOut.NhsNumber = GetRandomString(11);
            invalidOptOut.Status = GetRandomString(51);
            invalidOptOut.CreatedBy = inputCreatedByUpdatedByString;
            invalidOptOut.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.NhsNumber),
                values: $"Text length should not be greater than 10");

            invalidOptOutException.AddData(
                key: nameof(OptOut.Status),
                values: "Text length should not be greater than 50");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
                values: "Text is exceeding max length");

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
                service.ApplyAddOptOutAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutServiceMock.Object.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
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

            OptOut randomOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut;
            invalidOptOut.CreatedDate = GetRandomDateTimeOffset();
            invalidOptOut.UpdatedDate = GetRandomDateTimeOffset();

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is not the same as {nameof(OptOut.CreatedDate)}");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: $"Date is not recent");

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
                service.ApplyAddOptOutAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutServiceMock.Object.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
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

            OptOut randomOptOut =
                CreateRandomOptOut(randomDateTimeOffset, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut;
            invalidOptOut.CreatedBy = GetRandomString();
            invalidOptOut.UpdatedBy = GetRandomString();

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedBy),
                values: $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidOptOut.CreatedBy}'.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedBy),
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
                service.ApplyAddOptOutAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutServiceMock.Object.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
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

            OptOut randomOptOut =
                CreateRandomOptOut(invalidDateTime, randomEntraUser.EntraUserId);

            OptOut invalidOptOut = randomOptOut;

            var invalidOptOutException =
                new InvalidOptOutException(
                    message: "Invalid optOut. Please correct the errors and try again.");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: "Date is not recent");

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
                service.ApplyAddOptOutAsync(invalidOptOut))
                    .ReturnsAsync(invalidOptOut);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<OptOut> addOptOutTask =
                optOutServiceMock.Object.AddOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(addOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}