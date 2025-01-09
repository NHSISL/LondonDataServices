// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedSupplierStorageException(
                    message: "Failed supplier storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(
                    message: "Supplier dependency error occurred, please contact support.",
                    innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliersAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<Supplier>> retrieveAllSuppliersTask =
                this.supplierService.RetrieveAllSuppliersAsync();

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    testCode: retrieveAllSuppliersAction.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliersAsync(),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedSupplierServiceException =
                new FailedSupplierServiceException(
                    message: "Failed supplier service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSupplierServiceException =
                new SupplierServiceException(
                    message: "Supplier service error occurred, please contact support.",
                    innerException: failedSupplierServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliersAsync())
                    .Throws(serviceException);

            // when
            ValueTask<IQueryable<Supplier>> retrieveAllSuppliersTask =
                this.supplierService.RetrieveAllSuppliersAsync();

            SupplierServiceException actualSupplierServiceException =
                await Assert.ThrowsAsync<SupplierServiceException>(
                    testCode: retrieveAllSuppliersAction.AsTask);

            // then
            actualSupplierServiceException.Should()
                .BeEquivalentTo(expectedSupplierServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliersAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSupplierServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}