using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;
using System.Linq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;

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
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll.DeepClone();
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll;

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls())
                    .Returns(storageTerminologyPolls);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .RetrieveOrAddTerminologyPollAsync(resouceType);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Never());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddTerminologyPollIfTerminologyPollDoesNotExistsAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string randomString = GetRandomString();
            string resouceType = randomString;
            Guid randomId = Guid.NewGuid();
            IQueryable<TerminologyPoll> randomTerminologyPolls = CreateRandomTerminologyPolls();
            IQueryable<TerminologyPoll> storageTerminologyPolls = randomTerminologyPolls;

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            TerminologyPoll inputTerminologyPoll = new TerminologyPoll
            {
                Id = randomId,
                ResourceType = resouceType,
                LastPoll = randomDateTimeOffset,
                CreatedBy = "System",
                UpdatedBy = "System",
                UpdatedDate = randomDateTimeOffset,
                CreatedDate = randomDateTimeOffset
            };

            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls())
                    .Returns(storageTerminologyPolls);

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(It.Is(SameTerminologyPollAs(inputTerminologyPoll))))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .RetrieveOrAddTerminologyPollAsync(resouceType);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.Is(SameTerminologyPollAs(inputTerminologyPoll))),
                    Times.Once());

            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}