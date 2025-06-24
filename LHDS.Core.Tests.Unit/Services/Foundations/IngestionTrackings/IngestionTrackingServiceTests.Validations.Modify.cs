// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingIsNullAndLogItAsync()
        {
            // given
            IngestionTracking nullIngestionTracking = null;
            var nullIngestionTrackingException = new NullIngestionTrackingException(
                message: "Ingestion tracking is null.");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: nullIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                this.ingestionTrackingService.ModifyIngestionTrackingAsync(nullIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidIngestionTracking = new IngestionTracking
            {
                FileName = invalidText,
                EncryptedFileName = invalidText,
                DecryptedFileName = invalidText,
            };

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.Id),
                values: "Id is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.FileName),
                values: "Text is required");


            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.SupplierId),
                values: "Id is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.EncryptedFileName),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.DecryptedFileName),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: "Date is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedBy),
                values: "Text is required");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        "Date is not recent"
                    ]);

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedBy),
                 values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                            $"'{invalidIngestionTracking.UpdatedBy}'."
                    ]);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            //then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAsync(It.IsAny<IngestionTracking>()),
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

            IngestionTracking randomIngestionTracking = 
                CreateRandomIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTracking.CreatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
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

            IngestionTracking randomIngestionTracking = 
                CreateRandomIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            randomIngestionTracking.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(randomIngestionTracking))
                    .ReturnsAsync(randomIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingException =
                new InvalidIngestionTrackingException(message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingValidatonException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(randomIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            IngestionTracking randomIngestionTracking = 
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTracking nonExistIngestionTracking = randomIngestionTracking;
            IngestionTracking nullIngestionTracking = null;

            var notFoundIngestionTrackingException =
                new NotFoundIngestionTrackingException(nonExistIngestionTracking.Id);

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(nonExistIngestionTracking))
                    .ReturnsAsync(nonExistIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: notFoundIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(nonExistIngestionTracking.Id))
                .ReturnsAsync(nullIngestionTracking);

            // when 
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(nonExistIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(nonExistIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
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

            IngestionTracking randomIngestionTracking = 
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking.DeepClone();
            IngestionTracking storageIngestionTracking = invalidIngestionTracking.DeepClone();
            storageIngestionTracking.CreatedDate = storageIngestionTracking.CreatedDate.AddMinutes(randomMinutes);
            storageIngestionTracking.UpdatedDate = storageIngestionTracking.UpdatedDate.AddMinutes(randomMinutes);

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedDate),
                values: $"Date is not the same as {nameof(IngestionTracking.CreatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            ingestionTrackingServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTracking, storageIngestionTracking))
                        .ReturnsAsync(invalidIngestionTracking);
            // when
            ValueTask <IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            ingestionTrackingServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTracking, storageIngestionTracking),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            IngestionTracking randomIngestionTracking = 
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking.DeepClone();
            IngestionTracking storageIngestionTracking = invalidIngestionTracking.DeepClone();
            invalidIngestionTracking.CreatedBy = Guid.NewGuid().ToString();
            storageIngestionTracking.UpdatedDate = storageIngestionTracking.CreatedDate;

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.CreatedBy),
                values: $"Text is not the same as {nameof(IngestionTracking.CreatedBy)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            ingestionTrackingServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTracking, storageIngestionTracking))
                        .ReturnsAsync(invalidIngestionTracking);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            IngestionTrackingValidationException actualIngestionTrackingValidationException =
                await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                    modifyIngestionTrackingTask.AsTask);

            // then
            actualIngestionTrackingValidationException.Should().BeEquivalentTo(expectedIngestionTrackingValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            ingestionTrackingServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTracking, storageIngestionTracking),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingValidationException))),
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

            IngestionTracking randomIngestionTracking = 
                CreateRandomModifyIngestionTracking(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTracking invalidIngestionTracking = randomIngestionTracking;
            IngestionTracking storageIngestionTracking = randomIngestionTracking.DeepClone();

            var ingestionTrackingServiceMock = new Mock<IngestionTrackingService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTracking))
                    .ReturnsAsync(invalidIngestionTracking);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

            invalidIngestionTrackingException.AddData(
                key: nameof(IngestionTracking.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTracking.UpdatedDate)}");

            var expectedIngestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: invalidIngestionTrackingException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id))
                .ReturnsAsync(storageIngestionTracking);

            // when
            ValueTask<IngestionTracking> modifyIngestionTrackingTask =
                ingestionTrackingServiceMock.Object.ModifyIngestionTrackingAsync(invalidIngestionTracking);

            // then
            await Assert.ThrowsAsync<IngestionTrackingValidationException>(
                modifyIngestionTrackingTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingByIdAsync(invalidIngestionTracking.Id),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}