using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
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
                new FailedSupplierStorageException(sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(failedSupplierStorageException);

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
                broker.LogCritical(It.Is(SameExceptionAs(
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
                new FailedSupplierServiceException(serviceException);

            var expectedSupplierServiceException =
                new SupplierServiceException(failedSupplierServiceException);

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
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSupplierServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}