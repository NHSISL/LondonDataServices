// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task ShouldRetrieveUpdatedMeshOptOutStatusChangesAsync()
        {
            bool withHeader = optOutConfiguration.OptOutFileHasHeader;
            string batchReference = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<string> outputMessageIds = GetRandomStrings(GetRandomNumber());
            List<MeshMessage> outputMessages = GetRandomMessages(outputMessageIds, batchReference);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // Given
            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(outputMessageIds);

            List<OptOutIdentifier> randomOutputIdentifiers = CreateRandomListOfOptOutIdentifiers();
            List<OptOutIdentifier> outputOptOutIdentifierConsentedList = randomOutputIdentifiers;
            List<OptOutIdentifier> randomOutputIdentifierBatch = CreateRandomListOfOptOutIdentifiers();
            randomOutputIdentifierBatch.AddRange(outputOptOutIdentifierConsentedList);
            List<OptOut> randomOptOutBatch = CreateRandomOptOutsList(randomOutputIdentifierBatch, batchReference);
            List<OptOut> outputOptOutBatch = randomOptOutBatch;

            List<string> consentedIdentifiers = outputOptOutIdentifierConsentedList
                    .Select(identifier => identifier.NhsNumber).ToList();

            List<OptOut> consentedoptOutItems =
                outputOptOutBatch.Where(optout => consentedIdentifiers.Contains(optout.NhsNumber)).ToList();

            // Work out non consented items based on mesh return items
            List<OptOut> nonConsentedoptOutItems = outputOptOutBatch.Except(consentedoptOutItems).ToList();

            List<OptOut> differences = new List<OptOut>();

            List<OptOutIdentifier> differentIdentifiers = differences
                .Select(item => new OptOutIdentifier { NhsNumber = item.NhsNumber }).ToList();

            foreach (string messageId in outputMessageIds)
            {
                var message = outputMessages.First(message => message.MessageId == messageId);

                // Get message
                this.meshProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAndAcknowledgeMessageByIdAsync(messageId))
                        .ReturnsAsync(message);

                // Map message content to object
                this.csvMapperProcessingServiceMock.Setup(processings =>
                    processings.MapCsvToObjectAsync<OptOutIdentifier>(message.StringContent, withHeader))
                        .ReturnsAsync(outputOptOutIdentifierConsentedList);

                // Get original batch
                this.optOutProcessingServiceMock.Setup(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference))
                        .ReturnsAsync(outputOptOutBatch);

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                this.csvMapperProcessingServiceMock.Setup(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        differentIdentifiers,
                        this.optOutConfiguration.OptOutFileHasHeader,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma))
                            .ReturnsAsync(csvDifferences);

                foreach (var item in consentedoptOutItems)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        item.UpdatedDate = randomDateTimeOffset;
                        item.CacheTime = randomDateTimeOffset;
                        item.OptOutStatus = "Opt-In";

                        this.optOutProcessingServiceMock.Setup(processings =>
                            processings.ModifyOptOutAsync(item))
                                .ReturnsAsync(item);

                        differences.Add(item);
                    }
                }

                foreach (var item in nonConsentedoptOutItems)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        item.UpdatedDate = randomDateTimeOffset;
                        item.CacheTime = randomDateTimeOffset;
                        item.OptOutStatus = "Opt-Out";

                        this.optOutProcessingServiceMock.Setup(processings =>
                            processings.ModifyOptOutAsync(item))
                                .ReturnsAsync(item);

                        differences.Add(item);
                    }
                }

                Document document = new Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
                        $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
                };
            }

            // When
            await this.optOutOrchestrationService.RetrieveUpdatedMeshOptOutStatusChangesAsync();

            // Then
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

                // Get original batch
                this.optOutProcessingServiceMock.Verify(processings =>
                    processings.RetrieveAllOptOutsByBatchReferenceAsync(batchReference),
                        Times.Exactly(outputMessageIds.Count));

                foreach (var item in consentedoptOutItems)
                {
                    if (item.OptOutStatus != "Opt-In")
                    {
                        item.UpdatedDate = randomDateTimeOffset;
                        item.CacheTime = randomDateTimeOffset;
                        item.OptOutStatus = "Opt-In";

                        this.optOutProcessingServiceMock.Verify(processings =>
                            processings.ModifyOptOutAsync(item),
                                Times.AtLeastOnce());

                        differences.Add(item);
                    }
                }

                foreach (var item in nonConsentedoptOutItems)
                {
                    if (item.OptOutStatus != "Opt-Out")
                    {
                        item.UpdatedDate = randomDateTimeOffset;
                        item.CacheTime = randomDateTimeOffset;
                        item.OptOutStatus = "Opt-Out";

                        this.optOutProcessingServiceMock.Verify(processings =>
                            processings.ModifyOptOutAsync(item),
                                Times.AtLeastOnce());

                        differences.Add(item);
                    }
                }

                string csvDifferences = CreateNewCsvList(
                    differentIdentifiers,
                    this.optOutConfiguration.OptOutFileRequireTrailingComma);

                this.csvMapperProcessingServiceMock.Verify(processings =>
                    processings.MapObjectToCsvAsync<OptOutIdentifier>(
                        differentIdentifiers,
                        this.optOutConfiguration.OptOutFileHasHeader,
                        this.optOutConfiguration.OptOutFileRequireTrailingComma),
                            Times.Exactly(outputMessageIds.Count));

                Document document = new Document
                {
                    DocumentData = Encoding.ASCII.GetBytes(csvDifferences),
                    FileName = $"{optOutConfiguration.OutputFolder}/{batchReference}_Response_" +
                        $"{randomDateTimeOffset.ToString("yyyyMMddHHmmss")}.csv",
                };

                this.documentProcessingServiceMock.Verify(processings =>
                    processings.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Exactly(outputMessageIds.Count));
            }

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
