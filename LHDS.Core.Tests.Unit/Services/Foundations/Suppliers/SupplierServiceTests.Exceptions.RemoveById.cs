// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
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
                new FailedSupplierStorageException(
                    message: "Failed supplier storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(
                    message: "Supplier dependency error occurred, please contact support.",
                    innerException: failedSupplierStorageException);

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
                broker.GetCurrentDateTimeOffsetAsync(),
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
                new LockedSupplierException(
                    message: "Locked supplier record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedSupplierDependencyValidationException =
                new SupplierDependencyValidationException(
                    message: "Supplier dependency error occurred, please contact support.",
                    innerException: lockedSupplierException);

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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSupplierId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedSupplierStorageException =
                new FailedSupplierStorageException(
                    message: "Failed supplier storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(
                    message: "Supplier dependency error occurred, please contact support.",
                    innerException: failedSupplierStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Supplier> deleteSupplierTask =
                this.supplierService.RemoveSupplierByIdAsync(someSupplierId);

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    deleteSupplierTask.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSupplierDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someSupplierId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSupplierServiceException =
                new FailedSupplierServiceException(
                    message: "Failed supplier service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSupplierServiceException =
                new SupplierServiceException(
                    message: "Supplier service error occurred, please contact support.",
                    innerException: failedSupplierServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Supplier> removeSupplierByIdTask =
                this.supplierService.RemoveSupplierByIdAsync(someSupplierId);

            SupplierServiceException actualSupplierServiceException =
                await Assert.ThrowsAsync<SupplierServiceException>(
                    removeSupplierByIdTask.AsTask);

            // then
            actualSupplierServiceException.Should()
                .BeEquivalentTo(expectedSupplierServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}