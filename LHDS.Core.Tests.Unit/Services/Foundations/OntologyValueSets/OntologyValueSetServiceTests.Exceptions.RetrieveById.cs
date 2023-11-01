using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<OntologyValueSet> retrieveOntologyValueSetByIdTask =
                this.ontologyValueSetService.RetrieveOntologyValueSetByIdAsync(someId);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    retrieveOntologyValueSetByIdTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}