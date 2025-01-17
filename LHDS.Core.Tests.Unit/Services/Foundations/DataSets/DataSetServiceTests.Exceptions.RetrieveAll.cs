// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedDataSetStorageException =
                new FailedDataSetStorageException(
                    message: "Failed dataSet storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: failedDataSetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<DataSet>> retrieveAllDataSetsTask =
                this.dataSetService.RetrieveAllDataSetsAsync();

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    retrieveAllDataSetsTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedDataSetServiceException =
                new FailedDataSetServiceException(
                    message: "Failed dataSet service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: failedDataSetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<DataSet>> retrieveAllDataSetsTask =
                this.dataSetService.RetrieveAllDataSetsAsync();

            DataSetServiceException actualDataSetServiceException =
                await Assert.ThrowsAsync<DataSetServiceException>(
                    retrieveAllDataSetsTask.AsTask);

            // then
            actualDataSetServiceException.Should()
                .BeEquivalentTo(expectedDataSetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDataSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}