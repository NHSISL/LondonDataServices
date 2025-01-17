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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyOrAddIfErrorOccursAndLogItAsync(
           Xeption dependencyValidationException)
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = someTerminologyArtifact;

            var expectedTerminologyArtifactProcessingDependencyValidationException =
                new TerminologyArtifactProcessingDependencyValidationException(
                    message: "Terminology artifact processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<TerminologyArtifact> terminologyArtifactModifyOrAddTask =
                this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(inputTerminologyArtifact);

            TerminologyArtifactProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingDependencyValidationException>(
                    terminologyArtifactModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyValidationException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
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
        public async Task ShouldThrowDependencyExceptionOnModifyOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = someTerminologyArtifact;

            var expectedTerminologyArtifactProcessingDependencyException =
                new TerminologyArtifactProcessingDependencyException(
                    message: "Terminology artifact processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.terminologyArtifactServiceMock.Setup(service =>
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<TerminologyArtifact> terminologyArtifactModifyOrAddTask =
                this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(inputTerminologyArtifact);

            TerminologyArtifactProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingDependencyException>(
                    terminologyArtifactModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingDependencyException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedTerminologyArtifactProcessingDependencyException))),
                        Times.Once);

            this.terminologyArtifactServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyOrAddIfServiceErrorOccursAsync()
        {
            // given
            TerminologyArtifact someTerminologyArtifact = CreateRandomTerminologyArtifact();
            TerminologyArtifact inputTerminologyArtifact = someTerminologyArtifact;

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
                service.RetrieveAllTerminologyArtifactsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactProcessingService.ModifyOrAddTerminologyArtifactAsync(inputTerminologyArtifact);

            TerminologyArtifactProcessingServiceException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingServiceException>(addTerminologyArtifactTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTerminologyArtifactProcessingServiveException);

            this.terminologyArtifactServiceMock.Verify(service =>
                service.RetrieveAllTerminologyArtifactsAsync(),
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
