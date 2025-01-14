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
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldRetrieveSubscriberCredentialByIdAndLogAsync(bool externalUse)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberAgreement inputSubscriberAgreement = CreateSubscriberAgreementFromDynamic(randomDynamic);
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement;

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(
                credential: randomDynamic,
                blankKeys: true);

            SubscriberCredential outputSubscriberCredential = CreateSubscriberCredentialFromDynamic(
                credential: randomDynamic,
                blankKeys: false);

            if (externalUse)
            {
                outputSubscriberCredential.FtpPassPhrase = null;
                outputSubscriberCredential.FtpPassword = null;
                outputSubscriberCredential.FtpPrivateKey = null;
                outputSubscriberCredential.GpgPassPhrase = null;
                outputSubscriberCredential.GpgPrivateKey = null;
            }

            SubscriberCredential expectedSubscriberCredential = outputSubscriberCredential.DeepClone();

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(randomId))
                    .ReturnsAsync(storageSubscriberAgreement);

            if (externalUse == false)
            {
                this.secureDataProcessingServiceMock.Setup(service =>
                    service.RetrieveSecretsByKeyVaultKeyNameAsync(
                        It.Is(SameSubscriberCredentialAs(inputSubscriberCredential))))
                            .ReturnsAsync(outputSubscriberCredential);
            }

            // When
            SubscriberCredential actualSubscriberCredential = await this.subscriberCredentialOrchestration
                .RetrieveSubscriberCredentialByIdAsync(
                    subscriberCredentialId: randomId,
                    externalUse);

            // Then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(randomId),
                    Times.Once);

            if (externalUse == false)
            {
                this.secureDataProcessingServiceMock.Verify(service =>
                    service.RetrieveSecretsByKeyVaultKeyNameAsync(
                        It.Is(SameSubscriberCredentialAs(inputSubscriberCredential))),
                            Times.Once);
            }

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

