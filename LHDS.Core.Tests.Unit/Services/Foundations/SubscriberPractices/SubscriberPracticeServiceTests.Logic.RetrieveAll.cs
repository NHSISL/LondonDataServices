// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        [Fact]
        public async Task ShouldReturnSubscriberPracticesAsync()
        {
            // given
            IQueryable<SubscriberPractice> randomSubscriberPractices = CreateRandomSubscriberPractices();
            IQueryable<SubscriberPractice> storageSubscriberPractices = randomSubscriberPractices;
            IQueryable<SubscriberPractice> expectedSubscriberPractices = storageSubscriberPractices;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSubscriberPracticesAsync())
                    .ReturnsAsync(storageSubscriberPractices);

            // when
            IQueryable<SubscriberPractice> actualSubscriberPractices =
                await this.subscriberPracticeService.RetrieveAllSubscriberPracticesAsync();

            // then
            actualSubscriberPractices.Should().BeEquivalentTo(expectedSubscriberPractices);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSubscriberPracticesAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}