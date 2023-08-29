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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
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
            ValueTask<DataSet> modifyDataSetTask =
                this.dataSetService.ModifyDataSetAsync(randomDataSet);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    modifyDataSetTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(randomDataSet.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(randomDataSet),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}