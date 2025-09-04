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
        public async Task ShouldRemoveDecisionPollByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            DecisionPoll randomDecisionPoll =
                CreateRandomDecisionPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Guid inputDecisionPollId = randomDecisionPoll.Id;
            DecisionPoll storageDecisionPoll = randomDecisionPoll;
            DecisionPoll ingestionTrackingWithDeleteAuditApplied = storageDecisionPoll.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            DecisionPoll updatedDecisionPoll = storageDecisionPoll;
            DecisionPoll deletedDecisionPoll = updatedDecisionPoll;
            DecisionPoll expectedDecisionPoll = deletedDecisionPoll.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPollId))
                    .ReturnsAsync(storageDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDecisionPollAsync(randomDecisionPoll))
                    .ReturnsAsync(updatedDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDecisionPollAsync(updatedDecisionPoll))
                    .ReturnsAsync(deletedDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll = await this.decisionPollService
                .RemoveDecisionPollByIdAsync(inputDecisionPollId);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(inputDecisionPollId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(randomDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDecisionPollAsync(updatedDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
