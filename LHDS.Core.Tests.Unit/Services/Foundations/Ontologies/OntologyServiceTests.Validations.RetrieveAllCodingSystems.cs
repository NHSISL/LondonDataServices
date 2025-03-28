// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Ontologies
{
    public partial class OntologyServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveAllCodingSystemsIfUrlIsInvalidAndLogItAsync(
            string invalidText)
        {
            string invalidFileName = invalidText;

            var invalidArgumentOntologyException =
                new InvalidArgumentOntologyException(
                    message: "Invalid ontology arguments. Please correct the error and try again.");

            invalidArgumentOntologyException.AddData(
                key: "relativeUrl",
                values: "Text is required");

            var expectedOntologyValidationException =
                new OntologyValidationException(
                    message: "Ontology validation error occurred, please try again.",
                    innerException: invalidArgumentOntologyException);

            // when
            ValueTask<OntologyAssets> retrieveOntologyByRelativeUrlTask =
                this.ontologyService.RetrieveAllCodingSystemsAsync(invalidFileName);

            OntologyValidationException actualOntologyValidationException =
                await Assert.ThrowsAsync<OntologyValidationException>(
                    retrieveOntologyByRelativeUrlTask.AsTask);

            // then
            actualOntologyValidationException.Should()
                .BeEquivalentTo(expectedOntologyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOntologyValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ontologyBrokerMock.VerifyNoOtherCalls();
        }
    }
}