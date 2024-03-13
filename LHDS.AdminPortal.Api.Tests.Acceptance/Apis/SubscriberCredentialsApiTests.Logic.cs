// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberCredentials
{
    public partial class SubscriberCredentialsApiTests
    {
        [Fact]
        public async Task ShouldPostNewSubscriberCredentialAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PostSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPostModifiedSubscriberCredentialAsync()
        {
            // given
            DateTimeOffset dateOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = await PostRandomSubscriberAgreementAsync(subscriberAgreementId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            randomSubscriberCredential.CreatedDate = subscriberAgreement.CreatedDate;
            randomSubscriberCredential.CreatedBy = subscriberAgreement.CreatedBy;
            randomSubscriberCredential.UpdatedDate = dateOffset.AddMilliseconds(10);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PostSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPostNewSubscriberCredentialAndGenerateKeysAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PostSubscriberCredentialAndGenerateKeysAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPostModifiedSubscriberCredentialAndGenerateNewKeysAsync()
        {
            // given
            DateTimeOffset dateOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = await PostRandomSubscriberAgreementAsync(subscriberAgreementId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            randomSubscriberCredential.CreatedDate = subscriberAgreement.CreatedDate;
            randomSubscriberCredential.CreatedBy = subscriberAgreement.CreatedBy;
            randomSubscriberCredential.UpdatedDate = dateOffset.AddMilliseconds(10);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PostSubscriberCredentialAndGenerateKeysAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutNewSubscriberCredentialAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PutSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutModifiedSubscriberCredentialAsync()
        {
            // given
            DateTimeOffset dateOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = await PostRandomSubscriberAgreementAsync(subscriberAgreementId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            randomSubscriberCredential.CreatedDate = subscriberAgreement.CreatedDate;
            randomSubscriberCredential.CreatedBy = subscriberAgreement.CreatedBy;
            randomSubscriberCredential.UpdatedDate = dateOffset.AddMilliseconds(10);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PutSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutNewSubscriberCredentialAndGenerateKeysAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PutSubscriberCredentialAndGenerateKeysAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutModifiedSubscriberCredentialAndGenerateNewKeysAsync()
        {
            // given
            DateTimeOffset dateOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = await PostRandomSubscriberAgreementAsync(subscriberAgreementId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            randomSubscriberCredential.CreatedDate = subscriberAgreement.CreatedDate;
            randomSubscriberCredential.CreatedBy = subscriberAgreement.CreatedBy;
            randomSubscriberCredential.UpdatedDate = dateOffset.AddMilliseconds(10);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PutSubscriberCredentialAndGenerateKeysAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(subscriberAgreementId);
        }
    }
}