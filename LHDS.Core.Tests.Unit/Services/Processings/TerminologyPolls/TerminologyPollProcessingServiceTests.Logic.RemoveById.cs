// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveTerminologyPollByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputTerminologyPollId = randomId;
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll deletedTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = deletedTerminologyPoll.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.RemoveTerminologyPollByIdAsync(inputTerminologyPollId))
                    .ReturnsAsync(deletedTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll =
                await this.terminologyPollProcessingService.RemoveTerminologyPollByIdAsync(inputTerminologyPollId);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.RemoveTerminologyPollByIdAsync(inputTerminologyPollId),
                    Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}