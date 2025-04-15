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
        public async Task ShouldModifyTerminologyPollAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            TerminologyPoll randomTerminologyPoll =
                CreateRandomModifyTerminologyPoll(randomDateTimeOffset, randomEntraUser.EntraUserId);

            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll.DeepClone();
            storageTerminologyPoll.UpdatedDate = randomTerminologyPoll.CreatedDate;
            TerminologyPoll updatedTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = updatedTerminologyPoll.DeepClone();
            Guid terminologyPollId = inputTerminologyPoll.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(terminologyPollId))
                    .ReturnsAsync(storageTerminologyPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(updatedTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll =
                await this.terminologyPollService.ModifyTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPoll.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}