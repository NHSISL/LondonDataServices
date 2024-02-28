// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
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
            IQueryable<SubscriberAgreement> inputSubscriberAgreements = CreateRandomSubscriberAgreements();
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

    }
}