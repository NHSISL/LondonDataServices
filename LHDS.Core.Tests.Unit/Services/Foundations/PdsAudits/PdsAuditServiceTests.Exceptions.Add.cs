using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            PdsAudit somePdsAudit = CreateRandomPdsAudit();
            SqlException sqlException = GetSqlException();

            var failedPdsAuditStorageException =
                new FailedPdsAuditStorageException(sqlException);

            var expectedPdsAuditDependencyException =
                new PdsAuditDependencyException(failedPdsAuditStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.AddPdsAuditAsync(somePdsAudit);

            PdsAuditDependencyException actualPdsAuditDependencyException =
                await Assert.ThrowsAsync<PdsAuditDependencyException>(
                    addPdsAuditTask.AsTask);

            // then
            actualPdsAuditDependencyException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfPdsAuditAlreadyExsitsAndLogItAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit alreadyExistsPdsAudit = randomPdsAudit;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsPdsAuditException =
                new AlreadyExistsPdsAuditException(duplicateKeyException);

            var expectedPdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(alreadyExistsPdsAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<PdsAudit> addPdsAuditTask =
                this.pdsAuditService.AddPdsAuditAsync(alreadyExistsPdsAudit);

            // then
            PdsAuditDependencyValidationException actualPdsAuditDependencyValidationException =
                await Assert.ThrowsAsync<PdsAuditDependencyValidationException>(
                    addPdsAuditTask.AsTask);

            actualPdsAuditDependencyValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}