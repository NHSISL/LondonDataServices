using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OntologyCodeSystem> retrieveOntologyCodeSystemByIdTask =
                this.ontologyCodeSystemService.RetrieveOntologyCodeSystemByIdAsync(someId);

            OntologyCodeSystemDependencyException actualOntologyCodeSystemDependencyException =
                await Assert.ThrowsAsync<OntologyCodeSystemDependencyException>(
                    retrieveOntologyCodeSystemByIdTask.AsTask);

            // then
            actualOntologyCodeSystemDependencyException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
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