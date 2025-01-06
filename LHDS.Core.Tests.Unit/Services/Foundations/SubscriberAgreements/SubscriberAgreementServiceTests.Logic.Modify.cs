// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldModifySubscriberAgreementAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomModifySubscriberAgreement(randomDateTimeOffset);
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement storageSubscriberAgreement = inputSubscriberAgreement.DeepClone();
            storageSubscriberAgreement.UpdatedDate = randomSubscriberAgreement.CreatedDate;
            SubscriberAgreement updatedSubscriberAgreement = inputSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = updatedSubscriberAgreement.DeepClone();
            Guid subscriberAgreementId = inputSubscriberAgreement.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

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
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}