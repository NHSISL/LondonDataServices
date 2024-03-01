// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
            List<dynamic> randomDynamics = CreateRandomDynamicSubscriberAgreementCredentials();

            IQueryable<SubscriberAgreement> storageSubscriberAgreements =
                CreateSubscriberAgreementsFromDynamic(randomDynamics);

            IQueryable<SubscriberCredential> expectedSubscriberCredentials =
                CreateSubscriberCredentialsFromDynamic(randomDynamics);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Returns(storageSubscriberAgreements);

            // When
            IQueryable<SubscriberCredential> actualSubscriberCredentials = this.subscriberCredentialOrchestration
                .RetrieveAllSubscriberCredentials();

            // Then
            actualSubscriberCredentials.Should().BeEquivalentTo(expectedSubscriberCredentials);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreements(),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

