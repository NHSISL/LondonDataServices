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
            IQueryable<dynamic> randomDynamics = CreateRandomDynamicSubscriberAgreementCredentials();

            IQueryable<SubscriberAgreement> inputSubscriberAgreements = 
                CreateSubscriberAgreementsFromDynamic(randomDynamics);

            IQueryable<SubscriberAgreement> outputSubscriberAgreements = inputSubscriberAgreements;

            IQueryable<SubscriberCredential> expectedSubscriberCredential = 
                CreateSubscriberCredentialsFromDynamic(randomDynamics);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Returns(outputSubscriberAgreements);

            // When
            IQueryable<SubscriberCredential> actualSubscriberCredentials = this.subscriberCredentialOrchestration
                .RetrieveAllSubscriberCredentials();

            // Then
            actualSubscriberCredentials.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreements(),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

