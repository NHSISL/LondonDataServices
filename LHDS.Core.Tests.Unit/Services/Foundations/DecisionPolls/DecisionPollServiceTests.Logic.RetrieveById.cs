// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveDecisionPollByIdAsync()
        {
            // given
            DecisionPoll randomDecisionPoll = CreateRandomDecisionPoll();
            DecisionPoll inputDecisionPoll = randomDecisionPoll;
            DecisionPoll storageDecisionPoll = randomDecisionPoll;
            DecisionPoll expectedDecisionPoll = storageDecisionPoll.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPoll.Id))
                    .ReturnsAsync(storageDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll =
                await this.decisionPollService.RetrieveDecisionPollByIdAsync(inputDecisionPoll.Id);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPoll.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
