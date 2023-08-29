using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSet someDataSet = CreateRandomDataSet();
            SqlException sqlException = GetSqlException();

            var failedDataSetStorageException =
                new FailedDataSetStorageException(
                    message: "Failed dataSet storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, contact support.",
                    innerException: failedDataSetStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<DataSet> addDataSetTask =
                this.dataSetService.AddDataSetAsync(someDataSet);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    addDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(It.IsAny<DataSet>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}