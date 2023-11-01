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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            OntologyValueSet randomOntologyValueSet = CreateRandomOntologyValueSet();
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
                broker.SelectOntologyValueSetByIdAsync(randomOntologyValueSet.Id))
                    .Throws(sqlException);

            // when
            ValueTask<OntologyValueSet> addOntologyValueSetTask =
                this.ontologyValueSetService.RemoveOntologyValueSetByIdAsync(randomOntologyValueSet.Id);

            OntologyValueSetDependencyException actualOntologyValueSetDependencyException =
                await Assert.ThrowsAsync<OntologyValueSetDependencyException>(
                    addOntologyValueSetTask.AsTask);

            // then
            actualOntologyValueSetDependencyException.Should()
                .BeEquivalentTo(expectedOntologyValueSetDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyValueSetByIdAsync(randomOntologyValueSet.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOntologyValueSetDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyValueSetAsync(It.IsAny<OntologyValueSet>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}