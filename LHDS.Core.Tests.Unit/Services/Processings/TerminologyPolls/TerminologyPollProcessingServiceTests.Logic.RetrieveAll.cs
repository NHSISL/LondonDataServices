// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public void ShouldRetrieveAllTerminologyPollsAsync()
        {
            // given
            IQueryable<TerminologyPoll> randomTerminologyPolls = CreateRandomTerminologyPolls();
            IQueryable<TerminologyPoll> outputTerminologyPolls = randomTerminologyPolls;
            IQueryable<TerminologyPoll> expectedTerminologyPolls = outputTerminologyPolls.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls())
                    .Returns(outputTerminologyPolls);

            // when
            IQueryable<TerminologyPoll> actualTerminologyPolls = this.terminologyPollProcessingService
                .RetrieveAllTerminologyPolls();

            // then
            actualTerminologyPolls.Should().BeEquivalentTo(expectedTerminologyPolls);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}