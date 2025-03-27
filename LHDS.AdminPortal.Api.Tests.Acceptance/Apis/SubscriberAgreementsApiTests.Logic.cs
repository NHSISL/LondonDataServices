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
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement;

            // when 
            await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);

            SubscriberAgreement actualSubscriberAgreement =
                await this.apiBroker.GetSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(actualSubscriberAgreement.Id);
        }

        [Fact]
        public async Task ShouldGetAllSubscriberAgreementsAsync()
        {
            // given
            List<SubscriberAgreement> randomSubscriberAgreements = await PostRandomSubscriberAgreementsAsync();
            List<SubscriberAgreement> expectedSubscriberAgreements = randomSubscriberAgreements;

            // when
            List<SubscriberAgreement> actualSubscriberAgreements = 
                await this.apiBroker.GetAllSubscriberAgreementsAsync();

            // then
            foreach (SubscriberAgreement expectedSubscriberAgreement in expectedSubscriberAgreements)
            {
                SubscriberAgreement actualSubscriberAgreement = 
                    actualSubscriberAgreements.Single(approval => approval.Id == expectedSubscriberAgreement.Id);

                actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement, options => options
                    .Excluding(property => property.CreatedBy)
                    .Excluding(property => property.CreatedDate)
                    .Excluding(property => property.UpdatedBy)
                    .Excluding(property => property.UpdatedDate));
                    
                await this.apiBroker.DeleteSubscriberAgreementByIdAsync(actualSubscriberAgreement.Id);
            }
        }

        [Fact]
        public async Task ShouldGetSubscriberAgreementAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = await PostRandomSubscriberAgreementAsync();
            SubscriberAgreement expectedSubscriberAgreement = randomSubscriberAgreement;

            // when
            SubscriberAgreement actualSubscriberAgreement = 
                await this.apiBroker.GetSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(actualSubscriberAgreement.Id);
        }

        [Fact]
        public async Task ShouldPutSubscriberAgreementAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = await PostRandomSubscriberAgreementAsync();

            SubscriberAgreement modifiedSubscriberAgreement = 
                UpdateSubscriberAgreementWithRandomValues(randomSubscriberAgreement);

            // when
            await this.apiBroker.PutSubscriberAgreementAsync(modifiedSubscriberAgreement);

            SubscriberAgreement actualSubscriberAgreement = 
                await this.apiBroker.GetSubscriberAgreementByIdAsync(randomSubscriberAgreement.Id);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(modifiedSubscriberAgreement, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(actualSubscriberAgreement.Id);
        }

        [Fact]
        public async Task ShouldDeleteSubscriberAgreementAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = await PostRandomSubscriberAgreementAsync();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement;

            // when
            SubscriberAgreement deletedSubscriberAgreement =
                await this.apiBroker.DeleteSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            ValueTask<SubscriberAgreement> getSubscriberAgreementbyIdTask =
                this.apiBroker.GetSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // then
            deletedSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement, options => options
                .Excluding(property => property.CreatedBy)
                .Excluding(property => property.CreatedDate)
                .Excluding(property => property.UpdatedBy)
                .Excluding(property => property.UpdatedDate));

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(getSubscriberAgreementbyIdTask.AsTask);
        }
    }
}