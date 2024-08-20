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
        public async Task ShouldThrowDependencyValidationExceptionOnGetUserUndownloadedIfErrorOccursAndLogItAsync(
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
            ValueTask<TerminologyArtifact?> getUndownloadedTask =
                this.terminologyArtifactProcessingService.GetNonDownloadedUserArtifactAsync();

            TerminologyArtifactProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingDependencyValidationException>(
                    getUndownloadedTask.AsTask);

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
        public async Task ShouldThrowDependencyExceptionOnGetUserUndownloadedIfErrorOccursAndLogItAsync(
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
            ValueTask<TerminologyArtifact> getUndownloadedTask =
                this.terminologyArtifactProcessingService.GetNonDownloadedUserArtifactAsync();

            TerminologyArtifactProcessingDependencyException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingDependencyException>(
                    getUndownloadedTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnGetUserUndownloadedIfServiceErrorOccursAsync()
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
            ValueTask<TerminologyArtifact?> getUndownloadedTask =
                this.terminologyArtifactProcessingService.GetNonDownloadedUserArtifactAsync();

            TerminologyArtifactProcessingServiceException actualException =
                await Assert.ThrowsAsync<TerminologyArtifactProcessingServiceException>(
                    getUndownloadedTask.AsTask);

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
