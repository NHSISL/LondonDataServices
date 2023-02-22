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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidSupplierId = Guid.Empty;

            var invalidSupplierException =
                new InvalidSupplierException();

            invalidSupplierException.AddData(
                key: nameof(Supplier.Id),
                values: "Id is required");

            var expectedSupplierValidationException =
                new SupplierValidationException(invalidSupplierException);

            // when
            ValueTask<Supplier> retrieveSupplierByIdTask =
                this.supplierService.RetrieveSupplierByIdAsync(invalidSupplierId);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    retrieveSupplierByIdTask.AsTask);

            // then
            actualSupplierValidationException.Should()
                .BeEquivalentTo(expectedSupplierValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}