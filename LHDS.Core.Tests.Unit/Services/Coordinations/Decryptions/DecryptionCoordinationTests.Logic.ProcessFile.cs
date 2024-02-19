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
        public async Task ShouldProcessExistingFileAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid SubscriberCredentialId = Guid.NewGuid();

            string filePath = CreateRandomFilePath(SupplierSharingAgreementGuid);

            SubscriberCredential randomActiveSubscriberCredential =
                CreateRandomSubscriberCredential(SupplierSharingAgreementGuid);

            SubscriberCredential storageSubscriberCredential = randomActiveSubscriberCredential;
            string randomEmisLandingPath = GetRandomString();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId))
                    .ReturnsAsync(randomActiveSubscriberCredential);

            this.decryptionOrchestrationServiceMock.Setup(service =>
                    service.DecryptAsync(filePath, storageSubscriberCredential))
                        .ReturnsAsync(randomEmisLandingPath);

            // When
            string actualPath = await this.decryptionCoordinationService.DecryptAsync(filePath);

            // Then
            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialBySupplierSharingAgreementGuidAsync(SupplierSharingAgreementGuid),
                    Times.Once);

            this.emisLandingExtractionOrchestrationServiceMock.Verify(service =>
                    service.ProcessFileAsync(externalIngestionTracking.FileName, storageSubscriberCredential),
                        Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

