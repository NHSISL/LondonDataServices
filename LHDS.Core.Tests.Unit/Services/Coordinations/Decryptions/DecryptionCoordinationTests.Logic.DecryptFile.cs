// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldDecryptExistingFileAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid SubscriberCredentialId = Guid.NewGuid();

            string filePath = CreateRandomFilePath(SubscriberCredentialId);

            SubscriberCredential randomActiveSubscriberCredential =
                CreateRandomSubscriberCredential(SubscriberCredentialId);

            SubscriberCredential storageSubscriberCredential = randomActiveSubscriberCredential;
            string randomDecryptedFilePath = GetRandomString();
            Guid randomIngestionTrackingId = Guid.NewGuid();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId, false))
                    .ReturnsAsync(randomActiveSubscriberCredential);

            this.decryptionOrchestrationServiceMock.Setup(service =>
                    service.DecryptAsync(filePath, randomActiveSubscriberCredential))
                        .ReturnsAsync((randomDecryptedFilePath, randomIngestionTrackingId));

            // When
            string actualPath = await this.decryptionCoordinationService.DecryptAsync(filePath);

            // Then
            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId, false),
                    Times.Once);

            this.decryptionOrchestrationServiceMock.Verify(service =>
                service.DecryptAsync(filePath, randomActiveSubscriberCredential),
                    Times.Once);

            this.ingressOrchestrationServiceMock.Verify(service =>
                service.CheckForBatchCompleteAsync(randomIngestionTrackingId),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}