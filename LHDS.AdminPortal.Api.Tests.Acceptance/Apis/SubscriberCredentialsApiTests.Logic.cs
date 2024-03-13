// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberCredentials
{
    public partial class SubscriberCredentialsApiTests
    {
        [Fact]
        public async Task ShouldPostSubscriberCredentialAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            Guid subscriberAgreementId = Guid.NewGuid();
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(subscriberAgreementId);
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential;
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

        //[Fact]
        //public async Task ShouldGetAllSubscriberCredentialsAsync()
        //{
        //    // given
        //    List<SubscriberCredential> randomSubscriberCredentials = await PostRandomSubscriberCredentialsAsync();
        //    List<SubscriberCredential> expectedSubscriberCredentials = randomSubscriberCredentials;

        //    // when
        //    List<SubscriberCredential> actualSubscriberCredentials = await this.apiBroker
        //        .GetAllSubscriberCredentialsAsync();

        //    // then
        //    foreach (SubscriberCredential expectedSubscriberCredential in expectedSubscriberCredentials)
        //    {
        //        SubscriberCredential actualSubscriberCredential = actualSubscriberCredentials
        //            .Single(approval => approval.Id == expectedSubscriberCredential.Id);

        //        actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
        //        await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
        //    }
        //}

        //    [Fact]
        //    public async Task ShouldGetSubscriberCredentialAsync()
        //    {
        //        // given
        //        SubscriberCredential randomSubscriberCredential = await PostRandomSubscriberCredentialAsync();
        //        SubscriberCredential expectedSubscriberCredential = randomSubscriberCredential;

        //        // when
        //        SubscriberCredential actualSubscriberCredential = await this.apiBroker
        //            .GetSubscriberCredentialByIdAsync(randomSubscriberCredential.Id);

        //        // then
        //        actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
        //        await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
        //    }

        //    [Fact]
        //    public async Task ShouldPutSubscriberCredentialAsync()
        //    {
        //        // given
        //        SubscriberCredential randomSubscriberCredential = await PostRandomSubscriberCredentialAsync();

        //        SubscriberCredential modifiedSubscriberCredential =
        //            UpdateSubscriberCredentialWithRandomValues(randomSubscriberCredential);

        //        // when
        //        await this.apiBroker.PutSubscriberCredentialAsync(modifiedSubscriberCredential);

        //        SubscriberCredential actualSubscriberCredential = await this.apiBroker
        //            .GetSubscriberCredentialByIdAsync(randomSubscriberCredential.Id);

        //        // then
        //        actualSubscriberCredential.Should().BeEquivalentTo(modifiedSubscriberCredential);
        //        await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
        //    }

        //    [Fact]
        //    public async Task ShouldDeleteSubscriberCredentialAsync()
        //    {
        //        // given
        //        SubscriberCredential randomSubscriberCredential = await PostRandomSubscriberCredentialAsync();
        //        SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
        //        SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential;

        //        // when
        //        SubscriberCredential deletedSubscriberCredential =
        //            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(inputSubscriberCredential.Id);

        //        ValueTask<SubscriberCredential> getSubscriberCredentialbyIdTask =
        //            this.apiBroker.GetSubscriberCredentialByIdAsync(inputSubscriberCredential.Id);

        //        // then
        //        deletedSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

        //        await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
        //            getSubscriberCredentialbyIdTask.AsTask());
        //    }
    }
}