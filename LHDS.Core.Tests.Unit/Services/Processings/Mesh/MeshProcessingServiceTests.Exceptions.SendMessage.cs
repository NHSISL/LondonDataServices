// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldThrowDependencyValidationExceptionOnSendMessageIfDependencyValidationErrorOccursAndLogItAsync(
          Xeption dependencyValidationException)
        {
            // given
            string mailboxId = GetRandomString();
            string messageId = GetRandomString();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(mailboxId, messageId))
                    .Throws(dependencyValidationException);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusAsync(mailboxId, messageId))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<string> retrieveSendMessageTask =
                this.meshProcessingService.SendMessageAsync(mailboxId, messageId);

            MeshProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(retrieveSendMessageTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(messageId),
                    Times.Never);

            this.meshServiceMock.Verify(service =>
              service.RetrieveTrackingStatusAsync(mailboxId, messageId),
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyValidationException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnSendMessageIfDependencyErrorOccursAndLogItAsync(
         Xeption dependencyException)
        {
            // given
            string mailboxId = GetRandomString();
            string messageId = GetRandomString();

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
             service.RetrieveTrackingStatusAsync(mailboxId, messageId))
                 .Throws(dependencyException);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(messageId))
                    .Throws(dependencyException);

            // when
            ValueTask<string> retrieveMessageAndAcknowledgeTask =
                this.meshProcessingService.SendMessageAsync(mailboxId, messageId);

            MeshProcessingDependencyException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(retrieveMessageAndAcknowledgeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusAsync(mailboxId, messageId),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
               service.SendMessageAsync(messageId),
                   Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
