// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldModifyTerminologyPollAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll updatedTerminologyPoll = inputTerminologyPoll.DeepClone();
            updatedTerminologyPoll.UpdatedDate = randomTerminologyPoll.CreatedDate;
            TerminologyPoll expectedTerminologyPoll = updatedTerminologyPoll.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.ModifyTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(updatedTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll =
                await this.terminologyPollProcessingService.ModifyTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}