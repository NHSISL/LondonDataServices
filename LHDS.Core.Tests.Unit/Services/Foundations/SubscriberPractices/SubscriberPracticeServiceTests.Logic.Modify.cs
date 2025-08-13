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
            EntraUser randomEntraUser = CreateRandomEntraUser();

            SubscriberPractice randomSubscriberPractice =
                CreateRandomModifySubscriberPractice(randomDateTimeOffset, randomEntraUser.EntraUserId);

            SubscriberPractice inputSubscriberPractice = randomSubscriberPractice.DeepClone();
            inputSubscriberPractice.CreatedBy = Guid.NewGuid().ToString();
            inputSubscriberPractice.CreatedDate = randomDateTimeOffset.AddDays(1);
            SubscriberPractice storageSubscriberPractice = randomSubscriberPractice.DeepClone();
            storageSubscriberPractice.UpdatedDate = storageSubscriberPractice.CreatedDate;
            SubscriberPractice updatedSubscriberPractice = inputSubscriberPractice.DeepClone();
            updatedSubscriberPractice.CreatedBy = storageSubscriberPractice.CreatedBy;
            updatedSubscriberPractice.CreatedDate = storageSubscriberPractice.CreatedDate;
            SubscriberPractice expectedSubscriberPractice = updatedSubscriberPractice.DeepClone();
            Guid subscriberPracticeId = inputSubscriberPractice.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSubscriberPracticeByIdAsync(subscriberPracticeId))
                    .ReturnsAsync(storageSubscriberPractice);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSubscriberPracticeAsync(inputSubscriberPractice))
                    .ReturnsAsync(updatedSubscriberPractice);

            // when
            SubscriberPractice actualSubscriberPractice =
                await this.subscriberPracticeService.ModifySubscriberPracticeAsync(inputSubscriberPractice);

            // then
            actualSubscriberPractice.Should().BeEquivalentTo(expectedSubscriberPractice);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSubscriberPracticeByIdAsync(inputSubscriberPractice.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSubscriberPracticeAsync(inputSubscriberPractice),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}