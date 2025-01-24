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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidSupplierId = Guid.Empty;

            var invalidSupplierException =
                new InvalidSupplierException(message: "Invalid supplier. Please correct the errors and try again.");

            invalidSupplierException.AddData(
                key: nameof(Supplier.Id),
                values: "Id is required");

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: invalidSupplierException);

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
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfSupplierIsNotFoundAndLogItAsync()
        {
            //given
            Guid someSupplierId = Guid.NewGuid();
            Supplier noSupplier = null;

            var notFoundSupplierException =
                new NotFoundSupplierException(someSupplierId);

            var expectedSupplierValidationException =
                new SupplierValidationException(
                    message: "Supplier validation errors occurred, please try again.",
                    innerException: notFoundSupplierException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noSupplier);

            //when
            ValueTask<Supplier> retrieveSupplierByIdTask =
                this.supplierService.RetrieveSupplierByIdAsync(someSupplierId);

            SupplierValidationException actualSupplierValidationException =
                await Assert.ThrowsAsync<SupplierValidationException>(
                    retrieveSupplierByIdTask.AsTask);

            //then
            actualSupplierValidationException.Should().BeEquivalentTo(expectedSupplierValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSupplierValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}