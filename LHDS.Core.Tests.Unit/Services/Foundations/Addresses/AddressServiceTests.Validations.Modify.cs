// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressIsNullAndLogItAsync()
        {
            // given
            Address nullAddress = null;
            var nullAddressException = new NullAddressException(message: "Address is null.");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: nullAddressException);

            // when
            ValueTask<Address> modifyAddressTask =
                this.addressService.ModifyAddressAsync(nullAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidAddress = new Address
            {
                UPRN = invalidText,
                USRN = invalidText,
                OrganisationName = invalidText,
                DepartmentName = invalidText,
                SubBuildingName = invalidText,
                BuildingName = invalidText,
                BuildingNumber = invalidText,
                DependentThoroughfare = invalidText,
                Thoroughfare = invalidText,
                DoubleDependentLocality = invalidText,
                DependentLocality = invalidText,
                PostTown = invalidText,
                PostCode = invalidText,
                CreatedBy = invalidText,
                UpdatedBy = invalidText,
            };

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.Id),
                values: "Id is required");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedDate),
                values: "Date is required");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedBy),
                values: "Text is required");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Address.CreatedDate)}",
                    "Date is not recent"
                });

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUserId}' but found '{invalidText}'."
                    ]);

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask<Address> modifyAddressTask = addressService.ModifyAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            //then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomAddress(randomDateTimeOffset, randomEntraUserId);
            Address invalidAddress = randomAddress;

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values: $"Date is the same as {nameof(Address.CreatedDate)}");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask<Address> modifyAddressTask =
                addressService.ModifyAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id),
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
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUserId);
            randomAddress.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values: "Date is not recent");

            var expectedAddressValidatonException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(randomAddress))
                    .ReturnsAsync(randomAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            // when
            ValueTask<Address> modifyAddressTask = addressService.ModifyAddressAsync(randomAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfAddressDoesNotExistAndLogItAsync()
        {
            // given
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUserId);
            Address nonExistAddress = randomAddress;
            Address nullAddress = null;

            var notFoundAddressException =
                new NotFoundAddressException(nonExistAddress.Id);

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: notFoundAddressException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(randomAddress))
                    .ReturnsAsync(randomAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(nonExistAddress.Id))
                .ReturnsAsync(nullAddress);

            // when 
            ValueTask<Address> modifyAddressTask = addressService.ModifyAddressAsync(nonExistAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.securityAuditBrokerMock.Verify(service =>
                service.ApplyModifyAuditValuesAsync(nonExistAddress),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(nonExistAddress.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUserId);
            Address invalidAddress = randomAddress.DeepClone();
            Address storageAddress = invalidAddress.DeepClone();
            storageAddress.CreatedDate = storageAddress.CreatedDate.AddMinutes(randomMinutes);
            storageAddress.UpdatedDate = storageAddress.UpdatedDate.AddMinutes(randomMinutes);

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedDate),
                values: $"Date is not the same as {nameof(Address.CreatedDate)}");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id))
                .ReturnsAsync(storageAddress);

            // when
            ValueTask<Address> modifyAddressTask = addressService.ModifyAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.securityAuditBrokerMock.Verify(service =>
                service.ApplyModifyAuditValuesAsync(invalidAddress),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAddressValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMatchStorageAndLogItAsync()
        {
            // given
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUserId);
            Address invalidAddress = randomAddress.DeepClone();
            Address storageAddress = invalidAddress.DeepClone();
            invalidAddress.CreatedBy = Guid.NewGuid().ToString();
            storageAddress.UpdatedDate = storageAddress.CreatedDate;

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedBy),
                values: $"Text is not the same as {nameof(Address.CreatedBy)}");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id))
                .ReturnsAsync(storageAddress);

            // when
            ValueTask<Address> modifyAddressTask = addressService.ModifyAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(
                    modifyAddressTask.AsTask);

            // then
            actualAddressValidationException.Should().BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAddressValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            string randomEntraUserId = GetRandomStringWithLengthOf(50);
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomModifyAddress(randomDateTimeOffset, randomEntraUserId);
            Address invalidAddress = randomAddress;
            Address storageAddress = randomAddress.DeepClone();

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values: $"Date is the same as {nameof(Address.UpdatedDate)}");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            securityAuditBrokerMock.Setup(service =>
                service.ApplyModifyAuditValuesAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetCurrentUserIdAsync())
                    .ReturnsAsync(randomEntraUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id))
                .ReturnsAsync(storageAddress);

            // when
            ValueTask<Address> modifyAddressTask = addressService.ModifyAddressAsync(invalidAddress);

            // then
            await Assert.ThrowsAsync<AddressValidationException>(
                modifyAddressTask.AsTask);

            this.securityAuditBrokerMock.Verify(securityAuditBrokerMock =>
                securityAuditBrokerMock.ApplyModifyAuditValuesAsync(invalidAddress),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetCurrentUserIdAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressByIdAsync(invalidAddress.Id),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}