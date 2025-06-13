// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingAuditIsNullAndLogItAsync()
        {
            // given
            IngestionTrackingAudit nullIngestionTrackingAudit = null;
            var nullIngestionTrackingAuditException = new NullIngestionTrackingAuditException(
                message: "Resolved address is null.");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: nullIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(nullIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingAuditIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidIngestionTrackingAudit = new IngestionTrackingAudit
            {
                Message = invalidText,
            };

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.Id),
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
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidIngestionTrackingAudit.UpdatedBy}'."
                    ]);

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            //then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateIngestionTrackingAuditAsync(It.IsAny<IngestionTrackingAudit>()),
                    Times.Never);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTrackingAudit.CreatedDate)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Never);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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

            IngestionTrackingAudit invalidIngestionTrackingAudit =
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidIngestionTrackingAudit.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: "Date is not recent");

            var expectedIngestionTrackingAuditValidatonException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidatonException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIngestionTrackingAuditDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            IngestionTrackingAudit invalidIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit nonExistIngestionTrackingAudit = invalidIngestionTrackingAudit;

            var notFoundIngestionTrackingAuditException = 
                new NotFoundIngestionTrackingAuditException(message: "Not found ingestion tracking audit.");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: notFoundIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    nonExistIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(nonExistIngestionTrackingAudit.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit storageIngestionTrackingAudit = invalidIngestionTrackingAudit.DeepClone();

            storageIngestionTrackingAudit.CreatedDate = 
                storageIngestionTrackingAudit.CreatedDate.AddMinutes(randomMinutes);

            storageIngestionTrackingAudit.UpdatedDate = 
                storageIngestionTrackingAudit.UpdatedDate.AddMinutes(randomMinutes);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit))
                        .ReturnsAsync(invalidIngestionTrackingAudit);

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedDate),
                values: $"Date is not the same as {nameof(IngestionTrackingAudit.CreatedDate)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditValidationException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingAuditValidationException))),
                       Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            IngestionTrackingAudit storageIngestionTrackingAudit = invalidIngestionTrackingAudit.DeepClone();
            invalidIngestionTrackingAudit.CreatedBy = Guid.NewGuid().ToString();
            storageIngestionTrackingAudit.UpdatedDate = storageIngestionTrackingAudit.CreatedDate;

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.CreatedBy),
                values: $"Text is not the same as {nameof(IngestionTrackingAudit.CreatedBy)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit))
                        .ReturnsAsync(invalidIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    invalidIngestionTrackingAudit);

            IngestionTrackingAuditValidationException actualIngestionTrackingAuditValidationException =
                await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                    modifyIngestionTrackingAuditTask.AsTask);

            // then
            actualIngestionTrackingAuditValidationException.Should().BeEquivalentTo(
                expectedIngestionTrackingAuditValidationException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Once);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIngestionTrackingAuditValidationException))),
                       Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
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

            IngestionTrackingAudit randomIngestionTrackingAudit =
                CreateRandomModifyIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit invalidIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = randomIngestionTrackingAudit.DeepClone();
            invalidIngestionTrackingAudit.UpdatedDate = storageIngestionTrackingAudit.UpdatedDate;

            var invalidIngestionTrackingAuditException =
                new InvalidIngestionTrackingAuditException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidIngestionTrackingAuditException.AddData(
                key: nameof(IngestionTrackingAudit.UpdatedDate),
                values: $"Date is the same as {nameof(IngestionTrackingAudit.UpdatedDate)}");

            var expectedIngestionTrackingAuditValidationException =
                new IngestionTrackingAuditValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidIngestionTrackingAuditException);

            var ingestionTrackingAuditServiceMock = new Mock<IngestionTrackingAuditService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit))
                    .ReturnsAsync(invalidIngestionTrackingAudit);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit))
                        .ReturnsAsync(invalidIngestionTrackingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // when
            ValueTask<IngestionTrackingAudit> modifyIngestionTrackingAuditTask =
                ingestionTrackingAuditServiceMock.Object.ModifyIngestionTrackingAuditAsync(
                    invalidIngestionTrackingAudit);

            // then
            await Assert.ThrowsAsync<IngestionTrackingAuditValidationException>(
                modifyIngestionTrackingAuditTask.AsTask);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.ApplyModifyAuditAsync(invalidIngestionTrackingAudit),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                    invalidIngestionTrackingAudit,
                    storageIngestionTrackingAudit),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIngestionTrackingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectIngestionTrackingAuditByIdAsync(invalidIngestionTrackingAudit.Id),
                    Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}