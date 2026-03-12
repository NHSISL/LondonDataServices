// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSubscriberPracticeByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputSubscriberPracticeId = randomId;
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice expectedInputSubscriberPractice = storageSubscriberPractice;
            SubscriberPractice deletedSubscriberPractice = expectedInputSubscriberPractice;
            SubscriberPractice expectedSubscriberPractice = deletedSubscriberPractice.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(inputSubscriberPracticeId))
                    .ReturnsAsync(storageSubscriberPractice);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSubscriberPracticeAsync(expectedInputSubscriberPractice))
                    .ReturnsAsync(deletedSubscriberPractice);

            // when
            SubscriberPractice actualSubscriberPractice = await this.subscriberPracticeService
                .RemoveSubscriberPracticeByIdAsync(inputSubscriberPracticeId);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(inputSubscriberPracticeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSubscriberPracticeAsync(expectedInputSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}