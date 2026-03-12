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
        public async Task ShouldAddSubscriberPracticeAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomUserId = GetRandomStringWithLengthOf(50);

            SubscriberPractice randomSubscriberPractice = 
                CreateRandomSubscriberPractice(randomDateTimeOffset, randomUserId);

            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice storageSubscriberPractice = inputSubscriberPractice;
            SubscriberPractice expectedSubscriberPractice = storageSubscriberPractice.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(inputSubscriberPractice))
                    .ReturnsAsync(inputSubscriberPractice);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSubscriberPracticeAsync(inputSubscriberPractice))
                    .ReturnsAsync(storageSubscriberPractice);

            // when
            SubscriberPractice actualSubscriberPractice = await this.subscriberPracticeService
                .AddSubscriberPracticeAsync(inputSubscriberPractice);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(inputSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(inputSubscriberPractice),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}