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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ObjectColumn someObjectColumn = CreateRandomObjectColumn();
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
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnService.AddObjectColumnAsync(someObjectColumn);

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                await Assert.ThrowsAsync<ObjectColumnDependencyException>(
                    addObjectColumnTask.AsTask);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(It.IsAny<ObjectColumn>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}