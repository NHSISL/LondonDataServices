// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldProcessDecryptedItemsForBatchCompleteAndLogAsync()
        {
            // Given
            this.ingressOrchestrationServiceMock.Setup(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync())
                    .Returns(ValueTask.CompletedTask);

            // When
            await this.decryptionCoordinationService.ProcessDecryptedItemsForBatchCompleteAsync();

            // Then
            this.ingressOrchestrationServiceMock.Verify(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}