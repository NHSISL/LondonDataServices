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
            string randomUserId = GetRandomString();
            EntraUser randomEntraUser = CreateRandomEntraUser(entraUserId: randomUserId);

            DecisionPoll randomDecisionPoll =
                CreateRandomDecisionPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            DecisionPoll inputDecisionPoll = randomDecisionPoll;
            DecisionPoll auditAppliedDecisionPoll = inputDecisionPoll.DeepClone();
            auditAppliedDecisionPoll.CreatedBy = randomUserId;
            auditAppliedDecisionPoll.CreatedDate = randomDateTimeOffset;
            auditAppliedDecisionPoll.UpdatedBy = randomUserId;
            auditAppliedDecisionPoll.UpdatedDate = randomDateTimeOffset;
            DecisionPoll storageDecisionPoll = auditAppliedDecisionPoll.DeepClone();
            DecisionPoll expectedDecisionPoll = storageDecisionPoll.DeepClone();

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyAddAuditValuesAsync(inputDecisionPoll))
                    .ReturnsAsync(auditAppliedDecisionPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDecisionPollAsync(auditAppliedDecisionPoll))
                    .ReturnsAsync(storageDecisionPoll);

            // when
            DecisionPoll actualDecisionPoll =
                await this.decisionPollService.AddDecisionPollAsync(inputDecisionPoll);

            // then
            actualDecisionPoll.Should().BeEquivalentTo(expectedDecisionPoll);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyAddAuditValuesAsync(inputDecisionPoll),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDecisionPollAsync(auditAppliedDecisionPoll),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
