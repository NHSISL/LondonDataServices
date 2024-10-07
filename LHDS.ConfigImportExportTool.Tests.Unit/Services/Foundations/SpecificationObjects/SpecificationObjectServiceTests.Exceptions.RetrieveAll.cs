// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedSpecificationObjectStorageException =
                new FailedSpecificationObjectStorageException(
                    message: "Failed specificationObject storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedSpecificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: failedSpecificationObjectStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSpecificationObjectsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<SpecificationObject>> retrieveAllSpecificationObjectsTask =
                this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    retrieveAllSpecificationObjectsTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSpecificationObjectsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
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

            var failedSpecificationObjectServiceException =
                new FailedSpecificationObjectServiceException(
                    message: "Failed specificationObject service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSpecificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, please contact support.",
                    innerException: failedSpecificationObjectServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSpecificationObjectsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<SpecificationObject>> retrieveAllSpecificationObjectsTask =
                this.specificationObjectService.RetrieveAllSpecificationObjectsAsync();

            SpecificationObjectServiceException actualSpecificationObjectServiceException =
                await Assert.ThrowsAsync<SpecificationObjectServiceException>(
                    retrieveAllSpecificationObjectsTask.AsTask);

            // then
            actualSpecificationObjectServiceException.Should()
                .BeEquivalentTo(expectedSpecificationObjectServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSpecificationObjectsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSpecificationObjectServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}