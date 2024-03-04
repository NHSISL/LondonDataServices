// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSubscriberCredentialByIdAndLogAsync()
        {
            // Given
            Guid randomId = Guid.NewGuid();
            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(randomId);
            SubscriberAgreement inputSubscriberAgreement = CreateSubscriberAgreementFromDynamic(randomDynamic);
            SubscriberAgreement outputSubscriberAgreement = inputSubscriberAgreement;

            this.secureDataProcessingServiceMock.Setup(service =>
                service.RemoveSecureDataByIdAsync(randomId));

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(randomId))
                    .ReturnsAsync(outputSubscriberAgreement);

            // When
            await this.subscriberCredentialOrchestration.RemoveSubscriberCredentialByIdAsync(randomId);

            // Then
            this.secureDataProcessingServiceMock.Verify(service =>
                service.RemoveSecureDataByIdAsync(randomId),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(randomId),
                    Times.Once);

            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

