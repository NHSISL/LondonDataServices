// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowOperationCanceledOnValidateMailboxAccessAndNotWrapItAsync()
        {
            // given
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshProcessingService.ValidateMailboxAccessAsync(
                    cancellationTokenSource.Token).AsTask());

            // then
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnRetrieveMessageIdsFromInboxAndNotWrapItAsync()
        {
            // given
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync(
                    cancellationTokenSource.Token).AsTask());

            // then
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnRetrieveAndAcknowledgeMessageByIdAndNotWrapItAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(
                    messageId: randomMessageId,
                    outputStream: new MemoryStream(),
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnRetrieveMessageByIdAndNotWrapItAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshProcessingService.RetrieveMessageByIdAsync(
                    messageId: randomMessageId,
                    outputStream: new MemoryStream(),
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnSendMessageAndNotWrapItAsync()
        {
            // given
            string randomMexTo = GetRandomString();
            string randomWorkflowId = GetRandomString();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshProcessingService.SendMessageAsync(
                    mexTo: randomMexTo,
                    mexWorkflowId: randomWorkflowId,
                    content: new MemoryStream(),
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
