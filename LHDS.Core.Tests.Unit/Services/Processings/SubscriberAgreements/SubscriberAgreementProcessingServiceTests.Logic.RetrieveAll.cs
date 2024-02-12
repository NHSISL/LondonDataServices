// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllSubscriberAgreements()
        {
            // given
            IQueryable<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            IQueryable<SubscriberAgreement> storageSubscriberAgreements = randomSubscriberAgreements;
            IQueryable<SubscriberAgreement> expectedSubscriberAgreements = storageSubscriberAgreements;

            this.subscriberAgreementServiceMock.Setup(broker =>
                broker.RetrieveAllSubscriberAgreements())
                    .Returns(storageSubscriberAgreements);

            // when
            IQueryable<SubscriberAgreement> actualSubscriberAgreements =
                this.subscriberAgreementProcessingService.RetrieveAllSubscriberAgreements();

            // then
            actualSubscriberAgreements.Should().BeEquivalentTo(expectedSubscriberAgreements);

            this.subscriberAgreementServiceMock.Verify(broker =>
                broker.RetrieveAllSubscriberAgreements(),
                    Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}