// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllValueSetsIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string relativeUrl = GetRandomString();

            var expectedOntologyProcessingDependencyValidationException =
                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllValueSetsAsync(relativeUrl))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<OntologyAssets> ontologyRetrieveCodingSystemsAction =
                this.ontologyProcessingService.RetrieveAllValueSetsAsync(relativeUrl);

            OntologyProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OntologyProcessingDependencyValidationException>(
                    ontologyRetrieveCodingSystemsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingDependencyValidationException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllValueSetsAsync(relativeUrl),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOntologyProcessingDependencyValidationException))),
                         Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllValueSetsIfErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string relativeUrl = GetRandomString();

            var expectedOntologyProcessingDependencyException =
                new OntologyProcessingDependencyException(
                    message: "Ontology processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllValueSetsAsync(relativeUrl))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<OntologyAssets> ontologyRetrieveCodingSystemsAction =
                this.ontologyProcessingService.RetrieveAllValueSetsAsync(relativeUrl);

            OntologyProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OntologyProcessingDependencyException>(
                    ontologyRetrieveCodingSystemsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingDependencyException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllValueSetsAsync(relativeUrl),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOntologyProcessingDependencyException))),
                         Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllValueSetsIfServiceErrorOccursAsync()
        {
            // given
            string relativeUrl = GetRandomString();
            var serviceException = new Exception();

            var failedOntologyProcessingServiceException =
                new FailedOntologyProcessingServiceException(
                    message: "Failed ontology processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedOntologyProcessingServiceException =
                new OntologyProcessingServiceException(
                    message: "Ontology processing service error occurred, please contact support.",
                    innerException: failedOntologyProcessingServiceException);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveAllValueSetsAsync(relativeUrl))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<OntologyAssets> ontologyRetrieveCodingSystemsAction =
                this.ontologyProcessingService.RetrieveAllValueSetsAsync(relativeUrl);

            OntologyProcessingServiceException actualException =
                await Assert.ThrowsAsync<OntologyProcessingServiceException>(
                    ontologyRetrieveCodingSystemsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingServiceException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveAllValueSetsAsync(relativeUrl),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOntologyProcessingServiceException))),
                         Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
