// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public void ShouldReturnSubscriberAgreements()
        {
            // given
            IQueryable<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            IQueryable<SubscriberAgreement> storageSubscriberAgreements = randomSubscriberAgreements;
            IQueryable<SubscriberAgreement> expectedSubscriberAgreements = storageSubscriberAgreements;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberAgreements())
                    .Returns(storageSubscriberAgreements);

            // when
            IQueryable<SubscriberAgreement> actualSubscriberAgreements =
                this.subscriberAgreementService.RetrieveAllSubscriberAgreements();

            // then
            actualSubscriberAgreements.Should().BeEquivalentTo(expectedSubscriberAgreements);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberAgreements(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}