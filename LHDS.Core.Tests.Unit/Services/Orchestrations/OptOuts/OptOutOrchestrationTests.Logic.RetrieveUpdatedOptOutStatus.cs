// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Extensions.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndOutputChangeToFileAsync()
        {
            // Given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: 1);
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);
            MeshMessage firstMessage = outputMessages.FirstOrDefault();
            string randomWorkflowId = GetRandomWorkflowId();
            firstMessage.Headers["Mex-WorkflowID"] = new List<string> { randomWorkflowId };
            List<string> outputMessageIds = GetRandomStrings(count: GetRandomNumber());
            List<string> randomConsentedIdentifiers = CreateRandomListOfConsentedIdentifiers(count: GetRandomNumber());
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, randomConsentedIdentifiers);
            List<MeshMessage> expectedMessages = outputMessages.DeepClone();
            List<OptOut> originalConsentedItems = CreateRandomOptOuts(count: GetRandomNumber());
            List<OptOut> changedConsentedItems = CreateRandomOptOuts(count: GetRandomNumber());
            string[] delimiters = { "\r\n", "\n" };

            List<string> consentedIdentifiers = Encoding.UTF8
                .GetString(outputMessages[0].FileContent)
                    .Replace(",", string.Empty)
                        .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

            randomConsentedIdentifiers.Should().BeEquivalentTo(consentedIdentifiers);

            meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<MeshMessage> meshMessageList = new List<MeshMessage>();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                meshMessageList.Add(message);

                string batchReference = GetHeaderValue(message, "Mex-LocalID");

                optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(originalConsentedItems);

                optOutProcessingServiceMock.Setup(processings =>
                    processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                        originalConsentedItems,
                        It.Is(SameStringListAs(randomConsentedIdentifiers))))
                            .ReturnsAsync(changedConsentedItems);

                List<OptOutIdentifier> differentIdentifiers = changedConsentedItems
                    .Select(identifier => new OptOutIdentifier
                    {
                        NhsNumber = identifier.NhsNumber,
                        UniqueReference = identifier.UniqueReference,
                        Status = identifier.Status,
                        StatusChangedDateTime = identifier.CacheTime
                    }).ToList();

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                csvMapperProcessingServiceMock.Setup(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        It.Is(SameOptOutIdentifierListAs(differentIdentifiers)),
                        this.optOutConfiguration.OptOutFileHasHeader,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma))
                            .ReturnsAsync(csvDifferences);
            }

            List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMessages);

            meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveMessageByIdAsync(messageId),
                        Times.Once());

                meshMessageList.Add(message);

                string batchReference = GetHeaderValue(message, "Mex-LocalID");

                optOutProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                        Times.Once);

                optOutProcessingServiceMock.Verify(processings =>
                    processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                        originalConsentedItems,
                        It.Is(SameStringListAs(randomConsentedIdentifiers))),
                            Times.Exactly(outputMessageIds.Count));

                List<OptOutIdentifier> differentIdentifiers = changedConsentedItems
                    .Select(identifier => new OptOutIdentifier
                    {
                        NhsNumber = identifier.NhsNumber,
                        UniqueReference = identifier.UniqueReference,
                        Status = identifier.Status,
                        StatusChangedDateTime = identifier.CacheTime
                    }).ToList();

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    optOutConfiguration.OptOutFileRequireTrailingComma);

                csvMapperProcessingServiceMock.Verify(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        It.Is(SameOptOutIdentifierListAs(differentIdentifiers)),
                        this.optOutConfiguration.OptOutFileHasHeader,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma),
                            Times.Exactly(outputMessageIds.Count));

                Document document = new Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_deltaresponse.csv"
                };

                documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Exactly(outputMessageIds.Count));

                this.meshProcessingServiceMock.Verify(processings =>
                    processings.AcknowledgeMessageByIdAsync(messageId),
                        Times.Once());
            }

            meshProcessingServiceMock.VerifyNoOtherCalls();
            csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            optOutProcessingServiceMock.VerifyNoOtherCalls();
            documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndNotCreateOutputFileForZeroChangesAsync()
        {
            try
            {
                // Given
                bool withHeader = optOutConfiguration.OptOutFileHasHeader;
                List<string> outputMessageIds = GetRandomStrings(count: GetRandomNumber());
                List<string> randomConsentedIdentifiers = CreateRandomListOfConsentedIdentifiers(count: GetRandomNumber());
                List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, randomConsentedIdentifiers);
                List<MeshMessage> expectedMessages = outputMessages.DeepClone();
                List<OptOut> originalConsentedItems = CreateRandomOptOuts(count: GetRandomNumber());
                List<OptOut> changedConsentedItems = new List<OptOut>();
            // Given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: 1);
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);

            foreach (var message in outputMessages)
            {
                message.Headers["Mex-WorkflowID"] = new List<string> { this.meshConfiguration.WorkflowId };
            }

            List<MeshMessage> expectedMessages = outputMessages.DeepClone();

                meshProcessingServiceMock.Setup(processings =>
                    processings.RetrieveMessageIdsFromInboxAsync())
                        .ReturnsAsync(outputMessageIds);

                List<MeshMessage> meshMessageList = new List<MeshMessage>();

                foreach (string messageId in outputMessageIds)
                {
                    var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                    meshMessageList.Add(message);

                    string batchReference = GetHeaderValue(message, "Mex-LocalID");

                    optOutProcessingServiceMock.Setup(processings =>
                        processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                            .ReturnsAsync(originalConsentedItems);

                    optOutProcessingServiceMock.Setup(processings =>
                        processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                            originalConsentedItems,
                            It.Is(SameStringListAs(randomConsentedIdentifiers))))
                                .ReturnsAsync(changedConsentedItems);

                    List<OptOutIdentifier> differentIdentifiers = changedConsentedItems
                        .Select(identifier => new OptOutIdentifier
                        {
                            NhsNumber = identifier.NhsNumber,
                            UniqueReference = identifier.UniqueReference,
                            Status = identifier.Status,
                            StatusChangedDateTime = identifier.CacheTime
                        }).ToList();

                    string csvDifferences = CreateNewCsvList(
                        differentIdentifiers,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma);

                this.csvMapperProcessingServiceMock.Setup(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        It.Is(SameOptOutIdentifierListAs(differentIdentifiers)),
                        this.optOutConfiguration.OptOutFileHasHeader,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma))
                            .ReturnsAsync(csvDifferences);

                Document document = new Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_deltaresponse.csv",
                };

                this.meshProcessingServiceMock.Setup(processings =>
                    processings.AcknowledgeMessageByIdAsync(messageId));
            }

                List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

                // When
                List<MeshMessage> actualMeshMessageList =
                    await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

                // Then
                actualMeshMessageList.Should().BeEquivalentTo(expectedMessages);

                meshProcessingServiceMock.Verify(Processings =>
                    Processings.RetrieveMessageIdsFromInboxAsync(),
                        Times.Once);

                foreach (string messageId in outputMessageIds)
                {
                    var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveMessageByIdAsync(messageId),
                        Times.Once());

                    meshMessageList.Add(message);

                    string batchReference = GetHeaderValue(message, "Mex-LocalID");

                    optOutProcessingServiceMock.Verify(processings =>
                        processings.RetrieveMessageIdsFromInboxAsync(batchReference),
                            Times.Once);

                    optOutProcessingServiceMock.Verify(processings =>
                        processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                            originalConsentedItems,
                            It.Is(SameStringListAs(randomConsentedIdentifiers))),
                                Times.Exactly(outputMessageIds.Count));

                    csvMapperProcessingServiceMock.Verify(processings =>
                        processings.MapObjectToCsvAsync<OptOutIdentifier>(
                            It.IsAny<List<OptOutIdentifier>>(),
                            It.IsAny<bool>(),
                            It.IsAny<bool>()),
                                Times.Never);

                    documentProcessingServiceMock.Verify(processings =>
                        processings.AddDocumentAsync(It.IsAny<Document>()),
                            Times.Never);
                }

                meshProcessingServiceMock.VerifyNoOtherCalls();
                csvMapperProcessingServiceMock.VerifyNoOtherCalls();
                optOutProcessingServiceMock.VerifyNoOtherCalls();
                documentProcessingServiceMock.VerifyNoOtherCalls();
            }
            catch (System.Exception ex)
            {
                output.WriteLine($"Error: {ex.Message}, Validation: {ex.GetValidationSummary()}");
            }
        }

        [Fact]
        public async Task ShouldRetrieveMeshOptOutStatusesButExcludeUnmatchedWorkflowIdCacheAsync()
        {
            // Given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: 1);
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);
            MeshMessage firstMessage = outputMessages.FirstOrDefault();
            List<MeshMessage> expectedMessages = outputMessages.DeepClone();


            List<OptOutIdentifier> outputIdentifierUnknownList =
                CreateRandomListOfOptOutIdentifiers(count: 1);

            List<OptOutIdentifier> outputIdentifierConsentedList =
                CreateRandomListOfOptOutIdentifiers(count: 1);

            List<OptOutIdentifier> outputIdentifierNonConsentedList =
                CreateRandomListOfOptOutIdentifiers(count: 1);

            List<OptOutIdentifier> randomOutputIdentifierBatch = new List<OptOutIdentifier>();
            randomOutputIdentifierBatch.AddRange(outputIdentifierUnknownList);
            randomOutputIdentifierBatch.AddRange(outputIdentifierConsentedList);
            randomOutputIdentifierBatch.AddRange(outputIdentifierNonConsentedList);

            List<OptOut> outputBatch = new List<OptOut>();

            foreach (var message in outputMessages)
            {
                string batchReference = GetHeaderValue(message, "Mex-LocalID");

                List<OptOut> randomUnkownConsentBatch =
                    CreateRandomOptOutsList(outputIdentifierUnknownList, batchReference, "Unknown");

                List<OptOut> randomConsentBatch =
                    CreateRandomOptOutsList(outputIdentifierConsentedList, batchReference, "Opt-In");

                List<OptOut> randomNonConsentBatch =
                    CreateRandomOptOutsList(outputIdentifierNonConsentedList, batchReference, "Opt-In");

                outputBatch.AddRange(randomUnkownConsentBatch);
                outputBatch.AddRange(randomConsentBatch);
                outputBatch.AddRange(randomNonConsentBatch);
            }

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<MeshMessage> meshMessageList = new List<MeshMessage>();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveMessageByIdAsync(messageId))
                        .ReturnsAsync(message);
            }

            List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMeshMessageList);

            this.meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveMessageByIdAsync(messageId),
                        Times.Once());
            }

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

    }
}
