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
                new FailedDataTypeStorageException(sqlException);

            var expectedDataTypeDependencyException =
                new DataTypeDependencyException(failedDataTypeStorageException);

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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDataTypeServiceException =
                new FailedDataTypeServiceException(serviceException);

            var expectedDataTypeServiceException =
                new DataTypeServiceException(failedDataTypeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataType> retrieveDataTypeByIdTask =
                this.dataTypeService.RetrieveDataTypeByIdAsync(someId);

            DataTypeServiceException actualDataTypeServiceException =
                await Assert.ThrowsAsync<DataTypeServiceException>(
                    retrieveDataTypeByIdTask.AsTask);

            // then
            actualDataTypeServiceException.Should()
                .BeEquivalentTo(expectedDataTypeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDataTypeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}