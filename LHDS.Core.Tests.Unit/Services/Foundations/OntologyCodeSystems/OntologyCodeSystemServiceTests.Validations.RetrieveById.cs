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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidOntologyCodeSystemId = Guid.Empty;

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
            ValueTask<OntologyCodeSystem> retrieveOntologyCodeSystemByIdTask =
                this.ontologyCodeSystemService.RetrieveOntologyCodeSystemByIdAsync(invalidOntologyCodeSystemId);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    retrieveOntologyCodeSystemByIdTask.AsTask);

            // then
            actualOntologyCodeSystemValidationException.Should()
                .BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfOntologyCodeSystemIsNotFoundAndLogItAsync()
        {
            //given
            Guid someOntologyCodeSystemId = Guid.NewGuid();
            OntologyCodeSystem noOntologyCodeSystem = null;

            var notFoundOntologyCodeSystemException =
                new NotFoundOntologyCodeSystemException(someOntologyCodeSystemId);

            var expectedOntologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: notFoundOntologyCodeSystemException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noOntologyCodeSystem);

            //when
            ValueTask<OntologyCodeSystem> retrieveOntologyCodeSystemByIdTask =
                this.ontologyCodeSystemService.RetrieveOntologyCodeSystemByIdAsync(someOntologyCodeSystemId);

            OntologyCodeSystemValidationException actualOntologyCodeSystemValidationException =
                await Assert.ThrowsAsync<OntologyCodeSystemValidationException>(
                    retrieveOntologyCodeSystemByIdTask.AsTask);

            //then
            actualOntologyCodeSystemValidationException.Should().BeEquivalentTo(expectedOntologyCodeSystemValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOntologyCodeSystemByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOntologyCodeSystemValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}