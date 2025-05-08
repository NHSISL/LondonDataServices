// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldThrowValidationExceptionOnModifyIfSupplierIsNullAndLogItAsync()
        {
            // given
            Supplier nullSupplier = null;
            var nullSupplierException = new NullSupplierException(message: "Supplier is null.");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: nullSupplierException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(nullSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfSupplierIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

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
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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
                values: "Date is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values: "Text is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values:
                    [
                        "Date is required",
                        "Date is the same as CreatedDate",
                        $"Date is not recent"
                    ]);

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedBy),
                values:
                    [
                        "Text is required",
                        $"Expected value to be '{randomEntraUser.EntraUserId}' but found " +
                        $"'{invalidSupplier.UpdatedBy}'."
                    ]);

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            //then
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
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSupplierIsInvalidLengthAndLogItAsync()
        {
            // given 
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: GetRandomStringWithLengthOf(256));

            Supplier invalidSupplier =
                CreateRandomModifySupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            var inputCreatedByUpdatedByString = randomEntraUser.EntraUserId;
            invalidSupplier.CreatedBy = inputCreatedByUpdatedByString;
            invalidSupplier.UpdatedBy = inputCreatedByUpdatedByString;

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            //then
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
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(It.IsAny<Supplier>()),
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

            Supplier randomSupplier =
                CreateRandomSupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier;

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is the same as {nameof(Supplier.CreatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

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
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
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

            Supplier invalidSupplier =
                CreateRandomSupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            invalidSupplier.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: "Date is not recent");

            var expectedSupplierValidatonException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSupplierDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            Supplier invalidSupplier =
                CreateRandomModifySupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier nonExistSupplier = invalidSupplier;
            var notFoundSupplierException = new NotFoundSupplierException(nonExistSupplier.Id);

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: notFoundSupplierException);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            // when 
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(nonExistSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(nonExistSupplier.Id),
                    Times.Once);

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

            Supplier randomSupplier =
                CreateRandomModifySupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier.DeepClone();
            Supplier storageSupplier = invalidSupplier.DeepClone();
            storageSupplier.CreatedDate = storageSupplier.CreatedDate.AddMinutes(randomMinutes);
            storageSupplier.UpdatedDate = storageSupplier.UpdatedDate.AddMinutes(randomMinutes);

            var supplierServiceMock = new Mock<SupplierService>(
                storageBrokerMock.Object,
                dateTimeBrokerMock.Object,
                securityBrokerMock.Object,
                loggingBrokerMock.Object)
            {
                CallBase = true
            };

            supplierServiceMock.Setup(service =>
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedDate),
                values: $"Date is not the same as {nameof(Supplier.CreatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id))
                    .ReturnsAsync(storageSupplier);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSupplierValidationException))),
                       Times.Once);

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

            Supplier randomSupplier =
                CreateRandomModifySupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier.DeepClone();
            Supplier storageSupplier = invalidSupplier.DeepClone();
            invalidSupplier.CreatedBy = Guid.NewGuid().ToString();
            storageSupplier.UpdatedDate = storageSupplier.CreatedDate;

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
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
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id))
                    .ReturnsAsync(storageSupplier);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should().BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSupplierValidationException))),
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

            Supplier randomSupplier =
                CreateRandomModifySupplier(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Supplier invalidSupplier = randomSupplier;
            Supplier storageSupplier = randomSupplier.DeepClone();
            invalidSupplier.UpdatedDate = storageSupplier.UpdatedDate;

            var invalidSupplierException =
                new InvalidSupplierException(
                    message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is the same as {nameof(Supplier.UpdatedDate)}");

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
                service.ApplyModifyAuditAsync(invalidSupplier))
                    .ReturnsAsync(invalidSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id))
                    .ReturnsAsync(storageSupplier);

            // when
            ValueTask<Supplier> modifySupplierTask =
                supplierServiceMock.Object.ModifySupplierAsync(invalidSupplier);

            // then
            await Assert.ThrowsAsync<SupplierValidationException>(
                modifySupplierTask.AsTask);

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
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}