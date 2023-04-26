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
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndUpdateCacheAsync()
        {
            // Given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: 1);
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);
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
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();
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

            // Given
            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<MeshMessage> meshMessageList = new List<MeshMessage>();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                meshMessageList.Add(message);

                // Map message content to object
                this.csvMapperProcessingServiceMock.Setup(processing =>
                    processing.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
                        .ReturnsAsync(outputIdentifierConsentedList);

                // Get original batch storage
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> batchSpecificOptOuts =
                    outputBatch.Where(optout => optout.BatchReference == batchReference)
                        .ToList()
                            .DeepClone();

                this.optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(batchSpecificOptOuts);

                // Use the consented list to only get the items that need to be opt-in from the storage
                List<string> consentedIdentifiers = outputIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> expectedBatchSpecificOptOuts = batchSpecificOptOuts.DeepClone();

                List<OptOut> consentedItems =
                    expectedBatchSpecificOptOuts.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber))
                        .ToList();

                // The non-consented items is the remainder (including the unknown ones)
                List<OptOut> nonConsentedItems =
                    expectedBatchSpecificOptOuts.Except(consentedItems).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (var item in consentedItems)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        delta.Add(item);
                    }

                    item.UpdatedDate = randomDateTimeOffset;
                    item.CacheTime = randomDateTimeOffset;
                    item.LastSentToMesh = randomDateTimeOffset;
                    item.OptOutStatus = "Opt-In";

                    this.optOutProcessingServiceMock.Setup(processing =>
                        processing.ModifyOptOutAsync(item))
                            .ReturnsAsync(item);
                }

                foreach (var item in nonConsentedItems)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        delta.Add(item);
                    }

                    item.UpdatedDate = randomDateTimeOffset;
                    item.CacheTime = randomDateTimeOffset;
                    item.LastSentToMesh = randomDateTimeOffset;
                    item.OptOutStatus = "Opt-Out";

                    this.optOutProcessingServiceMock.Setup(processing =>
                        processing.ModifyOptOutAsync(item))
                            .ReturnsAsync(item);
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                    .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

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
            }

            List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMessages);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                    Times.AtLeastOnce());

            this.meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
                        Times.Once());

                // Map message content to object
                this.csvMapperProcessingServiceMock.Verify(processings =>
                    processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader),
                        Times.Once());

                // Get original batch storage
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> batchSpecificOptOuts =
                    outputBatch.Where(optout => optout.BatchReference == batchReference)
                        .ToList().DeepClone();

                this.optOutProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                        Times.Once());

                // Use the consented list to only get the items that need to be opt-in from the storage
                List<string> consentedIdentifiers = outputIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> expectedBatchSpecificOptOuts = batchSpecificOptOuts.DeepClone();

                List<OptOut> consentedItems =
                    expectedBatchSpecificOptOuts.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber))
                        .ToList();

                // The non-consented items is the remainder (including the unknown ones)
                List<OptOut> nonConsentedItems =
                    expectedBatchSpecificOptOuts.Except(consentedItems).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (OptOut consentedItem in consentedItems)
                {
                    if (consentedItem.OptOutStatus != "Opt-In")
                    {
                        delta.Add(consentedItem);
                    }

                    consentedItem.UpdatedDate = randomDateTimeOffset;
                    consentedItem.CacheTime = randomDateTimeOffset;
                    consentedItem.LastSentToMesh = randomDateTimeOffset;
                    consentedItem.OptOutStatus = "Opt-In";

                    this.optOutProcessingServiceMock.Verify(processings =>
                        processings.ModifyOptOutAsync(It.Is(SameOptOutAs(consentedItem))),
                            Times.Exactly(outputMessageIds.Count));
                }

                foreach (OptOut nonConsentedItem in nonConsentedItems)
                {
                    if (nonConsentedItem.OptOutStatus != "Opt-Out")
                    {
                        delta.Add(nonConsentedItem);
                    }

                    nonConsentedItem.UpdatedDate = randomDateTimeOffset;
                    nonConsentedItem.CacheTime = randomDateTimeOffset;
                    nonConsentedItem.LastSentToMesh = randomDateTimeOffset;
                    nonConsentedItem.OptOutStatus = "Opt-Out";

                    this.optOutProcessingServiceMock.Verify(processings =>
                        processings.ModifyOptOutAsync(It.Is(SameOptOutAs(nonConsentedItem))),
                            Times.Exactly(outputMessageIds.Count));
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                   .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                delta.Count.Should().Be(batchSpecificOptOuts.Count - consentedItems.Count);

                this.csvMapperProcessingServiceMock.Verify(processings =>
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

                this.documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Exactly(outputMessageIds.Count));
            }

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndMarkUnkownsAsConsentedAndWriteDeltaAsync()
        {
            // Given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: 1);
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);
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
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();
                List<OptOut> randomUnkownConsentBatch = new List<OptOut>();

                List<OptOut> randomConsentBatch =
                    CreateRandomOptOutsList(outputIdentifierConsentedList, batchReference, "Unknown");

                List<OptOut> randomNonConsentBatch = new List<OptOut>();

                outputBatch.AddRange(randomUnkownConsentBatch);
                outputBatch.AddRange(randomConsentBatch);
                outputBatch.AddRange(randomNonConsentBatch);
            }

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // Given
            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<MeshMessage> meshMessageList = new List<MeshMessage>();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                meshMessageList.Add(message);

                // Map message content to object
                this.csvMapperProcessingServiceMock.Setup(processing =>
                    processing.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
                        .ReturnsAsync(outputIdentifierConsentedList);

                // Get original batch storage
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> batchSpecificOptOuts =
                    outputBatch.Where(optout => optout.BatchReference == batchReference)
                        .ToList()
                            .DeepClone();

                this.optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(batchSpecificOptOuts);

                // Use the consented list to only get the items that need to be opt-in from the storage
                List<string> consentedIdentifiers = outputIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> expectedBatchSpecificOptOuts = batchSpecificOptOuts.DeepClone();

                List<OptOut> consentedItems =
                    expectedBatchSpecificOptOuts.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber))
                        .ToList();

                // The non-consented items is the remainder (including the unknown ones)
                List<OptOut> nonConsentedItems =
                    expectedBatchSpecificOptOuts.Except(consentedItems).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (var item in consentedItems)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        delta.Add(item);
                    }

                    item.UpdatedDate = randomDateTimeOffset;
                    item.CacheTime = randomDateTimeOffset;
                    item.LastSentToMesh = randomDateTimeOffset;
                    item.OptOutStatus = "Opt-In";

                    this.optOutProcessingServiceMock.Setup(processing =>
                        processing.ModifyOptOutAsync(item))
                            .ReturnsAsync(item);
                }

                foreach (var item in nonConsentedItems)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        delta.Add(item);
                    }

                    item.UpdatedDate = randomDateTimeOffset;
                    item.CacheTime = randomDateTimeOffset;
                    item.LastSentToMesh = randomDateTimeOffset;
                    item.OptOutStatus = "Opt-Out";

                    this.optOutProcessingServiceMock.Setup(processing =>
                        processing.ModifyOptOutAsync(item))
                            .ReturnsAsync(item);
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                    .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

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
            }

            List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMessages);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                    Times.AtLeastOnce());

            this.meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
                        Times.Once());

                // Map message content to object
                this.csvMapperProcessingServiceMock.Verify(processings =>
                    processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader),
                        Times.Once());

                // Get original batch storage
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> batchSpecificOptOuts =
                    outputBatch.Where(optout => optout.BatchReference == batchReference)
                        .ToList().DeepClone();

                this.optOutProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                        Times.Once());

                // Use the consented list to only get the items that need to be opt-in from the storage
                List<string> consentedIdentifiers = outputIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> expectedBatchSpecificOptOuts = batchSpecificOptOuts.DeepClone();

                List<OptOut> consentedItems =
                    expectedBatchSpecificOptOuts.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber))
                        .ToList();

                // The non-consented items is the remainder (including the unknown ones)
                List<OptOut> nonConsentedItems =
                    expectedBatchSpecificOptOuts.Except(consentedItems).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (OptOut consentedItem in consentedItems)
                {
                    if (consentedItem.OptOutStatus != "Opt-In")
                    {
                        delta.Add(consentedItem);
                    }

                    consentedItem.UpdatedDate = randomDateTimeOffset;
                    consentedItem.CacheTime = randomDateTimeOffset;
                    consentedItem.LastSentToMesh = randomDateTimeOffset;
                    consentedItem.OptOutStatus = "Opt-In";

                    this.optOutProcessingServiceMock.Verify(processings =>
                        processings.ModifyOptOutAsync(It.Is(SameOptOutAs(consentedItem))),
                            Times.Exactly(outputMessageIds.Count));
                }

                foreach (OptOut nonConsentedItem in nonConsentedItems)
                {
                    if (nonConsentedItem.OptOutStatus != "Opt-Out")
                    {
                        delta.Add(nonConsentedItem);
                    }

                    nonConsentedItem.UpdatedDate = randomDateTimeOffset;
                    nonConsentedItem.CacheTime = randomDateTimeOffset;
                    nonConsentedItem.LastSentToMesh = randomDateTimeOffset;
                    nonConsentedItem.OptOutStatus = "Opt-Out";

                    this.optOutProcessingServiceMock.Verify(processings =>
                        processings.ModifyOptOutAsync(It.Is(SameOptOutAs(nonConsentedItem))),
                            Times.Exactly(outputMessageIds.Count));
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                   .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                delta.Count.Should().Be(consentedItems.Count);

                this.csvMapperProcessingServiceMock.Verify(processings =>
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

                this.documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Exactly(outputMessageIds.Count));
            }

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndMarkUnkownsAsNonConsentedAndWriteDeltaAsync()
        {
            // Given
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(count: 1);
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds);
            List<MeshMessage> expectedMessages = outputMessages.DeepClone();

            List<OptOutIdentifier> outputIdentifierUnknownList =
                CreateRandomListOfOptOutIdentifiers(count: 1);

            List<OptOutIdentifier> outputIdentifierConsentedList = new List<OptOutIdentifier>();

            List<OptOutIdentifier> outputIdentifierNonConsentedList = new List<OptOutIdentifier>();

            List<OptOutIdentifier> randomOutputIdentifierBatch = new List<OptOutIdentifier>();
            randomOutputIdentifierBatch.AddRange(outputIdentifierUnknownList);
            randomOutputIdentifierBatch.AddRange(outputIdentifierConsentedList);
            randomOutputIdentifierBatch.AddRange(outputIdentifierNonConsentedList);

            List<OptOut> outputBatch = new List<OptOut>();

            foreach (var message in outputMessages)
            {
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> randomUnkownConsentBatch =
                    CreateRandomOptOutsList(outputIdentifierUnknownList, batchReference, "Unknown");

                List<OptOut> randomConsentBatch =
                    new List<OptOut>();

                List<OptOut> randomNonConsentBatch = new List<OptOut>();

                outputBatch.AddRange(randomUnkownConsentBatch);
                outputBatch.AddRange(randomConsentBatch);
                outputBatch.AddRange(randomNonConsentBatch);
            }

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // Given
            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<MeshMessage> meshMessageList = new List<MeshMessage>();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processing =>
                    processing.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                meshMessageList.Add(message);

                // Map message content to object
                this.csvMapperProcessingServiceMock.Setup(processing =>
                    processing.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
                        .ReturnsAsync(outputIdentifierConsentedList);

                // Get original batch storage
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> batchSpecificOptOuts =
                    outputBatch.Where(optout => optout.BatchReference == batchReference)
                        .ToList()
                            .DeepClone();

                this.optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(batchSpecificOptOuts);

                // Use the consented list to only get the items that need to be opt-in from the storage
                List<string> consentedIdentifiers = outputIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> expectedBatchSpecificOptOuts = batchSpecificOptOuts.DeepClone();

                List<OptOut> consentedItems =
                    expectedBatchSpecificOptOuts.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber))
                        .ToList();

                // The non-consented items is the remainder (including the unknown ones)
                List<OptOut> nonConsentedItems =
                    expectedBatchSpecificOptOuts.Except(consentedItems).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (var item in consentedItems)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        delta.Add(item);
                    }

                    item.UpdatedDate = randomDateTimeOffset;
                    item.CacheTime = randomDateTimeOffset;
                    item.LastSentToMesh = randomDateTimeOffset;
                    item.OptOutStatus = "Opt-In";

                    this.optOutProcessingServiceMock.Setup(processing =>
                        processing.ModifyOptOutAsync(item))
                            .ReturnsAsync(item);
                }

                foreach (var item in nonConsentedItems)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        delta.Add(item);
                    }

                    item.UpdatedDate = randomDateTimeOffset;
                    item.CacheTime = randomDateTimeOffset;
                    item.LastSentToMesh = randomDateTimeOffset;
                    item.OptOutStatus = "Opt-Out";

                    this.optOutProcessingServiceMock.Setup(processing =>
                        processing.ModifyOptOutAsync(item))
                            .ReturnsAsync(item);
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                    .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

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
            }

            List<MeshMessage> expectedMeshMessageList = meshMessageList.DeepClone();

            // When
            List<MeshMessage> actualMeshMessageList =
                await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            // Then
            actualMeshMessageList.Should().BeEquivalentTo(expectedMessages);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(),
                    Times.AtLeastOnce());

            this.meshProcessingServiceMock.Verify(Processings =>
                Processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
                        Times.Once());

                // Map message content to object
                this.csvMapperProcessingServiceMock.Verify(processings =>
                    processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader),
                        Times.Once());

                // Get original batch storage
                string batchReference = message.Headers["Mex-LocalID"].FirstOrDefault();

                List<OptOut> batchSpecificOptOuts =
                    outputBatch.Where(optout => optout.BatchReference == batchReference)
                        .ToList().DeepClone();

                this.optOutProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                        Times.Once());

                // Use the consented list to only get the items that need to be opt-in from the storage
                List<string> consentedIdentifiers = outputIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

                List<OptOut> expectedBatchSpecificOptOuts = batchSpecificOptOuts.DeepClone();

                List<OptOut> consentedItems =
                    expectedBatchSpecificOptOuts.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber))
                        .ToList();

                // The non-consented items is the remainder (including the unknown ones)
                List<OptOut> nonConsentedItems =
                    expectedBatchSpecificOptOuts.Except(consentedItems).ToList();

                List<OptOut> delta = new List<OptOut>();

                foreach (OptOut consentedItem in consentedItems)
                {
                    if (consentedItem.OptOutStatus != "Opt-In")
                    {
                        delta.Add(consentedItem);
                    }

                    consentedItem.UpdatedDate = randomDateTimeOffset;
                    consentedItem.CacheTime = randomDateTimeOffset;
                    consentedItem.LastSentToMesh = randomDateTimeOffset;
                    consentedItem.OptOutStatus = "Opt-In";

                    this.optOutProcessingServiceMock.Verify(processings =>
                        processings.ModifyOptOutAsync(It.Is(SameOptOutAs(consentedItem))),
                            Times.Exactly(outputMessageIds.Count));
                }

                foreach (OptOut nonConsentedItem in nonConsentedItems)
                {
                    if (nonConsentedItem.OptOutStatus != "Opt-Out")
                    {
                        delta.Add(nonConsentedItem);
                    }

                    nonConsentedItem.UpdatedDate = randomDateTimeOffset;
                    nonConsentedItem.CacheTime = randomDateTimeOffset;
                    nonConsentedItem.LastSentToMesh = randomDateTimeOffset;
                    nonConsentedItem.OptOutStatus = "Opt-Out";

                    this.optOutProcessingServiceMock.Verify(processings =>
                        processings.ModifyOptOutAsync(It.Is(SameOptOutAs(nonConsentedItem))),
                            Times.Exactly(outputMessageIds.Count));
                }

                List<OptOutIdentifier> differentIdentifiers = delta
                   .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                delta.Count.Should().Be(nonConsentedItems.Count);

                this.csvMapperProcessingServiceMock.Verify(processings =>
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

                this.documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Exactly(outputMessageIds.Count));
            }

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        //[Fact]
        //public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndMarkOptOutAsOptInAndWriteDeltaAsync()
        //{
        //    // Given
        //    bool withHeader = optOutConfiguration.OptOutFileHasHeader;
        //    string batchReference = GetRandomString();
        //    DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
        //    List<string> outputMessageIds = GetRandomStrings(GetRandomNumber(max: 3));
        //    List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, batchReference);

        //    List<OptOutIdentifier> outputIdentifierUnknownList =
        //        new List<OptOutIdentifier>();

        //    List<OptOutIdentifier> outputIdentifierConsentedList =
        //        CreateRandomListOfOptOutIdentifiers(GetRandomNumber(max: 3));

        //    List<OptOutIdentifier> outputIdentifierNonConsentedList =
        //        new List<OptOutIdentifier>();

        //    List<OptOutIdentifier> randomOutputIdentifierBatch = new List<OptOutIdentifier>();
        //    randomOutputIdentifierBatch.AddRange(outputIdentifierUnknownList);
        //    randomOutputIdentifierBatch.AddRange(outputIdentifierConsentedList);
        //    randomOutputIdentifierBatch.AddRange(outputIdentifierNonConsentedList);

        //    List<OptOut> randomUnkownConsentBatch =
        //        new List<OptOut>();

        //    List<OptOut> randomConsentBatch =
        //        new List<OptOut>();

        //    List<OptOut> randomNonConsentBatch =
        //        CreateRandomOptOutsList(outputIdentifierConsentedList, batchReference, "Unknown");

        //    List<OptOut> outputBatch = new List<OptOut>();
        //    outputBatch.AddRange(randomUnkownConsentBatch);
        //    outputBatch.AddRange(randomConsentBatch);
        //    outputBatch.AddRange(randomNonConsentBatch);

        //    List<string> consentedIdentifiers = outputIdentifierConsentedList
        //        .Select(identifier => identifier.NhsNumber).ToList();

        //    List<OptOut> consentedItems =
        //        outputBatch.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber)).ToList();

        //    List<OptOut> nonConsentedItems =
        //        outputBatch.Except(consentedItems).ToList();

        //    List<OptOut> differences = new List<OptOut>();

        //    List<OptOutIdentifier> differentIdentifiers = differences
        //        .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

        //    this.dateTimeBrokerMock.Setup(broker =>
        //        broker.GetCurrentDateTimeOffset())
        //            .Returns(randomDateTimeOffset);

        //    // Given
        //    this.meshProcessingServiceMock.Setup(processings =>
        //        processings.RetrieveMessageIdsFromInboxAsync())
        //            .ReturnsAsync(outputMessageIds);

        //    foreach (string messageId in outputMessageIds)
        //    {
        //        var message = outputMessages.First(message => message.MessageId == messageId);

        //        // Get message
        //        this.meshProcessingServiceMock.Setup(processings =>
        //            processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
        //                .ReturnsAsync(message);

        //        // Map message content to object
        //        this.csvMapperProcessingServiceMock.Setup(processings =>
        //            processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
        //                .ReturnsAsync(outputIdentifierConsentedList);

        //        // Get original batch
        //        this.optOutProcessingServiceMock.Setup(processings =>
        //            processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
        //                .ReturnsAsync(outputBatch);

        //        string csvDifferences = CreateNewCsvList(
        //            differentIdentifiers,
        //            this.optOutConfiguration.OptOutFileRequireTrailingComma);

        //        this.csvMapperProcessingServiceMock.Setup(processings =>
        //            processings.MapObjectToCsvAsync<OptOutIdentifier>(
        //                differentIdentifiers,
        //                this.optOutConfiguration.OptOutFileHasHeader,
        //                this.optOutConfiguration.OptOutFileRequireTrailingComma))
        //                    .ReturnsAsync(csvDifferences);

        //        foreach (var item in consentedItems)
        //        {
        //            if (item.OptOutStatus != "Opt-In")
        //            {
        //                differences.Add(item);
        //            }

        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-In";

        //            this.optOutProcessingServiceMock.Setup(processings =>
        //                processings.ModifyOptOutAsync(item))
        //                    .ReturnsAsync(item);
        //        }

        //        foreach (var item in nonConsentedItems)
        //        {
        //            if (item.OptOutStatus != "Opt-Out")
        //            {
        //                differences.Add(item);
        //            }

        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-Out";

        //            this.optOutProcessingServiceMock.Setup(processings =>
        //                processings.ModifyOptOutAsync(item))
        //                    .ReturnsAsync(item);
        //        }

        //        Document document = new Document
        //        {
        //            DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
        //            FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
        //                $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
        //        };
        //    }

        //    // When
        //    await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

        //    // Then
        //    this.dateTimeBrokerMock.Verify(broker =>
        //       broker.GetCurrentDateTimeOffset(),
        //            Times.AtLeastOnce());

        //    this.meshProcessingServiceMock.Verify(Processings =>
        //        Processings.RetrieveMessageIdsFromInboxAsync(),
        //            Times.Once);

        //    foreach (string messageId in outputMessageIds)
        //    {
        //        var message = outputMessages.First(message => message.MessageId == messageId);

        //        // Get message
        //        this.meshProcessingServiceMock.Verify(processings =>
        //            processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
        //                Times.Once());

        //        // Map message content to object
        //        this.csvMapperProcessingServiceMock.Verify(processings =>
        //            processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader),
        //                Times.Once());

        //        // Get original batch
        //        this.optOutProcessingServiceMock.Verify(processings =>
        //            processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
        //                Times.Exactly(outputMessageIds.Count));

        //        foreach (var item in consentedItems)
        //        {
        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-In";

        //            this.optOutProcessingServiceMock.Verify(processings =>
        //                processings.ModifyOptOutAsync(item),
        //                    Times.Exactly(outputMessageIds.Count));
        //        }

        //        foreach (var item in nonConsentedItems)
        //        {
        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-Out";

        //            this.optOutProcessingServiceMock.Verify(processings =>
        //                processings.ModifyOptOutAsync(item),
        //                    Times.Exactly(outputMessageIds.Count));
        //        }

        //        string csvDifferences = CreateNewCsvList(
        //            differentIdentifiers,
        //            this.optOutConfiguration.OptOutFileRequireTrailingComma);

        //        consentedItems.Count.Should().Be(outputIdentifierConsentedList.Count);
        //        nonConsentedItems.Count.Should().Be(0);
        //        differences.Count.Should().Be(outputIdentifierConsentedList.Count);

        //        this.csvMapperProcessingServiceMock.Verify(processings =>
        //            processings.MapObjectToCsvAsync<OptOutIdentifier>(
        //                differentIdentifiers,
        //                this.optOutConfiguration.OptOutFileHasHeader,
        //                this.optOutConfiguration.OptOutFileRequireTrailingComma),
        //                    Times.Exactly(outputMessageIds.Count));

        //        Document document = new Document
        //        {
        //            DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
        //            FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
        //                $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
        //        };

        //        this.documentProcessingServiceMock.Verify(processings =>
        //            processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
        //                Times.Exactly(outputMessageIds.Count));
        //    }

        //    this.documentProcessingServiceMock.VerifyNoOtherCalls();
        //    this.meshProcessingServiceMock.VerifyNoOtherCalls();
        //    this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
        //    this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        //    this.documentProcessingServiceMock.VerifyNoOtherCalls();
        //}

        //[Fact]
        //public async Task ShouldRetrieveUpdatedMeshOptOutStatusesAndNotWriteDeltaIfNoChangesAsync()
        //{
        //    // Given
        //    bool withHeader = optOutConfiguration.OptOutFileHasHeader;
        //    string batchReference = GetRandomString();
        //    DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
        //    List<string> outputMessageIds = GetRandomStrings(GetRandomNumber(max: 3));
        //    List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, batchReference);

        //    List<OptOutIdentifier> outputIdentifierUnknownList =
        //        new List<OptOutIdentifier>();

        //    List<OptOutIdentifier> outputIdentifierConsentedList =
        //        CreateRandomListOfOptOutIdentifiers(GetRandomNumber(max: 3));

        //    List<OptOutIdentifier> outputIdentifierNonConsentedList =
        //        CreateRandomListOfOptOutIdentifiers(GetRandomNumber(max: 3));

        //    List<OptOutIdentifier> randomOutputIdentifierBatch = new List<OptOutIdentifier>();
        //    randomOutputIdentifierBatch.AddRange(outputIdentifierUnknownList);
        //    randomOutputIdentifierBatch.AddRange(outputIdentifierConsentedList);
        //    randomOutputIdentifierBatch.AddRange(outputIdentifierNonConsentedList);

        //    List<OptOut> randomUnkownConsentBatch =
        //        new List<OptOut>();

        //    List<OptOut> randomConsentBatch =
        //        CreateRandomOptOutsList(outputIdentifierConsentedList, batchReference, "Opt-In");

        //    List<OptOut> randomNonConsentBatch =
        //        CreateRandomOptOutsList(outputIdentifierNonConsentedList, batchReference, "Opt-Out");

        //    List<OptOut> outputBatch = new List<OptOut>();
        //    outputBatch.AddRange(randomUnkownConsentBatch);
        //    outputBatch.AddRange(randomConsentBatch);
        //    outputBatch.AddRange(randomNonConsentBatch);

        //    List<string> consentedIdentifiers = outputIdentifierConsentedList
        //        .Select(identifier => identifier.NhsNumber).ToList();

        //    List<OptOut> consentedItems =
        //        outputBatch.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber)).ToList();

        //    List<OptOut> nonConsentedItems =
        //        outputBatch.Except(consentedItems).ToList();

        //    List<OptOut> differences = new List<OptOut>();

        //    List<OptOutIdentifier> differentIdentifiers = differences
        //        .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

        //    this.dateTimeBrokerMock.Setup(broker =>
        //        broker.GetCurrentDateTimeOffset())
        //            .Returns(randomDateTimeOffset);

        //    // Given
        //    this.meshProcessingServiceMock.Setup(processings =>
        //        processings.RetrieveMessageIdsFromInboxAsync())
        //            .ReturnsAsync(outputMessageIds);

        //    foreach (string messageId in outputMessageIds)
        //    {
        //        var message = outputMessages.First(message => message.MessageId == messageId);

        //        // Get message
        //        this.meshProcessingServiceMock.Setup(processings =>
        //            processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
        //                .ReturnsAsync(message);

        //        // Map message content to object
        //        this.csvMapperProcessingServiceMock.Setup(processings =>
        //            processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
        //                .ReturnsAsync(outputIdentifierConsentedList);

        //        // Get original batch
        //        this.optOutProcessingServiceMock.Setup(processings =>
        //            processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
        //                .ReturnsAsync(outputBatch);

        //        string csvDifferences = CreateNewCsvList(
        //            differentIdentifiers,
        //            this.optOutConfiguration.OptOutFileRequireTrailingComma);

        //        this.csvMapperProcessingServiceMock.Setup(processings =>
        //            processings.MapObjectToCsvAsync<OptOutIdentifier>(
        //                differentIdentifiers,
        //                this.optOutConfiguration.OptOutFileHasHeader,
        //                this.optOutConfiguration.OptOutFileRequireTrailingComma))
        //                    .ReturnsAsync(csvDifferences);

        //        foreach (var item in consentedItems)
        //        {
        //            if (item.OptOutStatus != "Opt-In")
        //            {
        //                differences.Add(item);
        //            }

        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-In";

        //            this.optOutProcessingServiceMock.Setup(processings =>
        //                processings.ModifyOptOutAsync(item))
        //                    .ReturnsAsync(item);
        //        }

        //        foreach (var item in nonConsentedItems)
        //        {
        //            if (item.OptOutStatus != "Opt-Out")
        //            {
        //                differences.Add(item);
        //            }

        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-Out";

        //            this.optOutProcessingServiceMock.Setup(processings =>
        //                processings.ModifyOptOutAsync(item))
        //                    .ReturnsAsync(item);
        //        }

        //        Document document = new Document
        //        {
        //            DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
        //            FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
        //                $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
        //        };
        //    }

        //    // When
        //    await this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

        //    // Then
        //    this.dateTimeBrokerMock.Verify(broker =>
        //       broker.GetCurrentDateTimeOffset(),
        //            Times.AtLeastOnce());

        //    this.meshProcessingServiceMock.Verify(Processings =>
        //        Processings.RetrieveMessageIdsFromInboxAsync(),
        //            Times.Once);

        //    foreach (string messageId in outputMessageIds)
        //    {
        //        var message = outputMessages.First(message => message.MessageId == messageId);

        //        // Get message
        //        this.meshProcessingServiceMock.Verify(processings =>
        //            processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId),
        //                Times.Once());

        //        // Map message content to object
        //        this.csvMapperProcessingServiceMock.Verify(processings =>
        //            processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader),
        //                Times.Once());

        //        // Get original batch
        //        this.optOutProcessingServiceMock.Verify(processings =>
        //            processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
        //                Times.Exactly(outputMessageIds.Count));

        //        foreach (var item in consentedItems)
        //        {
        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-In";

        //            this.optOutProcessingServiceMock.Verify(processings =>
        //                processings.ModifyOptOutAsync(item),
        //                    Times.Exactly(outputMessageIds.Count));
        //        }

        //        foreach (var item in nonConsentedItems)
        //        {
        //            item.UpdatedDate = randomDateTimeOffset;
        //            item.CacheTime = randomDateTimeOffset;
        //            item.LastSentToMesh = randomDateTimeOffset;
        //            item.OptOutStatus = "Opt-Out";

        //            this.optOutProcessingServiceMock.Verify(processings =>
        //                processings.ModifyOptOutAsync(item),
        //                    Times.Exactly(outputMessageIds.Count));
        //        }

        //        string csvDifferences = CreateNewCsvList(
        //            differentIdentifiers,
        //            this.optOutConfiguration.OptOutFileRequireTrailingComma);

        //        consentedItems.Count.Should().Be(outputIdentifierConsentedList.Count);
        //        nonConsentedItems.Count.Should().Be(outputIdentifierNonConsentedList.Count);
        //        differences.Count.Should().Be(0);

        //        this.csvMapperProcessingServiceMock.Verify(processings =>
        //            processings.MapObjectToCsvAsync<OptOutIdentifier>(
        //                differentIdentifiers,
        //                this.optOutConfiguration.OptOutFileHasHeader,
        //                this.optOutConfiguration.OptOutFileRequireTrailingComma),
        //                    Times.Exactly(outputMessageIds.Count));

        //        Document document = new Document
        //        {
        //            DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
        //            FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
        //                $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
        //        };

        //        this.documentProcessingServiceMock.Verify(processings =>
        //            processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
        //                Times.Exactly(outputMessageIds.Count));
        //    }

        //    this.documentProcessingServiceMock.VerifyNoOtherCalls();
        //    this.meshProcessingServiceMock.VerifyNoOtherCalls();
        //    this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
        //    this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        //    this.documentProcessingServiceMock.VerifyNoOtherCalls();
        //}
    }
}
