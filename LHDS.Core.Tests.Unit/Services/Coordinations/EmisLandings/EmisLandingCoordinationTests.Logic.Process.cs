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
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<SubscriberCredential> randomSubscriberAgreements =
                CreateRandomSubscriberCredentials(randomActiveSubscriberAgreementIds);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(inputSupplierId))
                    .ReturnsAsync(randomActiveSubscriberAgreementIds);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                SubscriberCredential randomSubscriberAgreement =
                    randomSubscriberAgreements.FirstOrDefault(agreement => agreement.Id == subscriberAgreementId);

                this.subscriberCredentialOrchestrationMock.Setup(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, false))
                        .ReturnsAsync(randomSubscriberAgreement);

                this.emisLandingOrchestrationServiceMock.Setup(service =>
                    service.ProcessAsync(randomSubscriberAgreement, inputSupplierId))
                        .ReturnsAsync(randomEmisLandingPaths);
            }

            // When
            List<string> actualPaths = await this.emisLandingCoordinationService
                .ProcessAsync(supplierId: inputSupplierId);

            // Then
            actualPaths.Count().Should().Be(randomEmisLandingPaths.Count * randomNumber);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(inputSupplierId),
                    Times.Once);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                SubscriberCredential randomSubscriberAgreement =
                    randomSubscriberAgreements.FirstOrDefault(agreement => agreement.Id == subscriberAgreementId);

                this.subscriberCredentialOrchestrationMock.Verify(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, false),
                        Times.Once);

                this.emisLandingOrchestrationServiceMock.Verify(service =>
                    service.ProcessAsync(randomSubscriberAgreement, inputSupplierId),
                        Times.Once);
            }

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

