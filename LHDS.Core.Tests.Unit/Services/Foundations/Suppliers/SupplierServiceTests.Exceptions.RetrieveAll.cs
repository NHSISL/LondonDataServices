// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Suppliers
{
    public partial class SupplierServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
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
                broker.SelectAllSuppliers())
                    .Throws(sqlException);

            // when
            Action retrieveAllSuppliersAction = () =>
                this.supplierService.RetrieveAllSuppliers();

            SupplierDependencyException actualSupplierDependencyException =
                Assert.Throws<SupplierDependencyException>(retrieveAllSuppliersAction);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliers(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
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
                broker.SelectAllSuppliers())
                    .Throws(serviceException);

            // when
            Action retrieveAllSuppliersAction = () =>
                this.supplierService.RetrieveAllSuppliers();

            SupplierServiceException actualSupplierServiceException =
                Assert.Throws<SupplierServiceException>(retrieveAllSuppliersAction);

            // then
            actualSupplierServiceException.Should()
                .BeEquivalentTo(expectedSupplierServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliers(),
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