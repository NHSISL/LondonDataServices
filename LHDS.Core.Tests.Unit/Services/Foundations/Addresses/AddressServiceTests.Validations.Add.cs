// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowValidationExceptionOnAddIfAddressIsNullAndLogItAsync()
        {
            // given
            Address nullAddress = null;

            var nullAddressException =
                new NullAddressException(message: "Address is null.");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: nullAddressException);

            // when
            ValueTask<Address> addAddressTask =
                this.addressService.AddAddressAsync(nullAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(addAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAddressIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            var invalidAddress = new Address
            {
                UPRN = invalidText,
                UPSN = invalidText,
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
            };

            var addressServiceMock = new Mock<AddressService>(
               storageBrokerMock.Object,
               dateTimeBrokerMock.Object,
               securityBrokerMock.Object,
               identifierBrokerMock.Object,
               loggingBrokerMock.Object,
               auditBrokerMock.Object)
            {
                CallBase = true
            };

            addressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.Id),
                values: "Id is required");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedDate),
                values:
                [
                    "Date is required",
                    $"Date is not recent"
                ]);

            invalidAddressException.AddData(
                key: nameof(Address.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidAddress.CreatedBy}'."
                ]);

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values: "Date is required");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedBy),
                values: "Text is required");


            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask<Address> addAddressTask =
                addressServiceMock.Object.AddAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(addAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            Address randomAddress = CreateRandomAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Address invalidAddress = randomAddress;

            invalidAddress.UpdatedDate =
                invalidAddress.CreatedDate.AddDays(randomNumber);

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedDate),
                values: $"Date is not the same as {nameof(Address.CreatedDate)}");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            var addressServiceMock = new Mock<AddressService>(
              storageBrokerMock.Object,
              dateTimeBrokerMock.Object,
              securityBrokerMock.Object,
              identifierBrokerMock.Object,
              loggingBrokerMock.Object,
              auditBrokerMock.Object)
            {
                CallBase = true
            };

            addressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Address> addAddressTask =
                addressServiceMock.Object.AddAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(addAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
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
            Address randomAddress = CreateRandomAddress(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Address invalidAddress = randomAddress;
            invalidAddress.UpdatedBy = Guid.NewGuid().ToString();

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.UpdatedBy),
                values: $"Text is not the same as {nameof(Address.CreatedBy)}");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            var addressServiceMock = new Mock<AddressService>(
              storageBrokerMock.Object,
              dateTimeBrokerMock.Object,
              securityBrokerMock.Object,
              identifierBrokerMock.Object,
              loggingBrokerMock.Object,
              auditBrokerMock.Object)
            {
                CallBase = true
            };

            addressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Address> addAddressTask =
                addressServiceMock.Object.AddAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(addAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
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
            Address randomAddress = CreateRandomAddress(invalidDateTime, randomEntraUser.EntraUserId);
            Address invalidAddress = randomAddress;

            var invalidAddressException =
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(Address.CreatedDate),
                values: "Date is not recent");

            var expectedAddressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            var addressServiceMock = new Mock<AddressService>(
              storageBrokerMock.Object,
              dateTimeBrokerMock.Object,
              securityBrokerMock.Object,
              identifierBrokerMock.Object,
              loggingBrokerMock.Object,
              auditBrokerMock.Object)
            {
                CallBase = true
            };

            addressServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidAddress))
                    .ReturnsAsync(invalidAddress);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Address> addAddressTask =
                addressServiceMock.Object.AddAddressAsync(invalidAddress);

            AddressValidationException actualAddressValidationException =
                await Assert.ThrowsAsync<AddressValidationException>(addAddressTask.AsTask);

            // then
            actualAddressValidationException.Should()
                .BeEquivalentTo(expectedAddressValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}