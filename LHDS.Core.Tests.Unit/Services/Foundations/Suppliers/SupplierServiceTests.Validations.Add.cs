using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
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
                new NullSupplierException();

            var expectedSupplierValidationException =
                new SupplierValidationException(nullSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(nullSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    addSupplierTask.AsTask);

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
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidSupplierException =
                new InvalidSupplierException();

            invalidSupplierException.AddData(
                key: nameof(Supplier.Id),
                values: "Id is required");

            //invalidSupplierException.AddData(
            //    key: nameof(Supplier.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Supplier model

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedDate),
                values: "Date is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedByUserId),
                values: "Id is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: "Date is required");

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedByUserId),
                values: "Id is required");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

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

            var invalidSupplierException = new InvalidSupplierException();

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is not the same as {nameof(Supplier.CreatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

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
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUserIdsIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Supplier randomSupplier = CreateRandomSupplier(randomDateTimeOffset);
            Supplier invalidSupplier = randomSupplier;
            invalidSupplier.UpdatedByUserId = Guid.NewGuid();

            var invalidSupplierException =
                new InvalidSupplierException();

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedByUserId),
                values: $"Id is not the same as {nameof(Supplier.CreatedByUserId)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

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
                new InvalidSupplierException();

            invalidSupplierException.AddData(
                key: nameof(Supplier.CreatedDate),
                values: "Date is not recent");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.AddSupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    addSupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
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