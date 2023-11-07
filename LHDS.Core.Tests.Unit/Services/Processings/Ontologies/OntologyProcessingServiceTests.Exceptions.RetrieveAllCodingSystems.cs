// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Ontologies
{
    public partial class OntologyProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllCodingSystensIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string relativeUrl = GetRandomString();

            var expectedOntologyProcessingDependencyValidationException =
                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllCodingSystemsAsync(relativeUrl))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<OntologyAssets> ontologyRetrieveCodingSystemsAction =
                this.ontologyProcessingService.RetrieveAllCodingSystemsAsync(relativeUrl);

            OntologyProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OntologyProcessingDependencyValidationException>(
                    ontologyRetrieveCodingSystemsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingDependencyValidationException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllCodingSystemsAsync(relativeUrl),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOntologyProcessingDependencyValidationException))),
                         Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
