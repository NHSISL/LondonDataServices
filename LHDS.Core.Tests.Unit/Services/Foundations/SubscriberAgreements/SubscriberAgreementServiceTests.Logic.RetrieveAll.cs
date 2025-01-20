// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldReturnSubscriberAgreementsAsync()
        {
            // given
            IQueryable<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            IQueryable<SubscriberAgreement> storageSubscriberAgreements = randomSubscriberAgreements;
            IQueryable<SubscriberAgreement> expectedSubscriberAgreements = storageSubscriberAgreements;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberAgreementsAsync())
                    .ReturnsAsync(storageSubscriberAgreements);

            // when
            IQueryable<SubscriberAgreement> actualSubscriberAgreements =
                await this.subscriberAgreementService.RetrieveAllSubscriberAgreementsAsync();

            // then
            actualSubscriberAgreements.Should().BeEquivalentTo(expectedSubscriberAgreements);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}