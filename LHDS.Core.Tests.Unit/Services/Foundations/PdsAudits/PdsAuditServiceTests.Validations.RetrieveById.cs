using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidPdsAuditId = Guid.Empty;

            var invalidPdsAuditException =
                new InvalidPdsAuditException();

            invalidPdsAuditException.AddData(
                key: nameof(PdsAudit.Id),
                values: "Id is required");

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(invalidPdsAuditException);

            // when
            ValueTask<PdsAudit> retrievePdsAuditByIdTask =
                this.pdsAuditService.RetrievePdsAuditByIdAsync(invalidPdsAuditId);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    retrievePdsAuditByIdTask.AsTask);

            // then
            actualPdsAuditValidationException.Should()
                .BeEquivalentTo(expectedPdsAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfPdsAuditIsNotFoundAndLogItAsync()
        {
            //given
            Guid somePdsAuditId = Guid.NewGuid();
            PdsAudit noPdsAudit = null;

            var notFoundPdsAuditException =
                new NotFoundPdsAuditException(somePdsAuditId);

            var expectedPdsAuditValidationException =
                new PdsAuditValidationException(notFoundPdsAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noPdsAudit);

            //when
            ValueTask<PdsAudit> retrievePdsAuditByIdTask =
                this.pdsAuditService.RetrievePdsAuditByIdAsync(somePdsAuditId);

            PdsAuditValidationException actualPdsAuditValidationException =
                await Assert.ThrowsAsync<PdsAuditValidationException>(
                    retrievePdsAuditByIdTask.AsTask);

            //then
            actualPdsAuditValidationException.Should().BeEquivalentTo(expectedPdsAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}