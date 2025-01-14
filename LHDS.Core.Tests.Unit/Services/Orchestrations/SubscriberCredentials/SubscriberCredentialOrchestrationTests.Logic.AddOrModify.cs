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
        public async Task ShouldModifyOrAddSubscriberCredentialWithoutSecureDataAndLogAsync(bool externalUse)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberAgreement inputSubscriberAgreement = CreateSubscriberAgreementFromDynamic(randomDynamic);
            SubscriberAgreement outputSubscriberAgreement = inputSubscriberAgreement;
            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
            SubscriberCredential storageSubscriberCredential = inputSubscriberCredential.DeepClone();
            SubscriberCredential outputSubscriberCredential = storageSubscriberCredential.DeepClone();

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
                service.ModifyOrAddSubscriberAgreementAsync(It.Is(SameSubscriberAgreementAs(inputSubscriberAgreement))))
                    .ReturnsAsync(outputSubscriberAgreement);

            // When
            SubscriberCredential actualSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: inputSubscriberCredential,
                    regenerateKeys: false,
                    externalUse);

            // Then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.Is(SameSubscriberAgreementAs(inputSubscriberAgreement))),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldModifyOrAddSubscriberCredentialWithSecureDataAndLogAsync(bool externalUse)
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(date: randomDateTimeOffset);
            SubscriberAgreement inputSubscriberAgreement = CreateSubscriberAgreementFromDynamic(randomDynamic);
            SubscriberAgreement outputSubscriberAgreement = inputSubscriberAgreement;
            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
            SubscriberCredential storageSubscriberCredential = inputSubscriberCredential.DeepClone();
            SubscriberCredential storageSubscriberCredentialWithGeneratedKeys = inputSubscriberCredential.DeepClone();
            storageSubscriberCredentialWithGeneratedKeys.FtpPassword = GetRandomString();
            storageSubscriberCredentialWithGeneratedKeys.FtpPassPhrase = GetRandomString();
            storageSubscriberCredentialWithGeneratedKeys.FtpPrivateKey = GetRandomString();
            storageSubscriberCredentialWithGeneratedKeys.FtpPublicKey = GetRandomString();
            storageSubscriberCredentialWithGeneratedKeys.GpgPassPhrase = GetRandomString();
            storageSubscriberCredentialWithGeneratedKeys.GpgPrivateKey = GetRandomString();
            storageSubscriberCredentialWithGeneratedKeys.GpgPublicKey = GetRandomString();

            SubscriberAgreement subscriberAgreementWithGeneratedKeys =
                CreateSubscriberAgreementFromSubscriberCredential(storageSubscriberCredentialWithGeneratedKeys);

            subscriberAgreementWithGeneratedKeys.UpdatedDate = randomDateTimeOffset;

            SubscriberAgreement outputSubscriberAgreementWithGeneratedKeys =
                subscriberAgreementWithGeneratedKeys.DeepClone();

            SubscriberCredential updatedSubscriberCredentialWithGeneratedKeys =
                storageSubscriberCredentialWithGeneratedKeys.DeepClone();

            SubscriberCredential outputSubscriberCredential = updatedSubscriberCredentialWithGeneratedKeys.DeepClone();

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
                service.ModifyOrAddSubscriberAgreementAsync(It.Is(SameSubscriberAgreementAs(inputSubscriberAgreement))))
                    .ReturnsAsync(outputSubscriberAgreement);

            this.cryptographyKeyProcessingServiceMock.Setup(service =>
                service.GenerateKeysAsync(It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))))
                    .ReturnsAsync(storageSubscriberCredentialWithGeneratedKeys);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.ModifyOrAddSubscriberAgreementAsync(
                    It.Is(SameSubscriberAgreementAs(subscriberAgreementWithGeneratedKeys))))
                        .ReturnsAsync(outputSubscriberAgreementWithGeneratedKeys);

            this.secureDataProcessingServiceMock.Setup(service =>
                service.AddOrModifySecureDataAsync(
                    It.Is(SameSubscriberCredentialAs(storageSubscriberCredentialWithGeneratedKeys))))
                        .ReturnsAsync(updatedSubscriberCredentialWithGeneratedKeys);

            // When
            SubscriberCredential actualSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(
                    subscriberCredential: inputSubscriberCredential,
                    regenerateKeys: true,
                    externalUse);

            // Then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.Is(SameSubscriberAgreementAs(inputSubscriberAgreement))),
                    Times.Once);

            this.cryptographyKeyProcessingServiceMock.Verify(service =>
                service.GenerateKeysAsync(
                    It.Is(SameSubscriberCredentialAs(storageSubscriberCredential))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
                service.ModifyOrAddSubscriberAgreementAsync(
                    It.Is(SameSubscriberAgreementAs(subscriberAgreementWithGeneratedKeys))),
                        Times.Once);

            this.secureDataProcessingServiceMock.Verify(service =>
                service.AddOrModifySecureDataAsync(
                    It.Is(SameSubscriberCredentialAs(storageSubscriberCredentialWithGeneratedKeys))),
                        Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

