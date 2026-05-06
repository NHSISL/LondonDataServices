// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldThrowOperationCanceledOnValidateMailboxAccessAndNotWrapItAsync()
        {
            // given
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshService.ValidateMailboxAccessAsync(
                    cancellationTokenSource.Token).AsTask());

            // then
            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnSendMessageAndNotWrapItAsync()
        {
            // given
            string randomMexTo = GetRandomString();
            string randomWorkflowId = GetRandomString();
            Stream randomContent = new MemoryStream();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshService.SendMessageAsync(
                    mexTo: randomMexTo,
                    mexWorkflowId: randomWorkflowId,
                    content: randomContent,
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshBrokerMock.VerifyNoOtherCalls();
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
                this.meshService.RetrieveMessageIdsFromInboxAsync(
                    cancellationTokenSource.Token).AsTask());

            // then
            this.meshBrokerMock.VerifyNoOtherCalls();
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
                this.meshService.RetrieveMessageByIdAsync(
                    messageId: randomMessageId,
                    outputStream: new MemoryStream(),
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnAcknowledgeMessageByIdAndNotWrapItAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshService.AcknowledgeMessageByIdAsync(
                    inputMessageId: randomMessageId,
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowOperationCanceledOnRetrieveTrackingStatusByIdAndNotWrapItAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.meshService.RetrieveTrackingStatusByIdAsync(
                    messageId: randomMessageId,
                    cancellationToken: cancellationTokenSource.Token).AsTask());

            // then
            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
