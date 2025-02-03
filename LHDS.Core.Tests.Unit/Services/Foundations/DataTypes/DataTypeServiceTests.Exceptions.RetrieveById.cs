// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
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
                    message: "Failed dataType storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
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
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
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
                new FailedDataTypeServiceException(
                    message: "Failed dataType service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataTypeServiceException =
                new DataTypeServiceException(
                    message: "DataType service error occurred, please contact support.",
                    innerException: failedDataTypeServiceException);

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
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDataTypeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}