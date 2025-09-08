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
            string randomUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomUserId);

            DecisionPoll randomDecisionPoll = CreateRandomModifyDecisionPoll(
                randomDateTimeOffset,
                randomEntraUser.EntraUserId);

            DecisionPoll inputDecisionPoll = randomDecisionPoll;
            DecisionPoll storageDecisionPoll = inputDecisionPoll.DeepClone();
            storageDecisionPoll.UpdatedDate = randomDecisionPoll.CreatedDate;
            DecisionPoll auditAppliedDecisionPoll = inputDecisionPoll.DeepClone();
            auditAppliedDecisionPoll.UpdatedBy = randomUserId;
            auditAppliedDecisionPoll.UpdatedDate = randomDateTimeOffset;
            DecisionPoll auditEnsuredDecisionPoll = auditAppliedDecisionPoll.DeepClone();
            DecisionPoll updatedDecisionPoll = inputDecisionPoll;
            DecisionPoll expectedDecisionPoll = updatedDecisionPoll.DeepClone();
            Guid decisionPollId = inputDecisionPoll.Id;

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyModifyAuditValuesAsync(inputDecisionPoll))
                    .ReturnsAsync(auditAppliedDecisionPoll);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDecisionPollByIdAsync(decisionPollId))
                    .ReturnsAsync(storageDecisionPoll);

            this.securityAuditBrokerMock.Setup(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    auditAppliedDecisionPoll,
                    storageDecisionPoll))
                    .ReturnsAsync(auditEnsuredDecisionPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDecisionPollAsync(auditEnsuredDecisionPoll))
                    .ReturnsAsync(updatedDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll =
                await this.decisionPollService.ModifyDecisionPollAsync(inputDecisionPoll);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyModifyAuditValuesAsync(inputDecisionPoll),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDecisionPollByIdAsync(decisionPollId),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker => broker
                .EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                    auditAppliedDecisionPoll,
                    storageDecisionPoll),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDecisionPollAsync(auditEnsuredDecisionPoll),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
