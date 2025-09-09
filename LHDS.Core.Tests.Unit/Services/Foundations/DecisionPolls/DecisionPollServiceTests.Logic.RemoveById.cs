// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DecisionPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDecisionPollByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputDecisionPollId = randomId;
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            DecisionPoll storageDecisionPoll = randomDecisionPoll;
            DecisionPoll expectedInputDecisionPoll = storageDecisionPoll;
            DecisionPoll deletedDecisionPoll = expectedInputDecisionPoll;
            DecisionPoll expectedDecisionPoll = deletedDecisionPoll.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPollId))
                    .ReturnsAsync(storageDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDecisionPollAsync(expectedInputDecisionPoll))
                    .ReturnsAsync(deletedDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll = await this.decisionPollService
                .RemoveDecisionPollByIdAsync(inputDecisionPollId);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPollId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(expectedInputDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
