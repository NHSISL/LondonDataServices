// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSubscriberAgreementByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            SubscriberAgreement randomSubscriberAgreement = 
                CreateRandomSubscriberAgreement(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Guid inputSubscriberAgreementId = randomSubscriberAgreement.Id;
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement ingestionTrackingWithDeleteAuditApplied = storageSubscriberAgreement.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            SubscriberAgreement updatedSubscriberAgreement = storageSubscriberAgreement;
            SubscriberAgreement deletedSubscriberAgreement = updatedSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = deletedSubscriberAgreement.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreementId))
                    .ReturnsAsync(storageSubscriberAgreement);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSubscriberAgreementAsync(randomSubscriberAgreement))
                    .ReturnsAsync(updatedSubscriberAgreement);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSubscriberAgreementAsync(updatedSubscriberAgreement))
                    .ReturnsAsync(deletedSubscriberAgreement);

            // when
            SubscriberAgreement actualSubscriberAgreement = await this.subscriberAgreementService
                .RemoveSubscriberAgreementByIdAsync(inputSubscriberAgreementId);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreementId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(randomSubscriberAgreement),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberAgreementAsync(updatedSubscriberAgreement),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}