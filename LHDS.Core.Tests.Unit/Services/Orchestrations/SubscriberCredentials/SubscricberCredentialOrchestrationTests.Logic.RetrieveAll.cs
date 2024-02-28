// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldRetireiveAllSubscriberCredentialAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential();
            SubscriberAgreement inputSubscriberAgreement = CreateSubscriberAgreementFromDynamic(randomDynamic);
            SubscriberAgreement outputSubscriberAgreement = inputSubscriberAgreement;
            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
            SubscriberCredential storageSubscriberCredential = inputSubscriberCredential.DeepClone();
            SubscriberCredential outputSubscriberCredential = storageSubscriberCredential.DeepClone();
            SubscriberCredential expectedSubscriberCredential = outputSubscriberCredential.DeepClone();

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.Is(SameSubscriberAgreementAs(inputSubscriberAgreement))))
                    .ReturnsAsync(outputSubscriberAgreement);

            // When
            IQueryable<SubscriberCredential> actualSubscriberCredentials = this.subscriberCredentialOrchestration
                .RetrieveAllSubscriberCredentials();

            // Then
            actualSubscriberCredentials.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.Is(SameSubscriberAgreementAs(inputSubscriberAgreement))),
                    Times.Once);

            this.secureDataProcessingServiceMock.Verify(service =>
                service.AddOrModifySecureDataAsync(It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

