// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
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
                broker.LogError(It.Is(SameExceptionAs(
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
            var invalidSupplier = new Supplier
            {
                Name = invalidText,
                FriendlyName = invalidText,
            };

            var invalidSupplierException = new InvalidSupplierException(
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
                key: nameof(Supplier.CreatedDate),
                values: "Date is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values: "Text is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Supplier.CreatedDate)}"
                });

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedBy),
                values: "Text is required");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            //then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(It.IsAny<Supplier>()),
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
            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier;

            var invalidSupplierException = new InvalidSupplierException(
                message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is the same as {nameof(Supplier.CreatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
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
            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            randomSupplier.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidSupplierException =
                new InvalidSupplierException(message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: "Date is not recent");

            var expectedSupplierValidatonException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(randomSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSupplierDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Supplier randomSupplier = CreateRandomModifySupplier(randomDateTimeOffset);
            Supplier nonExistSupplier = randomSupplier;
            Supplier nullSupplier = null;

            var notFoundSupplierException =
                new NotFoundSupplierException(nonExistSupplier.Id);

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    notFoundSupplierException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(nonExistSupplier.Id))
                .ReturnsAsync(nullSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(nonExistSupplier);

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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
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
            Supplier randomSupplier = CreateRandomModifySupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier.DeepClone();
            Supplier storageSupplier = invalidSupplier.DeepClone();
            storageSupplier.CreatedDate = storageSupplier.CreatedDate.AddMinutes(randomMinutes);
            storageSupplier.UpdatedDate = storageSupplier.UpdatedDate.AddMinutes(randomMinutes);

            var invalidSupplierException = new InvalidSupplierException(
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSupplierValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMatchStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Supplier randomSupplier = CreateRandomModifySupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier.DeepClone();
            Supplier storageSupplier = invalidSupplier.DeepClone();
            invalidSupplier.CreatedBy = Guid.NewGuid().ToString();
            storageSupplier.UpdatedDate = storageSupplier.CreatedDate;

            var invalidSupplierException = new InvalidSupplierException(
                message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values: $"Text is not the same as {nameof(Supplier.CreatedBy)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id))
                .ReturnsAsync(storageSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should().BeEquivalentTo(expectedSupplierValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSupplierValidationException))),
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
            Supplier randomSupplier = CreateRandomModifySupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier;
            Supplier storageSupplier = randomSupplier.DeepClone();

            var invalidSupplierException = new InvalidSupplierException(
                message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is the same as {nameof(Supplier.UpdatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id))
                .ReturnsAsync(storageSupplier);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            // then
            await Assert.ThrowsAsync<SupplierValidationException>(
                modifySupplierTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(invalidSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}