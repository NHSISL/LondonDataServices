// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveArtifactDetailsIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string relativeUrl = GetRandomString();

            var expectedOntologyProcessingDependencyValidationException =
                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveArtifactDetailsAsync(relativeUrl))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<string> ontologyRetrieveArtifactDetailsAction =
                this.ontologyProcessingService.RetrieveArtifactDetailsAsync(relativeUrl);

            OntologyProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<OntologyProcessingDependencyValidationException>(
                    ontologyRetrieveArtifactDetailsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingDependencyValidationException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(relativeUrl),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveArtifactDetailsIfErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string relativeUrl = GetRandomString();

            var expectedOntologyProcessingDependencyException =
                new OntologyProcessingDependencyException(
                    message: "Ontology processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ontologyServiceMock.Setup(service =>
                service.RetrieveArtifactDetailsAsync(relativeUrl))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<string> ontologyRetrieveArtifactDetailsAction =
                this.ontologyProcessingService.RetrieveArtifactDetailsAsync(relativeUrl);

            OntologyProcessingDependencyException actualException =
                await Assert.ThrowsAsync<OntologyProcessingDependencyException>(
                    ontologyRetrieveArtifactDetailsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingDependencyException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(relativeUrl),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedOntologyProcessingDependencyException))),
                         Times.Once);

            this.ontologyServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveArtifactDetailsIfServiceErrorOccursAsync()
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
                service.RetrieveArtifactDetailsAsync(relativeUrl))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> ontologyRetrieveArtifactDetailsAction =
                this.ontologyProcessingService.RetrieveArtifactDetailsAsync(relativeUrl);

            OntologyProcessingServiceException actualException =
                await Assert.ThrowsAsync<OntologyProcessingServiceException>(
                    ontologyRetrieveArtifactDetailsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedOntologyProcessingServiceException);

            this.ontologyServiceMock.Verify(service =>
                service.RetrieveArtifactDetailsAsync(relativeUrl),
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
