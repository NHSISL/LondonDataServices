// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Ontologies
{
    public partial class OntologyProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveAllConceptMapsIfUrlIsInvalidAndLogItAsync(
            string invalidText)
        {
            string invalidFileName = invalidText;

            var invalidArgumentOntologyProcessingException =
                new InvalidArgumentOntologyProcessingException(
                    message: "Invalid ontology processing arguments. Please correct the error and try again.");

            invalidArgumentOntologyProcessingException.AddData(
                key: "relativeUrl",
                values: "Text is required");

            var expectedOntologyValidationException =
                new OntologyProcessingValidationException(
                    message: "Ontology processing validation error occurred, please try again.",
                    innerException: invalidArgumentOntologyProcessingException);

            // when
            ValueTask<OntologyAssets> retrieveOntologyByRelativeUrlTask =
                this.ontologyProcessingService.RetrieveAllConceptMapsAsync(invalidFileName);

            OntologyProcessingValidationException actualOntologyValidationException =
                await Assert.ThrowsAsync<OntologyProcessingValidationException>(
                    retrieveOntologyByRelativeUrlTask.AsTask);

            // then
            actualOntologyValidationException.Should()
                .BeEquivalentTo(expectedOntologyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOntologyValidationException))),
                        Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}