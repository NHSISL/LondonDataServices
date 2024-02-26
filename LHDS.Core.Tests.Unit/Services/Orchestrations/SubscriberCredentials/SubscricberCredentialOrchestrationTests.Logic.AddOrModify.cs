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
        public async Task ShouldModifyOrAddSubscriberCredentialAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential();
            SubscriberAgreement inputSubscriberAgreement = CreateSubscriberAgreementFromDynamic(randomDynamic);
            SubscriberAgreement outputSubscriberAgreement = inputSubscriberAgreement;
            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
            SubscriberCredential outputSubscriberCredential = inputSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = outputSubscriberCredential.DeepClone();

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.ModifyOrAddSubscriberAgreementAsync(inputSubscriberAgreement))
                    .ReturnsAsync(outputSubscriberAgreement);

            this.secureDataProcessingServiceMock.Setup(service =>
                service.AddOrModifySecureDataAsync(inputSubscriberCredential))
                    .ReturnsAsync(outputSubscriberCredential);

            // When
            SubscriberCredential actualSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

            // Then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.ModifyOrAddSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.secureDataProcessingServiceMock.Verify(service =>
                service.AddOrModifySecureDataAsync(inputSubscriberCredential),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

