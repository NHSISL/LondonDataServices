// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldRemoveTerminologyPollByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll randomTerminologyPoll = 
                CreateRandomTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            Guid inputTerminologyPollId = randomTerminologyPoll.Id;
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll ingestionTrackingWithDeleteAuditApplied = storageTerminologyPoll.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            TerminologyPoll updatedTerminologyPoll = storageTerminologyPoll;
            TerminologyPoll deletedTerminologyPoll = updatedTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = deletedTerminologyPoll.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPollId))
                    .ReturnsAsync(storageTerminologyPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyPollAsync(randomTerminologyPoll))
                    .ReturnsAsync(updatedTerminologyPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTerminologyPollAsync(updatedTerminologyPoll))
                    .ReturnsAsync(deletedTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollService
                .RemoveTerminologyPollByIdAsync(inputTerminologyPollId);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPollId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(randomTerminologyPoll),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyPollAsync(updatedTerminologyPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}