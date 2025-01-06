// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
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
                broker.LogError(It.Is(SameExceptionAs(
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
        public async Task ShouldThrowValidationExceptionOnAddIfSupplierIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidSupplier = new Supplier
            {
                Name = invalidText,
                FriendlyName = invalidText,
            };

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
                key: nameof(Supplier.CreatedDate),
                values: "Date is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedBy),
                values: "Text is required");

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
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier;

            invalidSupplier.UpdatedDate =
                invalidSupplier.CreatedDate.AddDays(randomNumber);

            var invalidSupplierException = new InvalidSupplierException(
                message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is not the same as {nameof(Supplier.CreatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUserIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier;
            invalidSupplier.UpdatedBy = Guid.NewGuid().ToString();

            var invalidSupplierException =
                new InvalidSupplierException(message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedBy),
                values: $"Text is not the same as {nameof(Supplier.CreatedBy)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            Supplier randomSupplier = CreateRandomSupplier(invalidDateTime);
            Supplier invalidSupplier = randomSupplier;

            var invalidSupplierException =
                new InvalidSupplierException(message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedDate),
                values: "Date is not recent");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}