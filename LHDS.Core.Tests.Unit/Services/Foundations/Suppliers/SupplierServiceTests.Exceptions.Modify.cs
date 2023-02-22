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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Supplier randomSupplier = CreateRandomSupplier();
            SqlException sqlException = GetSqlException();

            var failedSupplierStorageException =
                new FailedSupplierStorageException(sqlException);

            var expectedSupplierDependencyException =
                new SupplierDependencyException(failedSupplierStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Supplier> modifySupplierTask =
                this.supplierService.ModifySupplierAsync(randomSupplier);

            SupplierDependencyException actualSupplierDependencyException =
                await Assert.ThrowsAsync<SupplierDependencyException>(
                    modifySupplierTask.AsTask);

            // then
            actualSupplierDependencyException.Should()
                .BeEquivalentTo(expectedSupplierDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSupplierDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSupplierAsync(randomSupplier),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}