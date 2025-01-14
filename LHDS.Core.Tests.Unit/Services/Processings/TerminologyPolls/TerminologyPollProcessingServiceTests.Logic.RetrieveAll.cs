// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task ShouldRetrieveAllTerminologyPollsAsync()
        {
            // given
            IQueryable<TerminologyPoll> randomTerminologyPolls = CreateRandomTerminologyPolls();
            IQueryable<TerminologyPoll> outputTerminologyPolls = randomTerminologyPolls;
            IQueryable<TerminologyPoll> expectedTerminologyPolls = outputTerminologyPolls.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPollsAsync())
                    .ReturnsAsync(outputTerminologyPolls);

            // when
            IQueryable<TerminologyPoll> actualTerminologyPolls = await this.terminologyPollProcessingService
                .RetrieveAllTerminologyPollsAsync();

            // then
            actualTerminologyPolls.Should().BeEquivalentTo(expectedTerminologyPolls);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPollsAsync(),
                    Times.Once());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}