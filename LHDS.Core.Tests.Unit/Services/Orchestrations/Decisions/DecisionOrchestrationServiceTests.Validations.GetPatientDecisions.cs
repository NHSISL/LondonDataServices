// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
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

            }
        }
