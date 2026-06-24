// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExOnValidateAccessIfDependencyValidationErrOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
              service.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()))
                  .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<bool> validateMailboxAccessTask =
               this.meshProcessingService.ValidateMailboxAccessAsync(
                   TestContext.Current.CancellationToken);

            MeshProcessingDependencyValidationException actualException =
               await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(validateMailboxAccessTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
               service.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyValidationException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnValidateAccessIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    message: "Mesh processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<bool> meshAddTask =
                this.meshProcessingService.ValidateMailboxAccessAsync(
                    TestContext.Current.CancellationToken);

            MeshProcessingDependencyException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(meshAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnalidateAccessIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedMeshProcessingServiceException =
                new FailedMeshProcessingServiceException(
                    message: "Failed mesh processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedMeshProcessingServiveException =
                new MeshProcessingServiceException(
                    message: "Mesh processing service error occurred, please contact support.",
                    innerException: failedMeshProcessingServiceException);

            this.meshServiceMock.Setup(service =>
                service.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> addValidateAccessTask =
                this.meshProcessingService.ValidateMailboxAccessAsync(
                    TestContext.Current.CancellationToken);

            MeshProcessingServiceException actualException =
                await Assert.ThrowsAsync<MeshProcessingServiceException>(addValidateAccessTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

            this.meshServiceMock.Verify(service =>
                service.ValidateMailboxAccessAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedMeshProcessingServiveException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
