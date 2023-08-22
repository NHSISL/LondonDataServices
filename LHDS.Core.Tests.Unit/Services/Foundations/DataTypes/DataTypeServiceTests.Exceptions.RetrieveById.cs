using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedDataTypeStorageException =
                new FailedDataTypeStorageException(
                    message: "Failed dataType storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, contact support.",
                    innerException: failedDataTypeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataType> retrieveDataTypeByIdTask =
                this.dataTypeService.RetrieveDataTypeByIdAsync(someId);

            DataTypeDependencyException actualDataTypeDependencyException =
                await Assert.ThrowsAsync<DataTypeDependencyException>(
                    retrieveDataTypeByIdTask.AsTask);

            // then
            actualDataTypeDependencyException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataTypeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}