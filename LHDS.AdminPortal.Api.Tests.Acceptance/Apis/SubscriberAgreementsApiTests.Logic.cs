// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using RESTFulSense.Exceptions;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberAgreements
{
    public partial class SubscriberAgreementsApiTests
    {
        [Fact]
        public async Task ShouldPostSubscriberAgreementAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement;

            // When
            SubscriberAgreement actualSubscriberAgreement =
                await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);

            // Then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            // Cleanup
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);
        }


        [Fact]
        public async Task ShouldGetAllSubscriberAgreementsAsync()
        {
            // Given
            IQueryable<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            IQueryable<SubscriberAgreement> inputSubscriberAgreements = randomSubscriberAgreements;
            IQueryable<SubscriberAgreement> expectedSubscriberAgreements = inputSubscriberAgreements;

            foreach (SubscriberAgreement inputSubscriberAgreement in inputSubscriberAgreements)
            {
                await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);
            }

            // When
            List<SubscriberAgreement> actualSubscriberAgreements = await this.apiBroker.GetAllSubscriberAgreementsAsync();

            // Then
            foreach (SubscriberAgreement expectedSubscriberAgreement in expectedSubscriberAgreements)
            {
                SubscriberAgreement actualSubscriberAgreement =
                    actualSubscriberAgreements.Single(approval => approval.Id == expectedSubscriberAgreement.Id);

                actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);
                await this.apiBroker.DeleteSubscriberAgreementByIdAsync(actualSubscriberAgreement.Id);
            }
        }

        [Fact]
        public async Task ShouldGetSubscriberAgreementByIdAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement;
            await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);

            // When
            SubscriberAgreement actualSubscriberAgreement =
                await this.apiBroker.GetSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // Then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            // Cleanup
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);
        }

        [Fact]
        public async Task ShouldPutSubscriberAgreementAsync()
        {
            // Given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);

            SubscriberAgreement modifiedSubscriberAgreement =
                UpdatSubscriberAgreementWithRandomValues(inputSubscriberAgreement);

            // When
            await this.apiBroker.PutSubscriberAgreementAsync(modifiedSubscriberAgreement);

            SubscriberAgreement actualSubscriberAgreement =
                await this.apiBroker.GetSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // Then
            actualSubscriberAgreement.Should().BeEquivalentTo(modifiedSubscriberAgreement);

            // Cleanup
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);
        }

        [Fact]
        public async Task ShouldDeleteSubscriberAgreementAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement;
            await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);

            // when
            SubscriberAgreement deletedSubscriberAgreement =
                await this.apiBroker.DeleteSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            ValueTask<SubscriberAgreement> getSubscriberAgreementbyIdTask =
                this.apiBroker.GetSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // then
            deletedSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getSubscriberAgreementbyIdTask.AsTask());

        }
    }
}