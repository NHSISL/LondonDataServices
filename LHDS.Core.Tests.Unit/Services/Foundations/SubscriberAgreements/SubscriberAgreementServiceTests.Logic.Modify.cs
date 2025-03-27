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
        public async Task ShouldModifySubscriberAgreementAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            SubscriberAgreement randomSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement.DeepClone();
            storageSubscriberAgreement.UpdatedDate = randomSubscriberAgreement.CreatedDate;
            SubscriberAgreement updatedSubscriberAgreement = inputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = updatedSubscriberAgreement.DeepClone();
            Guid subscriberAgreementId = inputSubscriberAgreement.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberAgreementByIdAsync(subscriberAgreementId))
                    .ReturnsAsync(storageSubscriberAgreement);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSubscriberAgreementAsync(inputSubscriberAgreement))
                    .ReturnsAsync(updatedSubscriberAgreement);

            // when
            SubscriberAgreement actualSubscriberAgreement =
                await this.subscriberAgreementService.ModifySubscriberAgreementAsync(inputSubscriberAgreement);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}