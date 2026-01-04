// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberAgreementIsNullAndLogItAsync()
        {
            // given
            SubscriberAgreement nullSubscriberAgreement = null;
            var nullSubscriberAgreementException = new NullSubscriberAgreementException(message: "SubscriberAgreement is null.");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: nullSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                this.subscriberAgreementService.ModifySubscriberAgreementAsync(nullSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullSubscriberAgreement),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberAgreementIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            string randomUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidSubscriberAgreement = new SubscriberAgreement
            {
                SupplierSharingAgreementShortName = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.Id),
                values: "Id is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierId),
                values: "Id is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
                values: "Text is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedDate),
                values: "Date is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values: "Text is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomUserId}' but found " +
                        $"'{invalidSubscriberAgreement.UpdatedBy}'."
                    ]);

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementService.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            //then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberAgreementIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(256);

            SubscriberAgreement invalidSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomUserId);

            var inputCreatedByUpdatedByString = randomUserId;
            invalidSubscriberAgreement.SupplierSharingAgreementShortName = GetRandomString(129);
            invalidSubscriberAgreement.CreatedBy = inputCreatedByUpdatedByString;
            invalidSubscriberAgreement.UpdatedBy = inputCreatedByUpdatedByString;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
                values: "Text exceeded length requirement");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values: "Text exceeded length requirement");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values: "Text exceeded length requirement");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementService.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            //then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement randomSubscriberAgreement =
                CreateRandomSubscriberAgreement(randomDateTimeOffset, randomUserId);

            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values: $"Date is the same as {nameof(SubscriberAgreement.CreatedDate)}");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementService.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement invalidSubscriberAgreement =
                CreateRandomSubscriberAgreement(randomDateTimeOffset, randomUserId);

            invalidSubscriberAgreement.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values: "Date is not recent");

            var expectedSubscriberAgreementValidatonException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementService.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSubscriberAgreementDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement invalidSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomUserId);

            SubscriberAgreement nonExistSubscriberAgreement = invalidSubscriberAgreement;
            var notFoundSubscriberAgreementException = new NotFoundSubscriberAgreementException(nonExistSubscriberAgreement.Id);

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: notFoundSubscriberAgreementException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            // when 
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementService.ModifySubscriberAgreementAsync(nonExistSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(nonExistSubscriberAgreement.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement randomSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomUserId);

            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement.DeepClone();
            SubscriberAgreement storageSubscriberAgreement = invalidSubscriberAgreement.DeepClone();
            storageSubscriberAgreement.CreatedDate = storageSubscriberAgreement.CreatedDate.AddMinutes(randomMinutes);
            storageSubscriberAgreement.UpdatedDate = storageSubscriberAgreement.UpdatedDate.AddMinutes(randomMinutes);

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedDate),
                values: $"Date is not the same as {nameof(SubscriberAgreement.CreatedDate)}");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id))
                    .ReturnsAsync(storageSubscriberAgreement);

            subscriberAgreementServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidSubscriberAgreement, storageSubscriberAgreement))
                        .ReturnsAsync(invalidSubscriberAgreement);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementServiceMock.Object.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id),
                    Times.Once);

            subscriberAgreementServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidSubscriberAgreement, storageSubscriberAgreement),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSubscriberAgreementValidationException))),
                       Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement randomSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomUserId);

            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement.DeepClone();
            SubscriberAgreement storageSubscriberAgreement = invalidSubscriberAgreement.DeepClone();
            invalidSubscriberAgreement.CreatedBy = Guid.NewGuid().ToString();
            storageSubscriberAgreement.UpdatedDate = storageSubscriberAgreement.CreatedDate;

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values: $"Text is not the same as {nameof(SubscriberAgreement.CreatedBy)}");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id))
                    .ReturnsAsync(storageSubscriberAgreement);

            subscriberAgreementServiceMock.Setup(service =>
               service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                   invalidSubscriberAgreement, storageSubscriberAgreement))
                       .ReturnsAsync(invalidSubscriberAgreement);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementServiceMock.Object.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    modifySubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should().BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
               broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                   Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id),
                    Times.Once);

            subscriberAgreementServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidSubscriberAgreement, storageSubscriberAgreement),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSubscriberAgreementValidationException))),
                       Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement randomSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomUserId);

            SubscriberAgreement invalidSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement.DeepClone();
            invalidSubscriberAgreement.UpdatedDate = storageSubscriberAgreement.UpdatedDate;

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values: $"Date is the same as {nameof(SubscriberAgreement.UpdatedDate)}");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            var subscriberAgreementServiceMock = new Mock<SubscriberAgreementService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityAuditBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement))
                    .ReturnsAsync(invalidSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id))
                    .ReturnsAsync(storageSubscriberAgreement);

            // when
            ValueTask<SubscriberAgreement> modifySubscriberAgreementTask =
                subscriberAgreementService.ModifySubscriberAgreementAsync(invalidSubscriberAgreement);

            // then
            await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                modifySubscriberAgreementTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidSubscriberAgreement),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(invalidSubscriberAgreement.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}