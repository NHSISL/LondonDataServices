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
        public async Task ShouldRetrieveTerminologyPollByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveTerminologyPollByIdAsync(inputId))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .RetrieveTerminologyPollByIdAsync(randomId);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveTerminologyPollByIdAsync(inputId),
                    Times.Once());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}