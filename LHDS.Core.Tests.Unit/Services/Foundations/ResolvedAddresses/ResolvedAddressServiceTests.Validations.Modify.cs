// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress nullResolvedAddress = null;
            var nullResolvedAddressException = new NullResolvedAddressException(message: "Resolved address is null.");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: nullResolvedAddressException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nullResolvedAddress))
                    .ThrowsAsync(nullResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(nullResolvedAddress),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
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
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidResolvedAddress = new ResolvedAddress
            {
                UnstructuredPostalAddress = invalidText,
            };

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.Id),
                values: "Id is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UniqueReference),
                values: "Id is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UnstructuredPostalAddress),
                values: "Text is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedDate),
                values: "Date is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedBy),
                values: "Text is required");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUserId}' but found " +
                        $"'{invalidResolvedAddress.UpdatedBy}'."
                    ]);

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            //then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: $"Date is the same as {nameof(ResolvedAddress.CreatedDate)}");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress invalidResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            invalidResolvedAddress.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: "Date is not recent");

            var expectedResolvedAddressValidatonException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidatonException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfHashGreaterThanMaxLengthAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddress invalidResolvedAddress =
                CreateRandomResolvedAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidResolvedAddress.HashedUnstructuredPostalAddress =
                GetRandomHexString(33).ToCharArray();

            this.securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUser.EntraUserId);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.HashedUnstructuredPostalAddress),
                values: "Char array length should not be greater than 32");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: "Date is the same as CreatedDate");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(service =>
                service.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress invalidResolvedAddress =
                CreateRandomModifyResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress nonExistResolvedAddress = invalidResolvedAddress;
            var notFoundResolvedAddressException = new NotFoundResolvedAddressException(nonExistResolvedAddress.Id);

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: notFoundResolvedAddressException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(nonExistResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when 
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(nonExistResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(nonExistResolvedAddress.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress =
                CreateRandomModifyResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress.DeepClone();
            ResolvedAddress storageResolvedAddress = invalidResolvedAddress.DeepClone();
            storageResolvedAddress.CreatedDate = storageResolvedAddress.CreatedDate.AddMinutes(randomMinutes);
            storageResolvedAddress.UpdatedDate = storageResolvedAddress.UpdatedDate.AddMinutes(randomMinutes);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidResolvedAddress,
                    storageResolvedAddress))
                        .ReturnsAsync(invalidResolvedAddress);

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedDate),
                values: $"Date is not the same as {nameof(ResolvedAddress.CreatedDate)}");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id))
                    .ReturnsAsync(storageResolvedAddress);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidResolvedAddress,
                    storageResolvedAddress),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressValidationException))),
                       Times.Once);

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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress =
                CreateRandomModifyResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress.DeepClone();
            ResolvedAddress storageResolvedAddress = invalidResolvedAddress.DeepClone();
            invalidResolvedAddress.CreatedBy = Guid.NewGuid().ToString();
            storageResolvedAddress.UpdatedDate = storageResolvedAddress.CreatedDate;

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.CreatedBy),
                values: $"Text is not the same as {nameof(ResolvedAddress.CreatedBy)}");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidResolvedAddress,
                    storageResolvedAddress))
                        .ReturnsAsync(invalidResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id))
                    .ReturnsAsync(storageResolvedAddress);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should().BeEquivalentTo(expectedResolvedAddressValidationException);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidResolvedAddress,
                    storageResolvedAddress),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressValidationException))),
                       Times.Once);

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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);

            ResolvedAddress randomResolvedAddress =
                CreateRandomModifyResolvedAddress(randomDateTimeOffset, randomEntraUserId);

            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = randomResolvedAddress.DeepClone();
            invalidResolvedAddress.UpdatedDate = storageResolvedAddress.UpdatedDate;

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedDate),
                values: $"Date is the same as {nameof(ResolvedAddress.UpdatedDate)}");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress))
                    .ReturnsAsync(invalidResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidResolvedAddress,
                    storageResolvedAddress))
                        .ReturnsAsync(invalidResolvedAddress);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id))
                    .ReturnsAsync(storageResolvedAddress);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            // then
            await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                modifyResolvedAddressTask.AsTask);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(invalidResolvedAddress),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    invalidResolvedAddress,
                    storageResolvedAddress),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}