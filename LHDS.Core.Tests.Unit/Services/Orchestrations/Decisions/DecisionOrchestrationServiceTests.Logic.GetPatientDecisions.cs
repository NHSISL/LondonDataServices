// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldGetPatientDecisionsWithHashingAsync(bool hashNhsNumber)
        {
            // given
            this.decisionConfiguration.HashNhsNumber = hashNhsNumber;
            List<Decision> expectedDecisions = CreateRandomDecisions();
            string expectedContainer = this.blobContainers.Ingress;
            DateTimeOffset currentPollDate = DateTimeOffset.UtcNow;

            string expectedFileName = $"{this.decisionConfiguration.FolderName}/" +
                $"{currentPollDate:yyyyMMddHHmmss}/" +
                $"{this.decisionConfiguration.FilePrefix}_{currentPollDate:yyyyMMddHHmmss}.csv";

            string expectedHash = GetRandomString();
            Dictionary<string, int> fieldMappings = GetFieldMappings();

            this.decisionServiceMock
                .Setup(service => service.GetPatientDecisions())
                .ReturnsAsync(expectedDecisions);

            if (hashNhsNumber)
            {
                this.hashBrokerMock
                    .Setup(broker => broker.GenerateSha256HashAsync(
                        It.Is<string>(nhsNumber => expectedDecisions.Any(
                            decision => decision.PatientNhsNumber == nhsNumber)),
                        this.decisionConfiguration.HashPepper))
                    .ReturnsAsync(expectedHash);
            }

            this.csvHelperBrokerMock
                .Setup(broker => broker.MapObjectToCsvAsync(
                    It.IsAny<IAsyncEnumerable<DecisionCsv>>(),
                    It.IsAny<Stream>(),
                    true,
                    fieldMappings,
                    false,
                    It.IsAny<System.Threading.CancellationToken>()))
                .Returns(async (
                    IAsyncEnumerable<DecisionCsv> objects,
                    Stream stream,
                    bool addHeader,
                    Dictionary<string, int> mappings,
                    bool? trailingComma,
                    System.Threading.CancellationToken ct) =>
                {
                    await foreach (DecisionCsv _ in objects.WithCancellation(ct)) { }
                });

            this.documentServiceMock
                .Setup(service => service.AddDocumentAsync(
                    It.IsAny<Stream>(), expectedFileName, expectedContainer))
                .Returns(ValueTask.CompletedTask);

            this.loggingBrokerMock
                .Setup(broker => broker.LogInformationAsync(It.IsAny<string>()))
                .Returns(ValueTask.CompletedTask);

            this.decisionServiceMock
                .Setup(service => service.RecordAdoption(expectedDecisions))
                .Returns(ValueTask.CompletedTask);

            // when
            List<Decision> actualDecisions =
                await this.decisionOrchestrationService.GetPatientDecisions();

            // then
            actualDecisions.Should().NotBeNull();
            compareLogic.Compare(expectedDecisions, actualDecisions).AreEqual.Should().BeTrue();

            this.decisionServiceMock.Verify(service =>
                service.GetPatientDecisions(),
                    Times.Once);

            if (hashNhsNumber)
            {
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
                        It.IsAny<IAsyncEnumerable<DecisionCsv>>(),
                        It.IsAny<Stream>(),
                        true,
                        fieldMappings,
                        false,
                        It.IsAny<System.Threading.CancellationToken>()),
                        Times.Once);
            }
            else
            {
                this.hashBrokerMock.Verify(
                    broker => broker.GenerateSha256HashAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Never);

                this.csvHelperBrokerMock.Verify(broker =>
                    broker.MapObjectToCsvAsync(
                        It.IsAny<IAsyncEnumerable<DecisionCsv>>(),
                        It.IsAny<Stream>(),
                        true,
                        fieldMappings,
                        false,
                        It.IsAny<System.Threading.CancellationToken>()),
                    Times.Once);
            }

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    expectedFileName,
                    expectedContainer),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformationAsync(It.IsAny<string>()),
                Times.Once);

            this.decisionServiceMock.Verify(service =>
                service.RecordAdoption(expectedDecisions), Times.Once);

            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotGetPatientDecisionsAsync()
        {
            // given
            List<Decision> expectedDecisions = new List<Decision>(); // empty list

            this.decisionServiceMock
                .Setup(service => service.GetPatientDecisions())
                .ReturnsAsync(expectedDecisions);

            // when
            List<Decision> actualDecisions =
                await this.decisionOrchestrationService.GetPatientDecisions();

            // then
            actualDecisions.Should().BeEmpty();

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
                    Times.Never());
            }

            this.csvHelperBrokerMock.Verify(broker =>
                broker.MapObjectToCsvAsync(
                    It.IsAny<IAsyncEnumerable<DecisionCsv>>(),
                    It.IsAny<Stream>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool?>(),
                    It.IsAny<System.Threading.CancellationToken>()),
                Times.Never());

            this.documentServiceMock.Verify(service =>
                service.AddDocumentAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never());

            this.decisionServiceMock.Verify(service =>
                service.RecordAdoption(It.IsAny<List<Decision>>()), Times.Never());

            this.decisionServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}