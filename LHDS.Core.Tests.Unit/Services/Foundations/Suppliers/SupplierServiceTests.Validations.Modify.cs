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
        public async Task ShouldThrowValidationExceptionOnModifyIfSupplierIsNullAndLogItAsync()
        {
            // given
            Supplier nullSupplier = null;
            var nullSupplierException = new NullSupplierException();

            var expectedSupplierValidationException =
                new SupplierValidationException(nullSupplierException);

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
                broker.GetCurrentDateTimeOffset(),
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
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidSupplierException = new InvalidSupplierException();

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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Supplier.CreatedDate)}"
                });

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedByUserId),
                values: "Id is required");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            //then
            actualSupplierValidationException.Should().BeEquivalentTo(expectedSupplierValidationException);

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
            var invalidSupplierException = new InvalidSupplierException();

            invalidSupplierException.AddData(
                key: nameof(Supplier.UpdatedDate),
                values: $"Date is the same as {nameof(Supplier.CreatedDate)}");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(invalidSupplier);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierValidationException.Should().BeEquivalentTo(expectedSupplierValidationException);

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
    }
}