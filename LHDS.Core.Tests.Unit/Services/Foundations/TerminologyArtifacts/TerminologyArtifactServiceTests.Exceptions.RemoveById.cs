using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
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
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id))
                    .Throws(sqlException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.RemoveTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id);

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyException>(
                    addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyArtifactByIdAsync(randomTerminologyArtifact.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
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