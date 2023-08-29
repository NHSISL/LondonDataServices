using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSetObject someDataSetObject = CreateRandomDataSetObject();
            SqlException sqlException = GetSqlException();

            var failedDataSetObjectStorageException =
                new FailedDataSetObjectStorageException(
                    message: "Failed dataSetObject storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataSetObjectDependencyException =
                new DataSetObjectDependencyException(
                    message: "DataSetObject dependency error occurred, contact support.",
                    innerException: failedDataSetObjectStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<DataSetObject> addDataSetObjectTask =
                this.dataSetObjectService.AddDataSetObjectAsync(someDataSetObject);

            DataSetObjectDependencyException actualDataSetObjectDependencyException =
                await Assert.ThrowsAsync<DataSetObjectDependencyException>(
                    addDataSetObjectTask.AsTask);

            // then
            actualDataSetObjectDependencyException.Should()
                .BeEquivalentTo(expectedDataSetObjectDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(It.IsAny<DataSetObject>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetObjectDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}