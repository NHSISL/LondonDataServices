// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldModifySubscriberPracticeAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberPractice randomSubscriberPractice =
                CreateRandomModifySubscriberPractice(randomDateTimeOffset, randomUserId);

            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice.DeepClone();
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice.DeepClone();
            storageSubscriberPractice.UpdatedDate = inputSubscriberPractice.CreatedDate;
            SubscriberPractice updatedSubscriberPractice = inputSubscriberPractice.DeepClone();
            updatedSubscriberPractice.CreatedBy = storageSubscriberPractice.CreatedBy;
            updatedSubscriberPractice.CreatedDate = storageSubscriberPractice.CreatedDate;
            SubscriberPractice expectedSubscriberPractice = updatedSubscriberPractice.DeepClone();
            Guid subscriberPracticeId = inputSubscriberPractice.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputSubscriberPractice))
                    .ReturnsAsync(inputSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(subscriberPracticeId))
                    .ReturnsAsync(storageSubscriberPractice);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    inputSubscriberPractice, storageSubscriberPractice))
                        .ReturnsAsync(inputSubscriberPractice);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSubscriberPracticeAsync(inputSubscriberPractice))
                    .ReturnsAsync(updatedSubscriberPractice);

            // when
            SubscriberPractice actualSubscriberPractice =
                await this.subscriberPracticeService.ModifySubscriberPracticeAsync(inputSubscriberPractice);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(inputSubscriberPractice.Id),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    inputSubscriberPractice, storageSubscriberPractice),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(inputSubscriberPractice),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}