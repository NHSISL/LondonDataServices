// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
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
            IngestionTracking externalIngestionTracking = CreateRandomIngestionTracking(randomDateTime);
            Guid SupplierSharingAgreementGuid = GetLastRandomGuid(externalIngestionTracking.FileName);

            SubscriberCredential randomActiveSubscriberCredential =
                CreateRandomSubscriberCredential(SupplierSharingAgreementGuid);

            SubscriberCredential storageSubscriberCredential = randomActiveSubscriberCredential;
            string randomEmisLandingPath = GetRandomString();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialBySupplierSharingAgreementGuidAsync(SupplierSharingAgreementGuid))
                    .ReturnsAsync(randomActiveSubscriberCredential);

            this.emisLandingExtractionOrchestrationServiceMock.Setup(service =>
                    service.ProcessFileAsync(externalIngestionTracking.FileName, storageSubscriberCredential))
                        .ReturnsAsync(randomEmisLandingPath);

            // When
            string actualPath = await this.emisLandingCoordinationService.
                ProcessFileAsync(externalIngestionTracking.FileName);

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

