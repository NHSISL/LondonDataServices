// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllSubscriberAgreementsAsync()
        {
            // given
            IQueryable<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            IQueryable<SubscriberAgreement> storageSubscriberAgreements = randomSubscriberAgreements;
            IQueryable<SubscriberAgreement> expectedSubscriberAgreements = storageSubscriberAgreements;

            this.subscriberAgreementServiceMock.Setup(broker =>
                broker.RetrieveAllSubscriberAgreementsAsync())
                    .ReturnsAsync(storageSubscriberAgreements);

            // when
            IQueryable<SubscriberAgreement> actualSubscriberAgreements =
                await this.subscriberAgreementProcessingService.RetrieveAllSubscriberAgreementsAsync();

            // then
            actualSubscriberAgreements.Should().BeEquivalentTo(expectedSubscriberAgreements);

            this.subscriberAgreementServiceMock.Verify(broker =>
                broker.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}