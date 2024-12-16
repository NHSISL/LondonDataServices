// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberCredentials
{
    public partial class SubscriberCredentialsApiTests
    {
        [Fact]
        public async Task ShouldPostNewSubscriberCredentialAsync()
        {
            // given
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
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.GpgPublicKey.Should().NotBeNullOrWhiteSpace();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
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
            actualSubscriberCredential.FtpPublicKey.Should().BeEquivalentTo(expectedSubscriberCredential.FtpPublicKey);
            actualSubscriberCredential.GpgPublicKey.Should().BeEquivalentTo(expectedSubscriberCredential.GpgPublicKey);
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPostNewSubscriberCredentialAndGenerateKeysAsync()
        {
            // given
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
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential, options =>
                options.Excluding(e => e.FtpPublicKey)
                    .Excluding(e => e.GpgPublicKey)
                    .Excluding(e => e.UpdatedDate));

            actualSubscriberCredential.FtpPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.GpgPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.UpdatedDate.ToString().Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPostModifiedSubscriberCredentialAndGenerateNewKeysAsync()
        {
            // given
            DateTimeOffset dateOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = await PostRandomSubscriberAgreementAsync(subscriberAgreementId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inititalSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential modifiedSubscriberCredential = randomSubscriberCredential;
            modifiedSubscriberCredential.CreatedDate = subscriberAgreement.CreatedDate;
            modifiedSubscriberCredential.CreatedBy = subscriberAgreement.CreatedBy;
            modifiedSubscriberCredential.UpdatedDate = dateOffset.AddMilliseconds(10);
            SubscriberCredential expectedSubscriberCredential = modifiedSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PostSubscriberCredentialAndGenerateKeysAsync(modifiedSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential, options =>
                options.Excluding(e => e.FtpPublicKey)
                    .Excluding(e => e.GpgPublicKey)
                    .Excluding(e => e.UpdatedDate));

            actualSubscriberCredential.FtpPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.GpgPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.UpdatedDate.ToString().Should().NotBeNullOrWhiteSpace();
            inititalSubscriberCredential.FtpPublicKey.Should().NotBeSameAs(actualSubscriberCredential.FtpPublicKey);
            inititalSubscriberCredential.GpgPublicKey.Should().NotBeSameAs(actualSubscriberCredential.GpgPublicKey);
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutNewSubscriberCredentialAsync()
        {
            // given
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
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.GpgPublicKey.Should().NotBeNullOrWhiteSpace();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
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
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutNewSubscriberCredentialAndGenerateKeysAsync()
        {
            // given
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
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential, options =>
                options.Excluding(e => e.FtpPublicKey)
                    .Excluding(e => e.GpgPublicKey)
                    .Excluding(e => e.UpdatedDate));

            actualSubscriberCredential.FtpPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.GpgPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.UpdatedDate.ToString().Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldPutModifiedSubscriberCredentialAndGenerateNewKeysAsync()
        {
            // given
            DateTimeOffset dateOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = await PostRandomSubscriberAgreementAsync(subscriberAgreementId);
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inititalSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential modifiedSubscriberCredential = randomSubscriberCredential;
            modifiedSubscriberCredential.CreatedDate = subscriberAgreement.CreatedDate;
            modifiedSubscriberCredential.CreatedBy = subscriberAgreement.CreatedBy;
            modifiedSubscriberCredential.UpdatedDate = dateOffset.AddMilliseconds(10);
            SubscriberCredential expectedSubscriberCredential = modifiedSubscriberCredential.DeepClone();
            expectedSubscriberCredential.FtpPassPhrase = null;
            expectedSubscriberCredential.FtpPrivateKey = null;
            expectedSubscriberCredential.FtpPassword = null;
            expectedSubscriberCredential.GpgPassPhrase = null;
            expectedSubscriberCredential.GpgPrivateKey = null;

            // when 
            await this.apiBroker.PutSubscriberCredentialAndGenerateKeysAsync(modifiedSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential, options =>
                options.Excluding(e => e.FtpPublicKey)
                    .Excluding(e => e.GpgPublicKey)
                    .Excluding(e => e.UpdatedDate));

            actualSubscriberCredential.FtpPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.GpgPublicKey.Should().NotBeNullOrWhiteSpace();
            actualSubscriberCredential.UpdatedDate.ToString().Should().NotBeNullOrWhiteSpace();
            inititalSubscriberCredential.FtpPublicKey.Should().NotBeSameAs(actualSubscriberCredential.FtpPublicKey);
            inititalSubscriberCredential.GpgPublicKey.Should().NotBeSameAs(actualSubscriberCredential.GpgPublicKey);
            actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
            actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
            actualSubscriberCredential.FtpPassword.Should().BeNull();
            actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
            actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact]
        public async Task ShouldGetAllSubscriberCredentialsAsync()
        {
            // given
            List<SubscriberAgreement> subscriberAgreements = CreateRandomSubscriberAgreements();

            foreach (SubscriberAgreement subscriberAgreement in subscriberAgreements)
            {
                await this.apiBroker.PostSubscriberAgreementAsync(subscriberAgreement);
            }

            List<SubscriberCredential> expectedSubscriberCredentials =
                CreatSubscriberCredentialsFromAgreements(subscriberAgreements);

            // when
            List<SubscriberCredential> actualSubscriberCredentials = await this.apiBroker
                .GetAllSubscriberCredentialsAsync();

            // then
            foreach (SubscriberCredential expectedSubscriberCredential in expectedSubscriberCredentials)
            {
                SubscriberCredential actualSubscriberCredential = actualSubscriberCredentials
                    .Single(actual => actual.Id == expectedSubscriberCredential.Id);

                actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
                actualSubscriberCredential.FtpPassPhrase.Should().BeNull();
                actualSubscriberCredential.FtpPrivateKey.Should().BeNull();
                actualSubscriberCredential.FtpPassword.Should().BeNull();
                actualSubscriberCredential.GpgPassPhrase.Should().BeNull();
                actualSubscriberCredential.GpgPrivateKey.Should().BeNull();
                await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
            }
        }

        [Fact]
        public async Task ShouldGetSubscriberCredentialAsync()
        {
            // given
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberAgreement subscriberAgreement = CreateRandomSubscriberAgreement(subscriberAgreementId);
            await this.apiBroker.PostSubscriberAgreementAsync(subscriberAgreement);

            SubscriberCredential expectedSubscriberCredential =
                CreateSubscriberCredentialFromAgreement(subscriberAgreement);

            // when
            SubscriberCredential actualSubscriberCredential = await this.apiBroker
                .GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);
        }

        [Fact(Skip = "Need to fix Assert.")]
        public async Task ShouldDeleteSubscriberCredentialAsync()
        {
            // given
            Guid subscriberAgreementId = Guid.NewGuid();
            await PostRandomSubscriberCredentialAsync(subscriberAgreementId);

            // when
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(subscriberAgreementId);

            ValueTask<SubscriberCredential> getSubscriberCredentialbyIdTask =
                this.apiBroker.GetSubscriberCredentialByIdAsync(subscriberAgreementId);

            // then
            await Assert.ThrowsAsync<HttpResponseInternalServerErrorException>(getSubscriberCredentialbyIdTask.AsTask);
        }
    }
}