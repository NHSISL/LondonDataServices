// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldProcessDataAndLogAsync()
        {
            // Given
            int randomNumber = GetRandomNumber();
            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<SubscriberCredential> randomSubscriberAgreements =
                CreateRandomSubscriberCredentials(randomActiveSubscriberAgreementIds);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIds())
                    .ReturnsAsync(randomActiveSubscriberAgreementIds);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                SubscriberCredential randomSubscriberAgreement =
                    randomSubscriberAgreements.FirstOrDefault(agreement => agreement.Id == subscriberAgreementId);

                this.subscriberCredentialOrchestrationMock.Setup(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId))
                        .ReturnsAsync(randomSubscriberAgreement);

                this.emisLandingExtractionOrchestrationServiceMock.Setup(service =>
                    service.ProcessAsync(randomSubscriberAgreement))
                        .ReturnsAsync(randomEmisLandingPaths);
            }

            // When
            List<string> actualPaths = await this.emisLandingCoordinationService.ProcessAsync();

            // Then
            actualPaths.Count().Should().Be(randomEmisLandingPaths.Count * randomNumber);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIds(),
                    Times.Once);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                SubscriberCredential randomSubscriberAgreement =
                    randomSubscriberAgreements.FirstOrDefault(agreement => agreement.Id == subscriberAgreementId);

                this.subscriberCredentialOrchestrationMock.Verify(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId),
                        Times.Once);

                this.emisLandingExtractionOrchestrationServiceMock.Verify(service =>
                    service.ProcessAsync(randomSubscriberAgreement),
                        Times.Once);
            }

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

