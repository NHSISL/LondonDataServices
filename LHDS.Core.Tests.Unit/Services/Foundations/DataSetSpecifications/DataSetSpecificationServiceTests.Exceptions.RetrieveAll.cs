// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedDataSetSpecificationStorageException =
                new FailedDataSetSpecificationStorageException(
                    message: "Failed dataSetSpecification storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedDataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: failedDataSetSpecificationStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetSpecificationsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<DataSetSpecification>> retrieveAllDataSetSpecificationsTask =
                this.dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync();

            DataSetSpecificationDependencyException actualDataSetSpecificationDependencyException =
                await Assert.ThrowsAsync<DataSetSpecificationDependencyException>(
                    retrieveAllDataSetSpecificationsTask.AsTask);

            // then
            actualDataSetSpecificationDependencyException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationDependencyException))),
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

            var failedDataSetSpecificationServiceException =
                new FailedDataSetSpecificationServiceException(
                    message: "Failed dataSetSpecification service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDataSetSpecificationServiceException =
                new DataSetSpecificationServiceException(
                    message: "DataSetSpecification service error occurred, please contact support.",
                    innerException: failedDataSetSpecificationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetSpecificationsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<DataSetSpecification>> retrieveAllDataSetSpecificationsTask =
                 this.dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync();

            DataSetSpecificationServiceException actualDataSetSpecificationServiceException =
                await Assert.ThrowsAsync<DataSetSpecificationServiceException>(
                    retrieveAllDataSetSpecificationsTask.AsTask);

            // then
            actualDataSetSpecificationServiceException.Should()
                .BeEquivalentTo(expectedDataSetSpecificationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataSetSpecificationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}