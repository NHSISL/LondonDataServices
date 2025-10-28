// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
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
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                hashBroker: this.hashBrokerMock.Object,
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
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                hashBroker: this.hashBrokerMock.Object,
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

            this.decisionServiceMock
                .Setup(service => service.GetPatientDecisions())
                .ReturnsAsync(expectedDecisions);

            this.hashBrokerMock
                .Setup(broker => broker.GenerateSha256HashAsync(
                    It.Is<string>(nhsNumber => expectedDecisions.Any(
                        decision => decision.PatientNhsNumber == nhsNumber)),
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

            this.decisionServiceMock.Verify(service =>
                service.GetPatientDecisions(),
                    Times.Once);

            foreach (string nhsNumber in expectedDecisions.Select(d => d.PatientNhsNumber))
            {
                int count = expectedDecisions.Count(decision => decision.PatientNhsNumber == nhsNumber);

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

            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
