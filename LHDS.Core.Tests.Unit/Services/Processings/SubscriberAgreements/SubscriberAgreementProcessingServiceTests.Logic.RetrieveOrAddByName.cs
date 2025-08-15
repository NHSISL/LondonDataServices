// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldRetrieveSubscriberAgreementIfNameExistsAndNotAddAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            List<SubscriberAgreement> storageSubscriberAgreements =
                new List<SubscriberAgreement> { storageSubscriberAgreement };

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ReturnsAsync(storageSubscriberAgreements.AsQueryable());

            // When
            SubscriberAgreement actualSubscriberAgreement = await this.subscriberAgreementProcessingService
                .RetrieveOrAddSubscriberAgreementByNameAsync(inputSubscriberAgreement);

            // Then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Never);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddSubscriberAgreementIfNameDoesNotExistAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();
            List<SubscriberAgreement> storageSubscriberAgreements = new List<SubscriberAgreement>();

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ReturnsAsync(storageSubscriberAgreements.AsQueryable);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement))
                    .ReturnsAsync(storageSubscriberAgreement);

            // When
            SubscriberAgreement actualSubscriberAgreement =  await this.subscriberAgreementProcessingService
                .RetrieveOrAddSubscriberAgreementByNameAsync(inputSubscriberAgreement);

            // Then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}