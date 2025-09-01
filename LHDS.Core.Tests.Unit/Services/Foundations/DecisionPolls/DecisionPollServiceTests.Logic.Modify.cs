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
        public async Task ShouldModifyDecisionPollAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DecisionPoll randomDecisionPoll =
                CreateRandomModifyDecisionPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            DecisionPoll inputDecisionPoll = randomDecisionPoll;
            DecisionPoll storageDecisionPoll = inputDecisionPoll.DeepClone();
            storageDecisionPoll.UpdatedDate = randomDecisionPoll.CreatedDate;
            DecisionPoll updatedDecisionPoll = inputDecisionPoll;
            DecisionPoll expectedDecisionPoll = updatedDecisionPoll.DeepClone();
            Guid decisionPollId = inputDecisionPoll.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(decisionPollId))
                    .ReturnsAsync(storageDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDecisionPollAsync(inputDecisionPoll))
                    .ReturnsAsync(updatedDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll =
                await this.decisionPollService.ModifyDecisionPollAsync(inputDecisionPoll);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPoll.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(inputDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
