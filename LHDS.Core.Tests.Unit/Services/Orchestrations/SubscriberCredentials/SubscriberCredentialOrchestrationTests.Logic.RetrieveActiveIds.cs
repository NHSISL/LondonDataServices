// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllActiveSubscriberCredentialIdsAndLogAsync()
        {
            // Given
            Guid inputSupplierId = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            List<SubscriberAgreement> storageSubscriberAgreements = CreateRandomSubscriberAgreements(inputSupplierId);
            List<Guid> expectedSubscriberCredentialIds = new List<Guid>();

            foreach (SubscriberAgreement subscriberAgreement in storageSubscriberAgreements)
            {
                if (subscriberAgreement.IsActive == true)
                {
                    expectedSubscriberCredentialIds.Add(subscriberAgreement.Id);
                }
            }

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ReturnsAsync(storageSubscriberAgreements.AsQueryable());

            // When
            List<Guid> actualSubscriberCredentialIds = await this.subscriberCredentialOrchestration
                .RetrieveAllActiveSubscriberCredentialIdsAsync(inputSupplierId);

            // Then
            actualSubscriberCredentialIds.Should().BeEquivalentTo(expectedSubscriberCredentialIds);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}

