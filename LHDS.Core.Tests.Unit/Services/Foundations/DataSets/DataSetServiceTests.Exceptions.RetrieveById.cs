// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DataSet> retrieveDataSetByIdTask =
                this.dataSetService.RetrieveDataSetByIdAsync(someId);

            DataSetDependencyException actualDataSetDependencyException =
                await Assert.ThrowsAsync<DataSetDependencyException>(
                    retrieveDataSetByIdTask.AsTask);

            // then
            actualDataSetDependencyException.Should()
                .BeEquivalentTo(expectedDataSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDataSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedDataSetServiceException =
                new FailedDataSetServiceException(
                    message: "Failed dataSet service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: failedDataSetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DataSet> retrieveDataSetByIdTask =
                this.dataSetService.RetrieveDataSetByIdAsync(someId);

            DataSetServiceException actualDataSetServiceException =
                await Assert.ThrowsAsync<DataSetServiceException>(
                    retrieveDataSetByIdTask.AsTask);

            // then
            actualDataSetServiceException.Should()
                .BeEquivalentTo(expectedDataSetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDataSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}