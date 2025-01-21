// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(nullResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

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

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressIsInvalidAndLogItAsync()
        {
            // given 
            var invalidResolvedAddress = new ResolvedAddress();

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
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(ResolvedAddress.CreatedDate)}"
                });

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.UpdatedBy),
                values: "Text is required");

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(It.IsAny<ResolvedAddress>()),
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
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;

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
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
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
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            randomResolvedAddress.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

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
                this.resolvedAddressService.ModifyResolvedAddressAsync(randomResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfResolvedAddressDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomModifyResolvedAddress(randomDateTimeOffset);
            ResolvedAddress nonExistResolvedAddress = randomResolvedAddress;
            ResolvedAddress nullResolvedAddress = null;

            var notFoundResolvedAddressException =
                new NotFoundResolvedAddressException(nonExistResolvedAddress.Id);

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: notFoundResolvedAddressException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(nonExistResolvedAddress.Id))
                .ReturnsAsync(nullResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when 
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(nonExistResolvedAddress);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(nonExistResolvedAddress.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
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
            ResolvedAddress randomResolvedAddress = CreateRandomModifyResolvedAddress(randomDateTimeOffset);
            ResolvedAddress invalidResolvedAddress = randomResolvedAddress.DeepClone();
            ResolvedAddress storageResolvedAddress = invalidResolvedAddress.DeepClone();
            storageResolvedAddress.CreatedDate = storageResolvedAddress.CreatedDate.AddMinutes(randomMinutes);
            storageResolvedAddress.UpdatedDate = storageResolvedAddress.UpdatedDate.AddMinutes(randomMinutes);

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
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomModifyResolvedAddress(randomDateTimeOffset);
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id))
                .ReturnsAsync(storageResolvedAddress);

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
            actualResolvedAddressValidationException.Should().BeEquivalentTo(expectedResolvedAddressValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedResolvedAddressValidationException))),
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
            ResolvedAddress randomResolvedAddress = CreateRandomModifyResolvedAddress(randomDateTimeOffset);
            ResolvedAddress invalidResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = randomResolvedAddress.DeepClone();

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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id))
                .ReturnsAsync(storageResolvedAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(invalidResolvedAddress);

            // then
            await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                modifyResolvedAddressTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(invalidResolvedAddress.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}