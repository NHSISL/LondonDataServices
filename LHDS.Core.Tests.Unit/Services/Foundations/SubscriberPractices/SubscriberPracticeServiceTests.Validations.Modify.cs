// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberPracticeIsNullAndLogItAsync()
        {
            // given
            SubscriberPractice nullSubscriberPractice = null;
            var nullSubscriberPracticeException = new NullSubscriberPracticeException(message: "SubscriberPractice is null.");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: nullSubscriberPracticeException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                this.subscriberPracticeService.ModifySubscriberPracticeAsync(nullSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
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
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberPracticeIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidSubscriberPractice = new SubscriberPractice
            {
                Name = invalidText,
                PracticeCode = invalidText,
            };

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.Id),
                values: "Id is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.SubscriberAgreementId),
                values: "Id is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.Name),
                values: "Text is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.PracticeCode),
                values: "Text is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedDate),
                values: "Date is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedBy),
                values: "Text is required");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidSubscriberPractice.UpdatedBy}'."
                    ]);

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            //then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(It.IsAny<SubscriberPractice>()),
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

            SubscriberPractice randomSubscriberPractice =
                CreateRandomSubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice;

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedDate),
                values: $"Date is the same as {nameof(SubscriberPractice.CreatedDate)}");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id),
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

            SubscriberPractice invalidSubscriberPractice =
                CreateRandomSubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidSubscriberPractice.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedDate),
                values: "Date is not recent");

            var expectedSubscriberPracticeValidatonException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberPracticeDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            SubscriberPractice invalidSubscriberPractice =
                CreateRandomModifySubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice nonExistSubscriberPractice = invalidSubscriberPractice;
            var notFoundSubscriberPracticeException = new NotFoundSubscriberPracticeException(nonExistSubscriberPractice.Id);

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: notFoundSubscriberPracticeException);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(nonExistSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(nonExistSubscriberPractice.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
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

            SubscriberPractice randomSubscriberPractice =
                CreateRandomModifySubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice.DeepClone();
            SubscriberPractice storageSubscriberPractice = invalidSubscriberPractice.DeepClone();
            storageSubscriberPractice.CreatedDate = storageSubscriberPractice.CreatedDate.AddMinutes(randomMinutes);
            storageSubscriberPractice.UpdatedDate = storageSubscriberPractice.UpdatedDate.AddMinutes(randomMinutes);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedDate),
                values: $"Date is not the same as {nameof(SubscriberPractice.CreatedDate)}");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id))
                    .ReturnsAsync(storageSubscriberPractice);

            subscriberPracticeServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidSubscriberPractice, storageSubscriberPractice))
                        .ReturnsAsync(invalidSubscriberPractice);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should()
                .BeEquivalentTo(expectedSubscriberPracticeValidationException);

            subscriberPracticeServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id),
                    Times.Once);

            subscriberPracticeServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidSubscriberPractice, storageSubscriberPractice),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSubscriberPracticeValidationException))),
                       Times.Once);

            subscriberPracticeServiceMock.VerifyNoOtherCalls();
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

            SubscriberPractice randomSubscriberPractice =
                CreateRandomModifySubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice.DeepClone();
            SubscriberPractice storageSubscriberPractice = invalidSubscriberPractice.DeepClone();
            invalidSubscriberPractice.CreatedBy = Guid.NewGuid().ToString();
            storageSubscriberPractice.UpdatedDate = storageSubscriberPractice.CreatedDate;

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.CreatedBy),
                values: $"Text is not the same as {nameof(SubscriberPractice.CreatedBy)}");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id))
                    .ReturnsAsync(storageSubscriberPractice);

            subscriberPracticeServiceMock.Setup(service =>
               service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                   invalidSubscriberPractice, storageSubscriberPractice))
                       .ReturnsAsync(invalidSubscriberPractice);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(invalidSubscriberPractice);

            SubscriberPracticeValidationException actualSubscriberPracticeValidationException =
                await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                    modifySubscriberPracticeTask.AsTask);

            // then
            actualSubscriberPracticeValidationException.Should().BeEquivalentTo(expectedSubscriberPracticeValidationException);

            subscriberPracticeServiceMock.Verify(service =>
               service.ApplyModifyAuditAsync(invalidSubscriberPractice),
                   Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id),
                    Times.Once);

            subscriberPracticeServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidSubscriberPractice, storageSubscriberPractice),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSubscriberPracticeValidationException))),
                       Times.Once);

            subscriberPracticeServiceMock.VerifyNoOtherCalls();
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

            SubscriberPractice randomSubscriberPractice =
                CreateRandomModifySubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice invalidSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice.DeepClone();
            invalidSubscriberPractice.UpdatedDate = storageSubscriberPractice.UpdatedDate;

            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            invalidSubscriberPracticeException.AddData(
                key: nameof(SubscriberPractice.UpdatedDate),
                values: $"Date is the same as {nameof(SubscriberPractice.UpdatedDate)}");

            var expectedSubscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: invalidSubscriberPracticeException);

            var subscriberPracticeServiceMock = new Mock<SubscriberPracticeService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            subscriberPracticeServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSubscriberPractice))
                    .ReturnsAsync(invalidSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id))
                    .ReturnsAsync(storageSubscriberPractice);

            // when
            ValueTask<SubscriberPractice> modifySubscriberPracticeTask =
                subscriberPracticeServiceMock.Object.ModifySubscriberPracticeAsync(invalidSubscriberPractice);

            // then
            await Assert.ThrowsAsync<SubscriberPracticeValidationException>(
                modifySubscriberPracticeTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberPracticeValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(invalidSubscriberPractice.Id),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}