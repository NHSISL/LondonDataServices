// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedTerminologyArtifactStorageException =
                new FailedTerminologyArtifactStorageException(
                    message: "Failed terminologyArtifact storage error occurred, please contact support.",
                    innerException: sqlException);

            var expectedTerminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: failedTerminologyArtifactStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifacts())
                    .Throws(sqlException);

            // when
            Action retrieveAllTerminologyArtifactsAction = () =>
                this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                Assert.Throws<TerminologyArtifactDependencyException>(retrieveAllTerminologyArtifactsAction);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifacts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
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

            var failedTerminologyArtifactServiceException =
                new FailedTerminologyArtifactServiceException(
                    message: "Failed terminologyArtifact service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyArtifactServiceException =
                new TerminologyArtifactServiceException(
                    message: "TerminologyArtifact service error occurred, please contact support.",
                    innerException: failedTerminologyArtifactServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifacts())
                    .Throws(serviceException);

            // when
            Action retrieveAllTerminologyArtifactsAction = () =>
                this.terminologyArtifactService.RetrieveAllTerminologyArtifacts();

            TerminologyArtifactServiceException actualTerminologyArtifactServiceException =
                Assert.Throws<TerminologyArtifactServiceException>(retrieveAllTerminologyArtifactsAction);

            // then
            actualTerminologyArtifactServiceException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifacts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}