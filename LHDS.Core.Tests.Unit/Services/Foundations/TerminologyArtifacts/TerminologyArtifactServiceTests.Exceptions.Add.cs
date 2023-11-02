using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();
            SqlException sqlException = GetSqlException();

            var failedTerminologyArtifactStorageException =
                new FailedTerminologyArtifactStorageException(
                    message: "Failed terminologyArtifact storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedTerminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, contact support.",
                    innerException: failedTerminologyArtifactStorageException);             

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.AddTerminologyArtifactAsync(someTerminologyArtifact);

            TerminologyArtifactDependencyException actualTerminologyArtifactDependencyException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyException>(
                    addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactDependencyException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfTerminologyArtifactAlreadyExsitsAndLogItAsync()
        {
            // given
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact alreadyExistsTerminologyArtifact = randomTerminologyArtifact;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsTerminologyArtifactException =
                new AlreadyExistsTerminologyArtifactException(
                    message: "TerminologyArtifact with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedTerminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: alreadyExistsTerminologyArtifactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.AddTerminologyArtifactAsync(alreadyExistsTerminologyArtifact);

            // then
            TerminologyArtifactDependencyValidationException actualTerminologyArtifactDependencyValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactDependencyValidationException>(
                    addTerminologyArtifactTask.AsTask);

            actualTerminologyArtifactDependencyValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}