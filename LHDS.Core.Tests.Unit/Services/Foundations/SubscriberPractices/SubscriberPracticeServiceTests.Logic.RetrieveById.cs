// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveSubscriberPracticeByIdAsync()
        {
            // given
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();
            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice;
            SubscriberPractice expectedSubscriberPractice = storageSubscriberPractice.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(inputSubscriberPractice.Id))
                    .ReturnsAsync(storageSubscriberPractice);

            // when
            SubscriberPractice actualSubscriberPractice =
                await this.subscriberPracticeService.RetrieveSubscriberPracticeByIdAsync(inputSubscriberPractice.Id);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(inputSubscriberPractice.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}