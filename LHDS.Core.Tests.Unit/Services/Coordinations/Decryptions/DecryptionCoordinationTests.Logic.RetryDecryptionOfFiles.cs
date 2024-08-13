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
        public async Task ShouldRetryDecryptionOfExistingFilesAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid SubscriberCredentialId = Guid.NewGuid();

            string filePath = CreateRandomFilePath(SubscriberCredentialId);

            this.decryptionOrchestrationServiceMock.SetupSequence(service =>
                service.GetNextItemToBeDecrypted())
                    .ReturnsAsync(filePath)
                    .ReturnsAsync(string.Empty);

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
            await this.decryptionCoordinationService.RetryDecryptOnAllAsync();

            // Then
            this.decryptionOrchestrationServiceMock.Verify(service =>
                service.GetNextItemToBeDecrypted(),
                    Times.Exactly(2));

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId, false),
                    Times.Once);

            this.decryptionOrchestrationServiceMock.Verify(service =>
                    service.DecryptAsync(filePath, randomActiveSubscriberCredential),
                        Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.decryptionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.ingressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}