// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSubscriberAgreementIfOneExistsAndNotAddAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ReturnsAsync(value: storageSubscriberAgreement);

            // When
            await this.subscriberAgreementProcessingService
                .RetrieveOrAddSubscriberAgreementAsync(inputSubscriberAgreement);

            // Then
            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Never);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddSubscriberAgreementIfOneDoesNotExistAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ReturnsAsync(value: null);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement))
                    .ReturnsAsync(storageSubscriberAgreement);

            // When
            await this.subscriberAgreementProcessingService
                .RetrieveOrAddSubscriberAgreementAsync(inputSubscriberAgreement);

            // Then
            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}