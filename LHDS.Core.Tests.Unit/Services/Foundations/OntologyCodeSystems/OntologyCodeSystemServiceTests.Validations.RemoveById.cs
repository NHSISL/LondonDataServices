using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidOntologyCodeSystemId = Guid.Empty;

            var invalidOntologyCodeSystemException = 
                new InvalidOntologyCodeSystemException(
                    message: "Invalid ontologyCodeSystem. Please correct the errors and try again.");

            invalidOntologyCodeSystemException.AddData(
                key: nameof(OntologyCodeSystem.Id),
                values: "Id is required");

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: invalidOntologyCodeSystemException);

            // when
            ValueTask<OntologyCodeSystem> removeOntologyCodeSystemByIdTask =
                this.ontologyCodeSystemService.RemoveOntologyCodeSystemByIdAsync(invalidOntologyCodeSystemId);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    removeOntologyCodeSystemByIdTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOntologyCodeSystemAsync(It.IsAny<OntologyCodeSystem>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}