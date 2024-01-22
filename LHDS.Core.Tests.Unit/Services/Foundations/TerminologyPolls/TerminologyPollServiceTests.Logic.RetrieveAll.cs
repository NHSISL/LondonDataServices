using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public void ShouldReturnTerminologyPolls()
        {
            // given
            IQueryable<TerminologyPoll> randomTerminologyPolls = CreateRandomTerminologyPolls();
            IQueryable<TerminologyPoll> storageTerminologyPolls = randomTerminologyPolls;
            IQueryable<TerminologyPoll> expectedTerminologyPolls = storageTerminologyPolls;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPolls())
                    .Returns(storageTerminologyPolls);

            // when
            IQueryable<TerminologyPoll> actualTerminologyPolls =
                this.terminologyPollService.RetrieveAllTerminologyPolls();

            // then
            actualTerminologyPolls.Should().BeEquivalentTo(expectedTerminologyPolls);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPolls(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}