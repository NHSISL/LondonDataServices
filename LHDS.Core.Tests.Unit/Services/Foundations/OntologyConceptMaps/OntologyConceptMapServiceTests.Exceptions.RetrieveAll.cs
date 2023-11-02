using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedOntologyConceptMapStorageException =
                new FailedOntologyConceptMapStorageException(
                    message: "Failed ontologyConceptMap storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyConceptMapDependencyException =
                new OntologyConceptMapDependencyException(
                    message: "OntologyConceptMap dependency error occurred, contact support.",
                    innerException: failedOntologyConceptMapStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyConceptMaps())
                    .Throws(sqlException);

            // when
            Action retrieveAllOntologyConceptMapsAction = () =>
                this.ontologyConceptMapService.RetrieveAllOntologyConceptMaps();

            OntologyConceptMapDependencyException actualOntologyConceptMapDependencyException =
                Assert.Throws<OntologyConceptMapDependencyException>(retrieveAllOntologyConceptMapsAction);

            // then
            actualOntologyConceptMapDependencyException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyConceptMaps(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapDependencyException))),
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

            var failedOntologyConceptMapServiceException =
                new FailedOntologyConceptMapServiceException(
                    message: "Failed ontologyConceptMap service occurred, please contact support", 
                    innerException: serviceException);

            var expectedOntologyConceptMapServiceException =
                new OntologyConceptMapServiceException(
                    message: "OntologyConceptMap service error occurred, contact support.",
                    innerException: failedOntologyConceptMapServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyConceptMaps())
                    .Throws(serviceException);

            // when
            Action retrieveAllOntologyConceptMapsAction = () =>
                this.ontologyConceptMapService.RetrieveAllOntologyConceptMaps();

            OntologyConceptMapServiceException actualOntologyConceptMapServiceException =
                Assert.Throws<OntologyConceptMapServiceException>(retrieveAllOntologyConceptMapsAction);

            // then
            actualOntologyConceptMapServiceException.Should()
                .BeEquivalentTo(expectedOntologyConceptMapServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyConceptMaps(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyConceptMapServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}