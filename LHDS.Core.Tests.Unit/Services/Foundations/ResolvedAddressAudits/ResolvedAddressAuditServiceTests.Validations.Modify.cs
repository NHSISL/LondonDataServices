// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressAuditIsNullAndLogItAsync()
        {
            // given
            ResolvedAddressAudit nullResolvedAddressAudit = null;
            var nullResolvedAddressAuditException = new NullResolvedAddressAuditException(message: "ResolvedAddressAudit is null.");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: nullResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(nullResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
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
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

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
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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
                values: "Date is required");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedBy),
                values: "Text is required");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidResolvedAddressAudit.UpdatedBy}'."
                    ]);

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            //then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressAuditIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            ResolvedAddressAudit invalidResolvedAddressAudit =
                CreateRandomModifyResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidResolvedAddressAudit.CreatedBy = inputCreatedByUpdatedByString;
            invalidResolvedAddressAudit.UpdatedBy = inputCreatedByUpdatedByString;

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };
                        
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            //then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(It.IsAny<ResolvedAddressAudit>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit;

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedDate),
                values: $"Date is the same as {nameof(ResolvedAddressAudit.CreatedDate)}");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit invalidResolvedAddressAudit =
                CreateRandomResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidResolvedAddressAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedResolvedAddressAuditValidatonException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidatonException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddressAudit invalidResolvedAddressAudit =
                CreateRandomModifyResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit nonExistResolvedAddressAudit = invalidResolvedAddressAudit;
            var notFoundResolvedAddressAuditException = new NotFoundResolvedAddressAuditException(nonExistResolvedAddressAudit.Id);

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: notFoundResolvedAddressAuditException);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(nonExistResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(nonExistResolvedAddressAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomModifyResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit storageResolvedAddressAudit = invalidResolvedAddressAudit.DeepClone();
            storageResolvedAddressAudit.CreatedDate = storageResolvedAddressAudit.CreatedDate.AddMinutes(randomMinutes);
            storageResolvedAddressAudit.UpdatedDate = storageResolvedAddressAudit.UpdatedDate.AddMinutes(randomMinutes);

            var resolvedAddressAuditServiceMock = new Mock<ResolvedAddressAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            resolvedAddressAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedDate),
                values: $"Date is not the same as {nameof(ResolvedAddressAudit.CreatedDate)}");

            var expectedResolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressAuditValidationException))),
                       Times.Once);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomModifyResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit.DeepClone();
            ResolvedAddressAudit storageResolvedAddressAudit = invalidResolvedAddressAudit.DeepClone();
            invalidResolvedAddressAudit.CreatedBy = Guid.NewGuid().ToString();
            storageResolvedAddressAudit.UpdatedDate = storageResolvedAddressAudit.CreatedDate;

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.CreatedBy),
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
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            ResolvedAddressAuditValidationException actualResolvedAddressAuditValidationException =
                await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                    modifyResolvedAddressAuditTask.AsTask);

            // then
            actualResolvedAddressAuditValidationException.Should().BeEquivalentTo(expectedResolvedAddressAuditValidationException);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressAuditValidationException))),
                       Times.Once);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
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

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomModifyResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit invalidResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit storageResolvedAddressAudit = randomResolvedAddressAudit.DeepClone();
            invalidResolvedAddressAudit.UpdatedDate = storageResolvedAddressAudit.UpdatedDate;

            var invalidResolvedAddressAuditException =
                new InvalidResolvedAddressAuditException(
                    message: "Invalid resolvedAddressAudit. Please correct the errors and try again.");

            invalidResolvedAddressAuditException.AddData(
                key: nameof(ResolvedAddressAudit.UpdatedDate),
                values: $"Date is the same as {nameof(ResolvedAddressAudit.UpdatedDate)}");

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
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit))
                    .ReturnsAsync(invalidResolvedAddressAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ValueTask<ResolvedAddressAudit> modifyResolvedAddressAuditTask =
                resolvedAddressAuditServiceMock.Object.ModifyResolvedAddressAuditAsync(invalidResolvedAddressAudit);

            // then
            await Assert.ThrowsAsync<ResolvedAddressAuditValidationException>(
                modifyResolvedAddressAuditTask.AsTask);

            resolvedAddressAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(invalidResolvedAddressAudit.Id),
                    Times.Once);

            resolvedAddressAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}