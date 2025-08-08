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
            EntraUser randomEntraUser = CreateRandomEntraUser();

            SubscriberPractice randomSubscriberPractice = 
                CreateRandomSubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice storageSubscriberPractice = inputSubscriberPractice;
            SubscriberPractice expectedSubscriberPractice = storageSubscriberPractice.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSubscriberPracticeAsync(inputSubscriberPractice))
                    .ReturnsAsync(storageSubscriberPractice);

            // when
            SubscriberPractice actualSubscriberPractice = await this.subscriberPracticeService
                .AddSubscriberPracticeAsync(inputSubscriberPractice);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberPracticeAsync(inputSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}