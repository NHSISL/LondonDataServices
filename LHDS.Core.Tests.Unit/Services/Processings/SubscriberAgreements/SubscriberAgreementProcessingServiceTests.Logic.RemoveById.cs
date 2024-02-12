// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSubscriberAgreementByIdAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id))
                    .ReturnsAsync(storageSubscriberAgreement);

            // When
            SubscriberAgreement actualSubscriberAgreement =
                await this.subscriberAgreementProcessingService
                    .RemoveSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id);

            // Then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}