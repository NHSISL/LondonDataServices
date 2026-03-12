// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using LHDS.Core.Services.Foundations.Suppliers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSupplierIsNullAndLogItAsync()
        {
            // given
            Supplier nullSupplier = null;

            var nullSupplierException =
                new NullSupplierException(message: "Supplier is null.");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: nullSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(nullSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfSuppliersIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDataTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            var invalidSupplier = new Supplier
            {
                Name = invalidText,
                FriendlyName = invalidText,
                Description = invalidText,
            };

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDataTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.Id),
                values: "Id is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.Name),
                values: "Text is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.FriendlyName),
                values: "Text is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.Description),
                values: "Text is required");

            invalidSupplierException.AddData(
                 key: nameof(Supplier.CreatedDate),
                 values:
                 [
                    "Date is required",
                    $"Date is not recent"
                 ]);

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values:
                [
                    "Text is required",
                    $"Expected value to be '{randomEntraUser.EntraUserId}' but found '{invalidSupplier.CreatedBy}'."
                ]);

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: "Date is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedBy),
                values: "Text is required");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                supplierServiceMock.Object.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSuppliersIsInvalidLenghtAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            Supplier invalidSupplier =
                CreateRandomSupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidSupplier.CreatedBy = inputCreatedByUpdatedByString;
            invalidSupplier.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values: "Text is exceeding max length");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedBy),
                values: "Text is exceeding max length");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Supplier> addSupplierTask =
                supplierServiceMock.Object.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

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

            Supplier randomSupplier =
                CreateRandomSupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier;
            invalidSupplier.CreatedDate = GetRandomDateTimeOffset();
            invalidSupplier.UpdatedDate = GetRandomDateTimeOffset();

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is not the same as {nameof(Supplier.CreatedDate)}");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedDate),
                values: $"Date is not recent");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Supplier> addSupplierTask =
                supplierServiceMock.Object.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
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

            Supplier randomSupplier =
                CreateRandomSupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier;
            invalidSupplier.CreatedBy = GetRandomString();
            invalidSupplier.UpdatedBy = GetRandomString();

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values: $"Expected value to be '{randomEntraUser.EntraUserId}' " +
                    $"but found '{invalidSupplier.CreatedBy}'.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedBy),
                values: $"Text is not the same as {nameof(Supplier.CreatedBy)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Supplier> addSupplierTask =
                supplierServiceMock.Object.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
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

            Supplier randomSupplier =
                CreateRandomSupplier(invalidDateTime, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier;

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedDate),
                values: "Date is not recent");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyAddAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when
            ValueTask<Supplier> addSupplierTask =
                supplierServiceMock.Object.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}