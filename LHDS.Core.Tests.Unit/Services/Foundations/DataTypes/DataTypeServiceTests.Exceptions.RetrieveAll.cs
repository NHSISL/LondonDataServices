// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
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
                broker.SelectAllDataTypes())
                    .Throws(sqlException);

            // when
            Action retrieveAllDataTypesAction = () =>
                this.dataTypeService.RetrieveAllDataTypes();

            DataTypeDependencyException actualDataTypeDependencyException =
                Assert.Throws<DataTypeDependencyException>(retrieveAllDataTypesAction);

            // then
            actualDataTypeDependencyException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataTypes(),
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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedDataTypeServiceException =
                new FailedDataTypeServiceException(
                    message: "Failed dataType service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataTypeServiceException =
                new DataTypeServiceException(
                    message: "DataType service error occurred, please contact support.",
                    innerException: failedDataTypeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataTypes())
                    .Throws(serviceException);

            // when
            Action retrieveAllDataTypesAction = () =>
                this.dataTypeService.RetrieveAllDataTypes();

            DataTypeServiceException actualDataTypeServiceException =
                Assert.Throws<DataTypeServiceException>(retrieveAllDataTypesAction);

            // then
            actualDataTypeServiceException.Should()
                .BeEquivalentTo(expectedDataTypeServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataTypes(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}