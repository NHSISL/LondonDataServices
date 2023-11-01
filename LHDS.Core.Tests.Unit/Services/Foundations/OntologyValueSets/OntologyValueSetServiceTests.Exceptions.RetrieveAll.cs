using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedOntologyValueSetStorageException =
                new FailedOntologyValueSetStorageException(
                    message: "Failed ontologyValueSet storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyValueSetDependencyException =
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: failedOntologyValueSetStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyValueSets())
                    .Throws(sqlException);

            // when
            Action retrieveAllOntologyValueSetsAction = () =>
                this.ontologyValueSetService.RetrieveAllOntologyValueSets();

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                Assert.Throws<OntologyValueSetDependencyException>(retrieveAllOntologyValueSetsAction);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyValueSets(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
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

            var failedOntologyValueSetServiceException =
                new FailedOntologyValueSetServiceException(
                    message: "Failed ontologyValueSet service occurred, please contact support", 
                    innerException: serviceException);

            var expectedOntologyValueSetServiceException =
                new OntologyValueSetServiceException(
                    message: "OntologyValueSet service error occurred, contact support.",
                    innerException: failedOntologyValueSetServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyValueSets())
                    .Throws(serviceException);

            // when
            Action retrieveAllOntologyValueSetsAction = () =>
                this.ontologyValueSetService.RetrieveAllOntologyValueSets();

            OntologyValueSetServiceException actualOntologyValueSetServiceException =
                Assert.Throws<OntologyValueSetServiceException>(retrieveAllOntologyValueSetsAction);

            // then
            actualOntologyValueSetServiceException.Should()
                .BeEquivalentTo(expectedOntologyValueSetServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyValueSets(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyValueSetServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}