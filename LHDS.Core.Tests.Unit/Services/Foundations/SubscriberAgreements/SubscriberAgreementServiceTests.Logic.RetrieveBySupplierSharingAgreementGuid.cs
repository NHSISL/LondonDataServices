// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSubscriberAgreementBySupplierSharingAgreementGuidAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = storageSubscriberAgreement.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(
                    inputSubscriberAgreement.SupplierSharingAgreementGuid))
                    .ReturnsAsync(storageSubscriberAgreement);

            // when
            SubscriberAgreement actualSubscriberAgreement =
                await this.subscriberAgreementService.RetrieveSubscriberAgreementBySupplierSharingAgreementGuidAsync(
                    inputSubscriberAgreement.SupplierSharingAgreementGuid);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(
                    inputSubscriberAgreement.SupplierSharingAgreementGuid),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}