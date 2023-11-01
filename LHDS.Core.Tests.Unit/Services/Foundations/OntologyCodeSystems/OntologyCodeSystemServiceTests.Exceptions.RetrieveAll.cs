using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedOntologyCodeSystemStorageException =
                new FailedOntologyCodeSystemStorageException(
                    message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedOntologyCodeSystemDependencyException =
                new OntologyCodeSystemDependencyException(
                    message: "OntologyCodeSystem dependency error occurred, contact support.",
                    innerException: failedOntologyCodeSystemStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOntologyCodeSystems())
                    .Throws(sqlException);

            // when
            Action retrieveAllOntologyCodeSystemsAction = () =>
                this.ontologyCodeSystemService.RetrieveAllOntologyCodeSystems();

            OntologyCodeSystemDependencyException actualOntologyCodeSystemDependencyException =
                Assert.Throws<OntologyCodeSystemDependencyException>(retrieveAllOntologyCodeSystemsAction);

            // then
            actualOntologyCodeSystemDependencyException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOntologyCodeSystems(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}