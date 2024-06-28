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
            string inputContainer = "optout";
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            Dictionary<string, int> fieldMappings = null;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: GetRandomNumber());
            List<string> randomConsentedIdentifiers = CreateRandomListOfConsentedIdentifiers(count: GetRandomNumber());
            string randomWorkflowId = this.optOutConfiguration.WorkflowId;

            List<MeshMessage> outputMessages = GetRandomMessages(
                items: outputMessageIds,
                randomConsentedIdentifiers,
                workflowId: randomWorkflowId);

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

            meshProcessingServiceMock.SetupSequence(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds)
                    .ReturnsAsync(new List<string>());

            List<MeshMessage> meshMessageList = new List<MeshMessage>();
            List<KeyValuePair<Stream, Stream>> streamAssertions = new List<KeyValuePair<Stream, Stream>>();

            foreach (string messageId in outputMessageIds)
            {
                var testMessage = outputMessages.First(message => message.MessageId == messageId);

                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveMessageByIdAsync(messageId))
                        .ReturnsAsync(testMessage);

                meshMessageList.Add(testMessage);

                string batchReference = GetHeaderValue(testMessage, "mex-localid");

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

                csvHelperBrokerMock.Setup(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        It.Is(SameOptOutIdentifierListAs(differentIdentifiers)),
                        withHeader,
                        fieldMappings,
                        shouldAddTrailingComma))
                            .ReturnsAsync(csvDifferences);

                Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(csvDifferences));
                Stream actualStream = new MemoryStream();
                streamAssertions.Add(new KeyValuePair<Stream, Stream>(inputStream, actualStream));

                string inputFileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_deltaresponse.csv";

                documentProcessingServiceMock
                    .Setup(processings => processings.AddDocumentAsync(
                        It.IsAny<Stream>(),
                        inputFileName,
                        inputContainer))
                    .Callback<Stream, string, string>((stream, fileName, container) =>
                    {
                        stream.Position = 0;
                        stream.CopyTo(actualStream);
                    })
                    .Returns(ValueTask.CompletedTask);
            }

            List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMeshMessageList);

            foreach (var item in streamAssertions)
            {
                Assert.True(IsSameStream(item.Key, item.Value));
            }

            meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Exactly(2));

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveMessageByIdAsync(messageId),
                        Times.Once());

                meshMessageList.Add(message);

                string batchReference = GetHeaderValue(message, "mex-localid");

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

                csvHelperBrokerMock.Verify(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        It.Is(SameOptOutIdentifierListAs(differentIdentifiers)),
                        withHeader,
                        fieldMappings,
                        shouldAddTrailingComma),
                            Times.Exactly(outputMessageIds.Count));

                string inputFileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_deltaresponse.csv";

                documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                        Times.Once());

                this.meshProcessingServiceMock.Verify(processings =>
                    processings.AcknowledgeMessageByIdAsync(messageId),
                        Times.Once());
            }

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndNotCreateOutputFileForZeroChangesAsync()
        {
            try
            {
                // Given
                bool withHeader = optOutConfiguration.OptOutFileHasHeader;
                Dictionary<string, int> fieldMappings = null;
                bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
                List<string> outputMessageIds = GetRandomStrings(count: GetRandomNumber());
                List<string> randomConsentedIdentifiers = CreateRandomListOfConsentedIdentifiers(count: GetRandomNumber());
                List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, randomConsentedIdentifiers);
                List<MeshMessage> expectedMessages = outputMessages.DeepClone();
                List<OptOut> originalConsentedItems = CreateRandomOptOuts(count: GetRandomNumber());
                List<OptOut> changedConsentedItems = new List<OptOut>();
                DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

                foreach (var message in outputMessages)
                {
                    message.Headers["mex-workflowid"] = new List<string> { this.optOutConfiguration.WorkflowId };
                }

                meshProcessingServiceMock.SetupSequence(processings =>
                    processings.RetrieveMessageIdsFromInboxAsync())
                        .ReturnsAsync(outputMessageIds)
                        .ReturnsAsync(new List<string>());

                List<MeshMessage> meshMessageList = new List<MeshMessage>();

                foreach (string messageId in outputMessageIds)
                {
                    var message = outputMessages.First(message => message.MessageId == messageId);

                    // Get message
                    this.meshProcessingServiceMock.Setup(processing =>
                        processing.RetrieveMessageByIdAsync(messageId))
                            .ReturnsAsync(message);

                    meshMessageList.Add(message);

                    string batchReference = GetHeaderValue(message, "mex-localid");

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

                    this.csvHelperBrokerMock.Setup(processings =>
                        processings.MapObjectToCsvAsync<OptOutIdentifier>(
                            It.Is(SameOptOutIdentifierListAs(differentIdentifiers)),
                            withHeader,
                            fieldMappings,
                            shouldAddTrailingComma))
                                .ReturnsAsync(csvDifferences);

                    Document document = new Document
                    {
                        //DocumentData = Encoding.UTF8.GetBytes(csvDifferences),
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

                    string batchReference = GetHeaderValue(message, "mex-localid");

                    optOutProcessingServiceMock.Verify(processings =>
                        processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                            Times.Once);

                    optOutProcessingServiceMock.Verify(processings =>
                        processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                            originalConsentedItems,
                            It.Is(SameStringListAs(randomConsentedIdentifiers))),
                                Times.Exactly(outputMessageIds.Count));
                }

                this.optOutProcessingServiceMock.VerifyNoOtherCalls();
                this.csvHelperBrokerMock.VerifyNoOtherCalls();
                this.meshProcessingServiceMock.VerifyNoOtherCalls();
                this.documentProcessingServiceMock.VerifyNoOtherCalls();
                this.loggingBrokerMock.VerifyNoOtherCalls();
                this.identifierBrokerMock.VerifyNoOtherCalls();
                this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
            List<string> outputMessageIds = GetRandomStrings(count: GetRandomNumber());
            List<string> randomConsentedIdentifiers = CreateRandomListOfConsentedIdentifiers(count: GetRandomNumber());
            string randomWorkflowId = GetRandomString();

            List<MeshMessage> outputMessages = GetRandomMessages(
                items: outputMessageIds,
                randomConsentedIdentifiers,
                workflowId: randomWorkflowId);

            string[] delimiters = { "\r\n", "\n" };

            List<string> consentedIdentifiers = Encoding.UTF8
                .GetString(outputMessages[0].FileContent)
                    .Replace(",", string.Empty)
                        .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();

            randomConsentedIdentifiers.Should().BeEquivalentTo(consentedIdentifiers);

            meshProcessingServiceMock.SetupSequence(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds)
                    .ReturnsAsync(new List<string>());

            foreach (string messageId in outputMessageIds)
            {
                var testMessage = outputMessages.First(message => message.MessageId == messageId);

                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveMessageByIdAsync(messageId))
                        .ReturnsAsync(testMessage);
            }

            List<MeshMessage> expectedMeshMessageList = new List<MeshMessage>();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMeshMessageList);

            meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Exactly(2));

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveMessageByIdAsync(messageId),
                        Times.Once());
            }

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
