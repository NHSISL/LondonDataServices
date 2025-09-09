// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DecisionPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollServiceTests
    {
        [Fact]
        public async Task ShouldReturnDecisionPollsAsync()
        {
            // given
            IQueryable<DecisionPoll> randomDecisionPolls = CreateRandomDecisionPolls();
            IQueryable<DecisionPoll> storageDecisionPolls = randomDecisionPolls;
            IQueryable<DecisionPoll> expectedDecisionPolls = storageDecisionPolls;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDecisionPollsAsync())
                    .ReturnsAsync(storageDecisionPolls);

            // when
            IQueryable<DecisionPoll> actualDecisionPolls =
                await this.decisionPollService.RetrieveAllDecisionPollsAsync();

            // then
            actualDecisionPolls.Should().BeEquivalentTo(expectedDecisionPolls);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDecisionPollsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
