using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            SqlException sqlException = GetSqlException();

            var failedSupplierStorageException =
                new FailedSupplierStorageException(sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(failedSupplierStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Supplier> addSupplierTask =
                this.supplierService.RemoveSupplierByIdAsync(randomSupplier.Id);

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    addSupplierTask.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSupplierDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someSupplierId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedSupplierException =
                new LockedSupplierException(databaseUpdateConcurrencyException);

            var expectedSupplierDependencyValidationException =
                new SupplierDependencyValidationException(lockedSupplierException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Supplier> removeSupplierByIdTask =
                this.supplierService.RemoveSupplierByIdAsync(someSupplierId);

            SupplierDependencyValidationException actualSupplierDependencyValidationException =
                await Assert.ThrowsAsync<SupplierDependencyValidationException>(
                    removeSupplierByIdTask.AsTask);

            // then
            actualSupplierDependencyValidationException.Should()
                .BeEquivalentTo(expectedSupplierDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSupplierAsync(It.IsAny<Supplier>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}