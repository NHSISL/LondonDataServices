// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfDocumentsToProcessAndLogAsync()
        {
            // Given
            Guid randomSubscriberCredentialId = Guid.NewGuid();
            Guid inputSubscriberCredentialId = randomSubscriberCredentialId;

            SubscriberCredential randomSubscriberCredential =
                CreateRandomSubscriberCredential(randomSubscriberCredentialId);

            SubscriberCredential storageSubscriberCredential = randomSubscriberCredential;
            List<string> randomFilePaths = GetRandomStrings(count: GetRandomNumber());
            List<string> storageFilePaths = randomFilePaths;
            List<string> expectedPaths = storageFilePaths.DeepClone();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveSubscriberCredentialByIdAsync(inputSubscriberCredentialId, false))
                    .ReturnsAsync(storageSubscriberCredential);

            this.emisLandingExtractionOrchestrationServiceMock.Setup(service =>
                service.RetrieveListOfDocumentsToProcessAsync(storageSubscriberCredential))
                    .ReturnsAsync(storageFilePaths);

            // When
            List<string> actualPaths = await this.emisLandingCoordinationService.
                RetrieveListOfDocumentsToProcessAsync(inputSubscriberCredentialId);

            // Then
            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveSubscriberCredentialByIdAsync(inputSubscriberCredentialId, false),
                    Times.Once);

            this.emisLandingExtractionOrchestrationServiceMock.Verify(service =>
                service.RetrieveListOfDocumentsToProcessAsync(storageSubscriberCredential),
                    Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

