// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DecisionPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldAddDecisionPollAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);
            DecisionPoll inputDecisionPoll = randomDecisionPoll;
            DecisionPoll storageDecisionPoll = inputDecisionPoll;
            DecisionPoll expectedDecisionPoll = storageDecisionPoll.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDecisionPollAsync(inputDecisionPoll))
                    .ReturnsAsync(storageDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll = await this.decisionPollService
                .AddDecisionPollAsync(inputDecisionPoll);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(inputDecisionPoll),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
