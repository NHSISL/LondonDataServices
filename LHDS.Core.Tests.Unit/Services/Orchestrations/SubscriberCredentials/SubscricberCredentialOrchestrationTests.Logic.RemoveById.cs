// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Processings.SubscriberCredentials;
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
            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();

            this.secureDataProcessingServiceMock.Setup(service =>
                service.RemoveSecureDataAsync(inputSubscriberCredential))
                    .ReturnsAsync(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(randomId))
                    .ReturnsAsync(outputSubscriberAgreement);

            // When
            SubscriberCredential actualSubscriberCredential = await this.subscriberCredentialOrchestration
                .RemoveSubscriberCredentialByIdAsync(randomId);

            // Then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.secureDataProcessingServiceMock.Verify(service =>
                service.RemoveSecureDataAsync(inputSubscriberCredential),
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

