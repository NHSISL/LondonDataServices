// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using LHDS.Core.Services.Orchestrations.Decisions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnGetPatientDecisionsIfBlobContainerIsNullAndLogItAsync()
        {
            // given
            var nullBlobContainersDecisionOrchestrationException =
                new NullBlobContainersDecisionOrchestrationException(
                    message: "Null blob container decision orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDecisionOrchestrationValidationException =
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersDecisionOrchestrationException);

            BlobContainers invalidBlobContainers = null;

            var invalidDecisionOrchestrationService = new DecisionOrchestrationService(
                decisionPollService: this.decisionPollServiceMock.Object,
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                hashBroker: this.hashBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                blobContainers: invalidBlobContainers,
                decisionConfiguration: this.decisionConfiguration);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                invalidDecisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationValidationException>(getPatientDecisionsTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecisionOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionOrchestrationValidationException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnGetPatientDecisionsIfDecisionConfigurationIsNullAndLogItAsync()
        {
            // given
            var nullDecisionConfigurationDecisionOrchestrationException =
                new NullDecisionConfigurationDecisionOrchestrationException(
                    message: "Null decision configuration decision orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDecisionOrchestrationValidationException =
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException: nullDecisionConfigurationDecisionOrchestrationException);

            DecisionConfiguration invalidDecisionConfiguration = null;

            var invalidDecisionOrchestrationService = new DecisionOrchestrationService(
                decisionPollService: this.decisionPollServiceMock.Object,
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                hashBroker: this.hashBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                blobContainers: this.blobContainers,
                decisionConfiguration: invalidDecisionConfiguration);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                invalidDecisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationValidationException>(getPatientDecisionsTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecisionOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionOrchestrationValidationException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnGetPatientDecisionsIfDecisionPollIsNullAndLogItAsync()
        {
            // given
            IQueryable<DecisionPoll> emptyDecisionPolls = Enumerable.Empty<DecisionPoll>().AsQueryable();
            Guid randomId = Guid.NewGuid();
            DecisionPoll nullDecisionPoll = null;

            var nullDecisionPollDecisionOrchestrationException =
                new NullDecisionPollDecisionOrchestrationException(message: "DecisionPoll is null.");

            var expectedDecisionOrchestrationValidationException =
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException: nullDecisionPollDecisionOrchestrationException);

            this.decisionPollServiceMock.Setup(service =>
                service.RetrieveAllDecisionPollsAsync())
                    .ReturnsAsync(emptyDecisionPolls);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomId);

            this.decisionPollServiceMock
                .Setup(service => service.AddDecisionPollAsync(It.Is<DecisionPoll>(poll => poll.Id == randomId)))
                .ReturnsAsync(nullDecisionPoll);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationValidationException actualDecisionOrchestrationValidationException =
                await Assert.ThrowsAsync<DecisionOrchestrationValidationException>(getPatientDecisionsTask.AsTask);

            // then
            actualDecisionOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedDecisionOrchestrationValidationException);

            this.decisionPollServiceMock.Verify(service =>
                service.RetrieveAllDecisionPollsAsync(),
                Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(), Times.Once);

            this.decisionPollServiceMock.Verify(service =>
                service.AddDecisionPollAsync(It.Is<DecisionPoll>(poll => poll.Id == randomId)),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionOrchestrationValidationException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnGetPatientDecisionsIfDocumentRequirementsAreNullOrEmptyAndLogItAsync()
        {
            // given
            IQueryable<DecisionPoll> randomDecisionPolls = CreateRandomDecisionPolls();
            IQueryable<DecisionPoll> decisionPolls = randomDecisionPolls;
            DateTimeOffset lastPollDate = decisionPolls.Max(poll => poll.LastPoll);
            List<Decision> expectedDecisions = CreateRandomDecisions();
            string expectedHash = GetRandomString();
            Dictionary<string, int> fieldMappings = GetFieldMappings();

            var invalidArgumentDecisionOrchestrationException =
                new InvalidArgumentDecisionOrchestrationException(
                    message: "Invalid Decision orchestration argument(s), please correct the errors and try again.");

            invalidArgumentDecisionOrchestrationException.AddData(
                key: "Content",
                values: "Text is required");

            var expectedDecisionOrchestrationValidationException =
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentDecisionOrchestrationException);

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
                            decision => decision.Id == csv.DecisionId && csv.NhsHash == expectedHash))),
                    true,
                    fieldMappings,
                    false))
                .ReturnsAsync(string.Empty);

            // when
            ValueTask<List<Decision>> getPatientDecisionsTask =
                this.decisionOrchestrationService.GetPatientDecisions();

            DecisionOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DecisionOrchestrationValidationException>(getPatientDecisionsTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDecisionOrchestrationValidationException);

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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDecisionOrchestrationValidationException))),
                        Times.Once);

            this.decisionPollServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
