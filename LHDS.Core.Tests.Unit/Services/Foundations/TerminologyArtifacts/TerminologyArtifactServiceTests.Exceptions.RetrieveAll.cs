using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
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
                    message: "Failed terminologyArtifact storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedTerminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, contact support.",
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
    }
}