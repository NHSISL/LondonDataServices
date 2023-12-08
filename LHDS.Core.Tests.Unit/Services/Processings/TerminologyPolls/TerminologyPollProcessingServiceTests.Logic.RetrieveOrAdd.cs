using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;
using System.Linq;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveTerminologyPollIfOneExistsAndNotAddAsync()
        {
            // given
            string randomString = GetRandomString();
            string resouceType = randomString;
            IQueryable<TerminologyPoll> randomTerminologyPolls = CreateRandomTerminologyPolls();
            randomTerminologyPolls.First().ResourceType = resouceType;
            IQueryable<TerminologyPoll> inputTerminologyPolls = randomTerminologyPolls;
            IQueryable<TerminologyPoll> storageTerminologyPolls = inputTerminologyPolls;
            TerminologyPoll inputTerminologyPoll = inputTerminologyPolls.First();
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll;

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls())
                    .Returns(storageTerminologyPolls);

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveTerminologyPollByIdAsync(inputTerminologyPoll.Id))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .RetrieveOrAddTerminologyPollAsync(resouceType);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}