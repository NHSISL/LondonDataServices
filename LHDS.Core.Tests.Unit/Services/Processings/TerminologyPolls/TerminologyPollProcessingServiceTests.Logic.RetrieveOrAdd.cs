// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
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

            this.terminologyPollServiceMock.Setup(service =>
                service.RetrieveAllTerminologyPolls())
                    .Returns(storageTerminologyPolls);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomId);

            TerminologyPoll inputTerminologyPoll = new TerminologyPoll
            {
                Id = randomId,
                ResourceType = resouceType,
                LastPoll = DateTimeOffset.MinValue.AddMilliseconds(1),
                CreatedBy = "System",
                UpdatedBy = "System",
                UpdatedDate = randomDateTimeOffset,
                CreatedDate = randomDateTimeOffset
            };

            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(It.Is(SameTerminologyPollAs(inputTerminologyPoll))))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .RetrieveOrAddTerminologyPollAsync(resouceType);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.RetrieveAllTerminologyPolls(),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.Is(SameTerminologyPollAs(inputTerminologyPoll))),
                    Times.Once());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}