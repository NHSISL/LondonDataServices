// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldReturnTerminologyPolls()
        {
            // given
            IQueryable<TerminologyPoll> randomTerminologyPolls = CreateRandomTerminologyPolls();
            IQueryable<TerminologyPoll> storageTerminologyPolls = randomTerminologyPolls;
            IQueryable<TerminologyPoll> expectedTerminologyPolls = storageTerminologyPolls;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPollsAsync())
                    .ReturnsAsync(storageTerminologyPolls);

            // when
            IQueryable<TerminologyPoll> actualTerminologyPolls =
                await this.terminologyPollService.RetrieveAllTerminologyPollsAsync();

            // then
            actualTerminologyPolls.Should().BeEquivalentTo(expectedTerminologyPolls);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}