// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldProcessExistingFileAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Guid SubscriberCredentialId = Guid.NewGuid();

            string filePath = CreateRandomFilePath(SubscriberCredentialId);

            SubscriberCredential randomActiveSubscriberCredential =
                CreateRandomSubscriberCredential(SubscriberCredentialId);

            SubscriberCredential storageSubscriberCredential = randomActiveSubscriberCredential;
            string randomEmisLandingPath = GetRandomString();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId))
                    .ReturnsAsync(randomActiveSubscriberCredential);

            this.emisLandingExtractionOrchestrationServiceMock.Setup(service =>
                service.ProcessFileAsync(filePath, storageSubscriberCredential))
                    .ReturnsAsync(randomEmisLandingPath);

            // When
            string actualPath = await this.emisLandingCoordinationService.
                ProcessFileAsync(filePath);

            // Then
            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(SubscriberCredentialId, true),
                    Times.Once);

            this.emisLandingExtractionOrchestrationServiceMock.Verify(service =>
                service.ProcessFileAsync(filePath, storageSubscriberCredential),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

