// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
            Guid supplierId = Guid.NewGuid();

            this.ingressOrchestrationServiceMock.Setup(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(supplierId))
                    .Returns(ValueTask.CompletedTask);

            // When
            await this.decryptionCoordinationService.ProcessDecryptedItemsForBatchCompleteAsync(supplierId);

            // Then
            this.ingressOrchestrationServiceMock.Verify(service =>
                service.ProcessDecryptedItemsForBatchCompleteAsync(supplierId),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}