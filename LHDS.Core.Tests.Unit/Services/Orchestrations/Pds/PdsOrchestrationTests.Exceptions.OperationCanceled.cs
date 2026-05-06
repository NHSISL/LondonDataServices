// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldNotWrapOperationCanceledOnValidateMailboxAccessAsync()
        {
            // given
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when / then
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.pdsOrchestrationService
                    .ValidateMailboxAccessAsync(cancellationTokenSource.Token).AsTask());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotWrapOperationCanceledOnPickupFileAndSendToMeshAsync()
        {
            // given
            byte[] randomBytes = Encoding.UTF8.GetBytes(GetRandomString());
            string randomFileName = GetRandomString();
            await using var randomStream = new MemoryStream(randomBytes);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when / then
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.pdsOrchestrationService
                    .PickupFileAndSendToMesh(
                        randomStream,
                        randomFileName,
                        cancellationTokenSource.Token).AsTask());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotWrapOperationCanceledOnRetreiveMessagesFromMeshAndUpdateStorageAsync()
        {
            // given
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // when / then
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                this.pdsOrchestrationService
                    .RetreiveMessagesFromMeshAndUpdateStorage(
                        cancellationTokenSource.Token).AsTask());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
