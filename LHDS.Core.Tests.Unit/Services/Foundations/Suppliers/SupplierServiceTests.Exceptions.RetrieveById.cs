// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
            ValueTask<Supplier> retrieveSupplierByIdTask =
                this.supplierService.RetrieveSupplierByIdAsync(someId);

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    retrieveSupplierByIdTask.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSupplierDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
            ValueTask<Supplier> retrieveSupplierByIdTask =
                this.supplierService.RetrieveSupplierByIdAsync(someId);

            SupplierServiceException actualSupplierServiceException =
                await Assert.ThrowsAsync<SupplierServiceException>(
                    retrieveSupplierByIdTask.AsTask);

            // then
            actualSupplierServiceException.Should()
                .BeEquivalentTo(expectedSupplierServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSupplierServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}