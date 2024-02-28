// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
    }
}