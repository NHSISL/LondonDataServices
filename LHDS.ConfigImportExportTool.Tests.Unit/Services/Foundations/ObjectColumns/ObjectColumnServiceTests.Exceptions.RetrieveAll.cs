// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedObjectColumnStorageException =
                new FailedObjectColumnStorageException(
                    message: "Failed objectColumn storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedObjectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: failedObjectColumnStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllObjectColumnsAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<ObjectColumn>> retrieveAllObjectColumnsTask =
                this.objectColumnService.RetrieveAllObjectColumnsAsync();

            ObjectColumnDependencyException actualObjectColumnDependencyException =
                await Assert.ThrowsAsync<ObjectColumnDependencyException>(retrieveAllObjectColumnsTask.AsTask);

            // then
            actualObjectColumnDependencyException.Should()
                .BeEquivalentTo(expectedObjectColumnDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllObjectColumnsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnDependencyException))),
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

            var failedObjectColumnServiceException =
                new FailedObjectColumnServiceException(
                    message: "Failed objectColumn service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedObjectColumnServiceException =
                new ObjectColumnServiceException(
                    message: "ObjectColumn service error occurred, please contact support.",
                    innerException: failedObjectColumnServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllObjectColumnsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<ObjectColumn>> retrieveAllObjectColumnsTask =
                this.objectColumnService.RetrieveAllObjectColumnsAsync();

            ObjectColumnServiceException actualObjectColumnServiceException =
                await Assert.ThrowsAsync<ObjectColumnServiceException>(retrieveAllObjectColumnsTask.AsTask);

            // then
            actualObjectColumnServiceException.Should()
                .BeEquivalentTo(expectedObjectColumnServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllObjectColumnsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedObjectColumnServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}