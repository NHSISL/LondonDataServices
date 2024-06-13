// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdIfErrorOccursAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedTerminologyArtifactProcessingDependencyValidationException =
                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<TerminologyArtifact> terminologyArtifactRetrieveByIdTask =
                this.terminologyArtifactProcessingService.RetrieveTerminologyArtifactByIdAsync(someId);

            TerminologyArtifactProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingDependencyValidationException>(
                    terminologyArtifactRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyValidationException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedTerminologyArtifactProcessingDependencyException =
                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId))
                    .Throws(dependencyException);

            // when
            ValueTask<TerminologyArtifact> terminologyArtifactRetrieveByIdTask =
                this.terminologyArtifactProcessingService.RetrieveTerminologyArtifactByIdAsync(someId);

            TerminologyArtifactProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingDependencyException>(terminologyArtifactRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyArtifactProcessingDependencyException))),
                         Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();

            var serviceException = new Exception();

            var failedTerminologyArtifactProcessingServiceException =
               new FailedTerminologyArtifactProcessingServiceException(
                    message: "Failed terminology artifact processing service error occurred, please contact support.",
                   innerException: serviceException);

            var expectedTerminologyArtifactProcessingServiveException =
                new TerminologyArtifactProcessingServiceException(
                    message: "Terminology artifact processing service error occurred, please contact support.",
                    innerException: failedTerminologyArtifactProcessingServiceException);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId))
                    .Throws(serviceException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactProcessingService.RetrieveTerminologyArtifactByIdAsync(someId);

            TerminologyArtifactProcessingServiceException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingServiceException>(addTerminologyArtifactTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingServiveException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveTerminologyArtifactByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactProcessingServiveException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
