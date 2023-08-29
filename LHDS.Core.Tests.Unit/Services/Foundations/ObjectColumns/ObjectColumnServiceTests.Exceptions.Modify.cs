using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            SqlException sqlException = GetSqlException();

            var failedObjectColumnStorageException =
                new FailedObjectColumnStorageException(
                    message: "Failed objectColumn storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedObjectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, contact support.",
                    innerException: failedObjectColumnStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ObjectColumn> modifyObjectColumnTask =
                this.objectColumnService.ModifyObjectColumnAsync(randomObjectColumn);

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                await Assert.ThrowsAsync<ObjectColumnDependencyException>(
                    modifyObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}