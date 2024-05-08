// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyArtifacts
{
    public partial class TerminologyArtifactProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedTerminologyArtifactProcessingDependencyValidationException =
                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Throws(dependencyValidationException);

            // when
            Action terminologyArtifactRetrieveAction = () =>
                this.terminologyArtifactProcessingService.RetrieveAllTerminologyArtifactsAsync();

            TerminologyArtifactProcessingDependencyValidationException actualException =
                Assert.Throws<TerminologyArtifactProcessingDependencyValidationException>(
                    terminologyArtifactRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyValidationException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactProcessingDependencyValidationException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedTerminologyArtifactProcessingDependencyException =
                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifacts())
                    .Throws(dependencyException);

            // when
            Action terminologyArtifactRetrieveAction = () =>
                this.terminologyArtifactProcessingService.RetrieveAllTerminologyArtifactsAsync();

            TerminologyArtifactProcessingDependencyException actualException =
                Assert.Throws<TerminologyArtifactProcessingDependencyException>(
                    terminologyArtifactRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactProcessingDependencyException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedTerminologyArtifactProcessingServiceException =
                new FailedTerminologyArtifactProcessingServiceException(
                    message: "Failed terminology artifact processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedTerminologyArtifactProcessingServiceException =
                new TerminologyArtifactProcessingServiceException(
                    message: "Terminology artifact processing service error occurred, please contact support.",
                    innerException: failedTerminologyArtifactProcessingServiceException);

            this.terminologyArtifactServiceMock.Setup(service =>
                 service.RetrieveAllTerminologyArtifacts())
                    .Throws(serviceException);

            // when
            Action terminologyArtifactRetrieveAction = () =>
                this.terminologyArtifactProcessingService.RetrieveAllTerminologyArtifactsAsync();

            TerminologyArtifactProcessingServiceException actualException =
                Assert.Throws<TerminologyArtifactProcessingServiceException>(
                    terminologyArtifactRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingServiceException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifacts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyArtifactProcessingServiceException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
