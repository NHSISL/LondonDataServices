// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using LHDS.Core.Services.Foundations.ResolvedAddressAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfResolvedAddressAuditIsNullAndLogItAsync()
        {
            // given
            ResolvedAddressAudit nullResolvedAddressAudit = null;

            var nullResolvedAddressAuditException =
                new NullResolvedAddressAuditException(message: "ResolvedAddressAudit is null.");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: nullResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                this.resolvedAddressAuditService.AddResolvedAddressAuditAsync(nullResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfResolvedAddressAuditsIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidResolvedAddressAudit = new ResolvedAddressAudit
            {
                Message = invalidText,
            };

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.Id),
                values: "Id is required");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CorrelationId),
                values: "Id is required");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UniqueResolvedAddressReference),
                values: "Id is required");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.Message),
                values: "Text is required");

            invalidResolvedAddressAuditException.AddData(
                 key: nameof(ResolvedAddressAudit.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' but found '{invalidResolvedAddressAudit.CreatedBy}'."
                ]);

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedDate),
                values: "Date is required");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedBy),
                values: "Text is required");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.AddResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfResolvedAddressAuditsIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            ResolvedAddressAudit invalidResolvedAddressAudit =
                CreateRandomResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidResolvedAddressAudit.CreatedBy = inputCreatedByUpdatedByString;
            invalidResolvedAddressAudit.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedBy),
                values: "Text is exceeding max length");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.AddResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit;
            invalidResolvedAddressAudit.CreatedDate = GetRandomDateTimeOffset();
            invalidResolvedAddressAudit.UpdatedDate = GetRandomDateTimeOffset();

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedDate),
                values: $"Date is not the same as {nameof(ResolvedAddressAudit.CreatedDate)}");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedDate),
                values: $"Date is not recent");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.AddResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit;
            invalidResolvedAddressAudit.CreatedBy = GetRandomString();
            invalidResolvedAddressAudit.UpdatedBy = GetRandomString();

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedBy),
                values: $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidResolvedAddressAudit.CreatedBy}'.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedBy),
                values: $"Text is not the same as {nameof(ResolvedAddressAudit.CreatedBy)}");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.AddResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomResolvedAddressAudit(invalidDateTime, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit;

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedDate),
                values: "Date is not recent");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<ResolvedAddressAudit> addResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.AddResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(addResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyAddResolvedAddressAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}