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
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberAgreement randomSubscriberAgreement =
                CreateRandomModifySubscriberAgreement(randomDateTimeOffset, randomUserId);

            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement.DeepClone();
            inputSubscriberAgreement.CreatedBy = Guid.NewGuid().ToString();
            inputSubscriberAgreement.CreatedDate = randomDateTimeOffset.AddDays(1);
            SubscriberAgreement storageSubscriberAgreement = randomSubscriberAgreement.DeepClone();
            storageSubscriberAgreement.UpdatedDate = storageSubscriberAgreement.CreatedDate;
            SubscriberAgreement updatedSubscriberAgreement = inputSubscriberAgreement.DeepClone();
            updatedSubscriberAgreement.CreatedBy = storageSubscriberAgreement.CreatedBy;
            updatedSubscriberAgreement.CreatedDate = storageSubscriberAgreement.CreatedDate;
            SubscriberAgreement expectedSubscriberAgreement = updatedSubscriberAgreement.DeepClone();
            Guid subscriberAgreementId = inputSubscriberAgreement.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputSubscriberAgreement))
                    .ReturnsAsync(inputSubscriberAgreement);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputSubscriberAgreement),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}