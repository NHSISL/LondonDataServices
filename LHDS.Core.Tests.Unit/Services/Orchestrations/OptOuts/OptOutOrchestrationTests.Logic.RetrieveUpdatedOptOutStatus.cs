// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            List<string> outputMessageIds = GetRandomStrings(count: GetRandomNumber());
            List<string> randomConsentedIdentifiers = CreateRandomListOfConsentedIdentifiers(count: GetRandomNumber());
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, randomConsentedIdentifiers);
            List<MeshMessage> expectedMessages = outputMessages.DeepClone();
            List<OptOut> originalConsentedItems = CreateRandomOptOuts(count: GetRandomNumber());
            List<OptOut> changedConsentedItems = CreateRandomOptOuts(count: GetRandomNumber());

            meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<MeshMessage> meshMessageList = new List<MeshMessage>();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                meshMessageList.Add(message);

                string batchReference = GetHeaderValue(message, "Mex-LocalID");

                optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(originalConsentedItems);

                optOutProcessingServiceMock.Setup(processings =>
                    processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                        originalConsentedItems,
                        randomConsentedIdentifiers))
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

                meshProcessingServiceMock.Verify(processing =>
                    processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
                        Times.Once);

                meshMessageList.Add(message);

                string batchReference = GetHeaderValue(message, "Mex-LocalID");

                optOutProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                        Times.Once);

                optOutProcessingServiceMock.Verify(processings =>
                    processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                        originalConsentedItems,
                        It.IsAny<List<string>>()), //randomConsentedIdentifiers),
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
                        optOutConfiguration.OptOutFileHasHeader,
                        optOutConfiguration.OptOutFileRequireTrailingComma),
                            Times.Exactly(outputMessageIds.Count));

                Document document = new Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_deltaresponse.csv"
                };

                documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Once);
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

                meshProcessingServiceMock.Setup(processings =>
                    processings.RetrieveMessageIdsFromInboxAsync())
                        .ReturnsAsync(outputMessageIds);

                List<MeshMessage> meshMessageList = new List<MeshMessage>();

                foreach (string messageId in outputMessageIds)
                {
                    var message = outputMessages.First(message => message.MessageId == messageId);

                    meshProcessingServiceMock.Setup(processing =>
                        processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                            .ReturnsAsync(message);

                    meshMessageList.Add(message);

                    string batchReference = GetHeaderValue(message, "Mex-LocalID");

                    optOutProcessingServiceMock.Setup(processings =>
                        processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                            .ReturnsAsync(originalConsentedItems);

                    optOutProcessingServiceMock.Setup(processings =>
                        processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                            It.Is(SameOptOutListAs(originalConsentedItems)),
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

                    meshProcessingServiceMock.Verify(processing =>
                        processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
                            Times.Once);

                    meshMessageList.Add(message);

                    string batchReference = GetHeaderValue(message, "Mex-LocalID");

                    optOutProcessingServiceMock.Verify(processings =>
                        processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                            Times.Once);

                    optOutProcessingServiceMock.Verify(processings =>
                        processings.ConsolidateOptOutChangesAndReturnChangesOnly(
                            It.Is(SameOptOutListAs(originalConsentedItems)),
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
    }
}
