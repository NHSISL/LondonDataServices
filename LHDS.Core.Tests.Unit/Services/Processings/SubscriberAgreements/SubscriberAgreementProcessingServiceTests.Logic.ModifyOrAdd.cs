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
        public async Task ShouldModifySubscriberAgreementIfOneExistsAndNotAddAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement modifiedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            modifiedSubscriberAgreement.SupplierSharingAgreementShortName =
                modifiedSubscriberAgreement.SupplierSharingAgreementShortName + "Modified";

            SubscriberAgreement updatedSubscriberAgreement = modifiedSubscriberAgreement.DeepClone();
            SubscriberAgreement expectedSubscriberAgreement = updatedSubscriberAgreement;

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(modifiedSubscriberAgreement.Id))
                    .ReturnsAsync(value: storageSubscriberAgreement);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.ModifySubscriberAgreementAsync(modifiedSubscriberAgreement))
                    .ReturnsAsync(value: updatedSubscriberAgreement);

            // When
            await this.subscriberAgreementProcessingService.ModifyOrAddSubscriberAgreementAsync(
                modifiedSubscriberAgreement);

            // Then
            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(modifiedSubscriberAgreement.Id),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.ModifySubscriberAgreementAsync(modifiedSubscriberAgreement),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.AddSubscriberAgreementAsync(modifiedSubscriberAgreement),
                    Times.Never);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddSubscriberAgreementIfSubscriberAgreementDoesNotExistsAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement.DeepClone();
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement;

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ReturnsAsync(value: null);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.AddSubscriberAgreementAsync(inputSubscriberAgreement))
                    .ReturnsAsync(value: storageSubscriberAgreement);

            // When
            await this.subscriberAgreementProcessingService
                .ModifyOrAddSubscriberAgreementAsync(inputSubscriberAgreement);

            // Then
            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
            service.AddSubscriberAgreementAsync(inputSubscriberAgreement),
            Times.Once);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Never);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}