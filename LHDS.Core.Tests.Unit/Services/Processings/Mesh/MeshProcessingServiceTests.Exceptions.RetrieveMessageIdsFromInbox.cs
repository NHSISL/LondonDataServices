// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveMessageIdsIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveMessageByIdsTask =
                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync();

            MeshProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(retrieveMessageByIdsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
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
        public async Task ShouldThrowDependencyOnRetrieveMessageIdsIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    message: "Mesh processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .Throws(dependencyException);

            // when
            ValueTask<List<string>> retrieveMessageByIdsTask =
                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync();

            MeshProcessingDependencyException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(retrieveMessageByIdsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshProcessingDependencyException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveMessageIdsIfServiceErrorOccursAsync()
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
                service.RetrieveMessageIdsFromInboxAsync())
                    .Throws(serviceException);

            // when
            ValueTask<List<string>> retrievRetrieveMessageIdsFromInboxTask =
                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync();

            MeshProcessingServiceException actualException =
                await Assert.ThrowsAsync<MeshProcessingServiceException>(retrievRetrieveMessageIdsFromInboxTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
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
