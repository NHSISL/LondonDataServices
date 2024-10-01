// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SpecificationObject> retrieveSpecificationObjectByIdTask =
                this.specificationObjectService.RetrieveSpecificationObjectByIdAsync(someId);

            SpecificationObjectDependencyException actualSpecificationObjectDependencyException =
                await Assert.ThrowsAsync<SpecificationObjectDependencyException>(
                    retrieveSpecificationObjectByIdTask.AsTask);

            // then
            actualSpecificationObjectDependencyException.Should()
                .BeEquivalentTo(expectedSpecificationObjectDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSpecificationObjectDependencyException))),
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

            var failedSpecificationObjectServiceException =
                new FailedSpecificationObjectServiceException(
                    message: "Failed specificationObject service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSpecificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, please contact support.",
                    innerException: failedSpecificationObjectServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SpecificationObject> retrieveSpecificationObjectByIdTask =
                this.specificationObjectService.RetrieveSpecificationObjectByIdAsync(someId);

            SpecificationObjectServiceException actualSpecificationObjectServiceException =
                await Assert.ThrowsAsync<SpecificationObjectServiceException>(
                    retrieveSpecificationObjectByIdTask.AsTask);

            // then
            actualSpecificationObjectServiceException.Should()
                .BeEquivalentTo(expectedSpecificationObjectServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSpecificationObjectByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSpecificationObjectServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}