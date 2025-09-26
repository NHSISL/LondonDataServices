// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldGetPatientDecisionsAsync()
        {
            // given
            IQueryable<DecisionPoll> randomDecisionPolls = CreateRandomDecisionPolls();
            IQueryable<DecisionPoll> decisionPolls = randomDecisionPolls;

            DecisionPoll lastPoll = decisionPolls
                .OrderByDescending(poll => poll.LastPoll)
                .First();

            DateTimeOffset lastPollDate = lastPoll.LastPoll;
            List<Decision> expectedDecisions = CreateRandomDecisions();
            string expectedContainer = this.blobContainers.Ingress;
            DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;

            string expectedFileName = $"{this.decisionConfiguration.FolderName}/" +
                                      $"{currentPollDate:yyyyMMdd}/" +
                                      $"{this.decisionConfiguration.FilePrefix}_{currentPollDate:yyyyMMddHHmmss}.csv";

            string expectedHash = GetRandomString();
            string expectedCsvProcessedData = GetRandomString();
            byte[] expectedDocumentBytes = Encoding.UTF8.GetBytes(expectedCsvProcessedData);
            Stream tempDocument = new MemoryStream(expectedDocumentBytes);
            var documentStream = new MemoryStream();
            Dictionary<string, int> fieldMappings = GetFieldMappings();
            DateTimeOffset originalLastPollDate = lastPoll.LastPoll;
            DecisionPoll actualDecisionPoll = null;

            this.decisionPollServiceMock
                .Setup(service => service.RetrieveAllDecisionPollsAsync())
                .ReturnsAsync(decisionPolls);

            this.decisionServiceMock
                .Setup(service => service.GetPatientDecisions(lastPollDate))
                .ReturnsAsync(expectedDecisions);

            this.hashBrokerMock
                .Setup(broker => broker.GenerateSha256HashAsync(
                    It.Is<string>(nhsNumber => expectedDecisions.Any(
                        decision => decision.Patient.NhsNumber == nhsNumber)),
                    this.decisionConfiguration.HashPepper))
                .ReturnsAsync(expectedHash);

            this.csvHelperBrokerMock
                .Setup(broker => broker.MapObjectToCsvAsync(
                    It.Is<List<DecisionCsv>>(csvs =>
                        csvs.All(csv => expectedDecisions.Any(
                            decision => decision.Id == csv.DecisionId &&
                                csv.NhsHash == expectedHash))),
                    true,
                    fieldMappings,
                    false))
                .ReturnsAsync(expectedCsvProcessedData);

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.Is(SameStreamAs(tempDocument)), expectedFileName, expectedContainer))
                .Callback<Stream, string, string>((output, fileName, container) =>
                {
                    tempDocument.Position = 0;
                    tempDocument.CopyTo(documentStream);
                })
                .Returns(ValueTask.CompletedTask);

            this.decisionServiceMock
                .Setup(service => service.RecordAdoption(expectedDecisions))
                .Returns(ValueTask.CompletedTask);

            this.decisionPollServiceMock
                .Setup(service => service.ModifyDecisionPollAsync(
                    It.Is<DecisionPoll>(poll => poll.Id == lastPoll.Id)))
                .Callback<DecisionPoll>(poll => actualDecisionPoll = poll)
                .ReturnsAsync((DecisionPoll poll) => poll);

            // when
            List<Decision> actualDecisions =
                await this.decisionOrchestrationService.GetPatientDecisions();

            // then
            actualDecisions.Should().NotBeNull();
            compareLogic.Compare(expectedDecisions, actualDecisions).AreEqual.Should().BeTrue();

            this.decisionPollServiceMock.Verify(service =>
                service.RetrieveAllDecisionPollsAsync(), Times.Once);

            this.decisionServiceMock.Verify(service =>
                service.GetPatientDecisions(lastPollDate), Times.Once);

            foreach (string nhsNumber in expectedDecisions.Select(d => d.Patient.NhsNumber))
            {
                int count = expectedDecisions.Count(decision => decision.Patient.NhsNumber == nhsNumber);

                this.hashBrokerMock.Verify(
                    broker => broker.GenerateSha256HashAsync(
                        It.Is<string>(number => number == nhsNumber),
                        this.decisionConfiguration.HashPepper),
                        Times.Exactly(count));
            }

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync(
                    It.Is<List<DecisionCsv>>(csvs =>
                        csvs.All(csv => expectedDecisions.Any(
                            decision => decision.Id == csv.DecisionId &&
                                csv.NhsHash == expectedHash))),
                    true,
                    fieldMappings,
                    false),
                    Times.Once);

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    expectedFileName,
                    expectedContainer),
                    Times.Once);

            this.decisionServiceMock.Verify(service =>
                service.RecordAdoption(expectedDecisions), Times.Once);

            this.decisionPollServiceMock.Verify(service =>
                service.ModifyDecisionPollAsync(
                    It.Is<DecisionPoll>(poll => poll.Id == lastPoll.Id)),
                    Times.Once);

            documentStream.Position = 0;
            ReadAllBytesFromStream(documentStream).Should().BeEquivalentTo(expectedDocumentBytes);

            actualDecisionPoll.Should().NotBeNull();
            actualDecisionPoll.Id.Should().Be(lastPoll.Id);
            actualDecisionPoll.LastPoll.Should().BeAfter(originalLastPollDate);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}