// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            DateTimeOffset lastPollDate = decisionPolls.Max(poll => poll.LastPoll);
            List<Decision> expectedDecisions = CreateRandomDecisions();
            string expectedDecisionsSerialized = JsonSerializer.Serialize(expectedDecisions);
            byte[] expectedDocumentBytes = Encoding.UTF8.GetBytes(expectedDecisionsSerialized);
            string expectedContainer = this.blobContainers.Decisions;
            DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;
            string systemUser = "system";
            string expectedFileName = $"IDecide_{currentPollDate:HHmm_ddMMyyyy}";

            DecisionPoll actualDecisionPoll = null;

            this.decisionPollServiceMock
                .Setup(service => service.RetrieveAllDecisionPollsAsync())
                .ReturnsAsync(decisionPolls);

            this.decisionServiceMock
                .Setup(service => service.GetPatientDecisions(lastPollDate))
                .ReturnsAsync(expectedDecisions);

            MemoryStream documentStream = new MemoryStream();

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(), expectedFileName, expectedContainer))
                .Callback<Stream, string, string>((stream, fileName, container) =>
                {
                    stream.Position = 0;
                    stream.CopyTo(documentStream);
                })
                .Returns(ValueTask.CompletedTask);

            this.decisionPollServiceMock
                .Setup(service => service.AddDecisionPollAsync(It.IsAny<DecisionPoll>()))
                .Callback<DecisionPoll>(poll => actualDecisionPoll = poll)
                .ReturnsAsync((DecisionPoll poll) => poll);

            this.decisionServiceMock
                .Setup(service => service.RecordAdoption(expectedDecisions))
                .Returns(ValueTask.CompletedTask);

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

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(It.IsAny<Stream>(), expectedFileName, expectedContainer), Times.Once);

            this.decisionPollServiceMock.Verify(service =>
                service.AddDecisionPollAsync(It.IsAny<DecisionPoll>()), Times.Once);

            this.decisionServiceMock.Verify(service =>
                service.RecordAdoption(expectedDecisions), Times.Once);

            ReadAllBytesFromStream(documentStream).Should().BeEquivalentTo(expectedDocumentBytes);
            actualDecisionPoll.Should().NotBeNull();
            actualDecisionPoll.CreatedBy.Should().Be(systemUser);
            actualDecisionPoll.UpdatedBy.Should().Be(systemUser);
            actualDecisionPoll.LastPoll.Should().BeCloseTo(currentPollDate, TimeSpan.FromSeconds(5));
            actualDecisionPoll.CreatedDate.Should().BeCloseTo(currentPollDate, TimeSpan.FromSeconds(5));
            actualDecisionPoll.UpdatedDate.Should().BeCloseTo(currentPollDate, TimeSpan.FromSeconds(5));

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
        }
    }
}