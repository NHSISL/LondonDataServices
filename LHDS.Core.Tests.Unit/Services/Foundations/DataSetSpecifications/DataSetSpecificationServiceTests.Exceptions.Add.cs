using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataSetSpecification someDataSetSpecification = CreateRandomDataSetSpecification();
            SqlException sqlException = GetSqlException();

            var failedDataSetSpecificationStorageException =
                new FailedDataSetSpecificationStorageException(
                    message: "Failed dataSetSpecification storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, contact support.",
                    innerException: failedDataSetSpecificationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<DataSetSpecification> addDataSetSpecificationTask =
                this.dataSetSpecificationService.AddDataSetSpecificationAsync(someDataSetSpecification);

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    addDataSetSpecificationTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetSpecificationAsync(It.IsAny<DataSetSpecification>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}